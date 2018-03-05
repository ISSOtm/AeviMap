using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeviMap
{
    class Tileset
    {
        public byte nbOfBlocks;
        public byte nbOfPalettes = 8;

        private byte tilesetID;
        private byte bank;
        private UInt16 ptr;

        private byte[][] tiles = new byte[0x200][];
        private Block[] blocks;
        private CGBPalette[] BGPalettes;

        public Tileset(INI_File properties, GB_ROM ROM, byte tilesetID)
        {
            this.nbOfBlocks = (byte)properties.GetProperty("nbofblocks");
            blocks = new Block[this.nbOfBlocks];
            BGPalettes = new CGBPalette[this.nbOfBlocks];

            this.tiles[0] = new byte[16]
            {
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00
            };
            for(UInt16 i = 1; i < 0x200; i++)
            {
                this.tiles[i] = new byte[16] {
                    0xAA, 0xAA, 0x55, 0x55,
                    0xAA, 0xAA, 0x55, 0x55,
                    0xAA, 0xAA, 0x55, 0x55,
                    0xAA, 0xAA, 0x55, 0x55
                };
            }

            this.tilesetID = tilesetID;

            // Get the tileset's data location
            byte bank = (byte)properties.GetProperty("tilesetptrsbank");
            UInt16 addr = (UInt16)(properties.GetProperty("tilesetbanksptr") + tilesetID);
            this.bank = ROM.GetByte(bank, addr);

            addr = (UInt16)(properties.GetProperty("tilesetptrsptr") + tilesetID * 2);
            this.ptr = ROM.GetShort(bank, addr);

            // Read tiles
            for (UInt16 targetTileID = 0x80; targetTileID < 0x200; )
            {
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
                        UInt16 tileBlockPtr = ROM.GetByte(this.bank, this.ptr);
                        this.ptr += 2;

                        // All tiles are contiguous
                        for(byte i = 0; i < nbOfTiles; i++)
                        {
                            // Read the tile's data
                            this.tiles[targetTileID] = ROM.GetBytes(tileBlockBank, tileBlockPtr, 16);

                            // Advance to next tile
                            tileBlockPtr += 16;
                            targetTileID++;
                        }
                    }
                } while (nbOfTiles != 0);

                // Advance to next tile bank
                targetTileID = (UInt16)((targetTileID & 0x100) + 0x100);
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
            for(byte i = 0; i < this.nbOfPalettes; i++)
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
    }
}
