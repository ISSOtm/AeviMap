using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeviMap
{
    public class Tileset
    {
        public byte nbOfBlocks;
        public byte nbOfPalettes = 8;

        private byte tilesetID;
        private byte bank;
        private UInt16 ptr;

        private byte[][][] tiles = new byte[2][][];
        private Block[] blocks;
        private CGBPalette[] BGPalettes;

        public Tileset(INI_File properties, GB_ROM ROM, byte tilesetID)
        {
            this.nbOfBlocks = (byte)properties.GetProperty("nbofblocks");
            blocks = new Block[this.nbOfBlocks];
            BGPalettes = new CGBPalette[this.nbOfBlocks];

            for (byte VRAMBank = 0; VRAMBank <= 1; VRAMBank++)
            {
                this.tiles[VRAMBank] = new byte[0x100][];

                this.tiles[VRAMBank][0] = new byte[16]
                {
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00
                };
                for (UInt16 i = 1; i < 0x100; i++)
                {
                    this.tiles[VRAMBank][i] = new byte[16] {
                        0xAA, 0xAA, 0x55, 0x55,
                        0xAA, 0xAA, 0x55, 0x55,
                        0xAA, 0xAA, 0x55, 0x55,
                        0xAA, 0xAA, 0x55, 0x55
                    };
                }
            }

            this.tilesetID = tilesetID;

            // Get the tileset's data location
            byte bank = (byte)properties.GetProperty("tilesetptrsbank");
            UInt16 addr = (UInt16)(properties.GetProperty("tilesetbanksptr") + tilesetID);
            this.bank = ROM.GetByte(bank, addr);

            addr = (UInt16)(properties.GetProperty("tilesetptrsptr") + tilesetID * 2);
            this.ptr = ROM.GetShort(bank, addr);

            // Read tiles
            for (byte VRAMBank = 0; VRAMBank <= 1; VRAMBank++)
            {
                UInt16 targetTileID = 0x80;
                byte nbOfTiles;
                do
                {
                    // Read nb of tiles in this block
                    nbOfTiles = ROM.GetByte(this.bank, this.ptr++);

                    // If there are 0 tiles, it's a terminator
                    if(nbOfTiles != 0)
                    {
                        // Read ptr to block
                        byte tileBlockBank = ROM.GetByte(this.bank, this.ptr++);
                        UInt16 tileBlockPtr = ROM.GetShort(this.bank, this.ptr);
                        this.ptr += 2;

                        // All tiles are contiguous
                        for(byte i = 0; i < nbOfTiles; i++)
                        {
                            // Read the tile's data
                            this.tiles[VRAMBank][targetTileID] = ROM.GetBytes(tileBlockBank, tileBlockPtr, 16);

                            // Advance to next tile
                            tileBlockPtr += 16;
                            targetTileID++;
                        }
                    }
                } while (nbOfTiles != 0);
            }


            byte[][] rawIDs = new byte[this.nbOfBlocks][];
            byte[][] attributes = new byte[this.nbOfBlocks][];
            for (byte blockID = 0; blockID < this.nbOfBlocks; blockID++)
            {
                rawIDs[blockID] = new byte[4];
                attributes[blockID] = new byte[4];

                for (byte i = 0; i < 4; i++)
                {
                    rawIDs[blockID][i] = ROM.GetByte(this.bank, this.ptr++);
                    attributes[blockID][i] = ROM.GetByte(this.bank, this.ptr++);
                }

            }


            // Skip tile metadata
            this.ptr += 256;


            // Skip animations
            byte nbOfAnimations = ROM.GetByte(this.bank, this.ptr++);
            this.ptr += (UInt16)(5 * nbOfAnimations);


            // Load palettes
            this.BGPalettes[0] = new CGBPalette(ROM.GetBytes(
                (byte)properties.GetProperty("palettesbank"), properties.GetProperty("bgpalette0ptr"), CGBPalette.paletteSize
            ));
            for(byte i = 1; i < this.nbOfPalettes; i++)
            {
                UInt16 paletteAddr = ROM.GetShort(this.bank, this.ptr);
                this.ptr += 2;

                this.BGPalettes[i] = new CGBPalette(ROM.GetBytes(
                    (byte)properties.GetProperty("palettesbank"), paletteAddr, CGBPalette.paletteSize
                ));
            }



            // Generate blocks
            for (byte blockID = 0; blockID < this.nbOfBlocks; blockID++)
            {
                this.blocks[blockID] = new Block(rawIDs[blockID], attributes[blockID], this.tiles, this.BGPalettes, properties);
            }
        }


        public Bitmap GetBlockBMP(byte blockID)
        {
            return this.blocks[blockID].GetBMP();
        }


        public Bitmap GetPreviewImage()
        {
            // Try to determine the palette each tile uses
            byte[][] Palettes = new byte[2][] { new byte[256], new byte[256] };
            for(int i = 0; i < 256; i++)
            {
                Palettes[0][i] = 0xff;
                Palettes[1][i] = 0xff;
            }

            for(byte blockID = 0; blockID < this.nbOfBlocks; blockID++)
            {
                byte[] IDs = this.blocks[blockID].rawIDs;
                byte[] attributes = this.blocks[blockID].attributes;

                for(byte i = 0; i < 4; i++)
                {
                    byte bank = (byte)(((attributes[i] & 0x08) != 0) ? 1 : 0);
                    if (Palettes[bank][IDs[i]] == 0xff)
                    {
                        Palettes[bank][IDs[i]] = (byte)(attributes[i] & 0x07);
                    }
                }
            }


            Size size = tilesetDB.unknown_tileset.Size;
            Bitmap BMP = new Bitmap(size.Width, size.Height, PixelFormat.Format16bppRgb555);

            UInt16 TileID = 128;
            byte VRAMBank = 0;

            for (byte y = 0; y < size.Height / 8; y++)
            {
                for(byte x = 0; x < size.Width / 8; x++)
                {
                    for(byte dy = 0; dy < 8; dy++)
                    {
                        byte layer0 = this.tiles[VRAMBank][TileID][dy * 2];
                        byte layer1 = this.tiles[VRAMBank][TileID][dy * 2 + 1];
                        for(byte dx = 0; dx < 8; dx++)
                        {
                            byte Color = 0;
                            if((layer0 & 0x80) != 0)
                            {
                                Color++;
                            }
                            if((layer1 & 0x80) != 0)
                            {
                                Color += 2;
                            }

                            byte PaletteID = Palettes[VRAMBank][TileID];
                            if(PaletteID == 0xff)
                            {
                                PaletteID = 0;
                            }
                            BMP.SetPixel(x * 8 + dx, y * 8 + dy, this.BGPalettes[PaletteID].GetColor(Color));

                            layer0 <<= 1;
                            layer1 <<= 1;
                        }
                    }

                    TileID++;
                    if(TileID == 256)
                    {
                        TileID = 128;
                        VRAMBank = 1;
                    }
                }
            }

            return BMP;
        }
    }
}
