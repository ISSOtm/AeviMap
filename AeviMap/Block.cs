using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace AeviMap
{
    public class Block
    {
        private byte size;
        private Bitmap bmp;

        public byte[] rawIDs { get; }
        public byte[] attributes { get; }
        
        
        // Block's          tile IDs,      tile's attribs,        loaded tiles' data,    loaded palettes
        public Block(byte[] rawIDs, byte[] attributes, byte[][][] rawTiles, CGBPalette[] palettes, INI_File INIFile)
        {
            this.size = (byte)INIFile.GetProperty("sizeofblock");
            this.rawIDs = (byte[])rawIDs.Clone();
            this.attributes = (byte[])attributes.Clone();


            UInt16[] IDs = new UInt16[4];

            // First, convert raw tiles IDs to IDs within the rawTiles array
            for (uint j = 0; j < 4; j++)
            {
                if (rawIDs[j] < 0x80)
                {
                    // Filter special tiles here, currently they will be displayed blank
                    IDs[j] = 0x100;
                }
                else
                {
                    IDs[j] = rawIDs[j];

                    // If we aren't in VRAM bank 1, go in the lower half of the table
                    if ((attributes[j] & 0x08) != 0x08)
                    {
                        IDs[j] -= 0x100;
                    }
                }
            }


            this.bmp = new Bitmap(this.size, this.size, PixelFormat.Format16bppRgb555);


            // Now, generate the pixel data.
            // For each line of pixels (there are 16)
            for (byte lineID = 0; lineID < this.size; lineID++)
            {
                // ID of the tile that will be processed
                UInt16 curTileID;
                byte VRAMBank;
                // These will hold the bitplanes for the current 8 pixels
                byte tileLayer0, tileLayer1;
                // Mask used to "unroll" bitplanes
                byte planeMask;

                // Index to read from in rawIDs and attributes
                byte curIndex = (byte)(lineID >> 3 & 1); // Tile IDs depend on which horizontal half of the block we are on
                // Palette the current tile is using
                CGBPalette curPalette;

                for (UInt16 x = 0; x < this.size; )
                {
                    // When we reach the edge of a tile (x = 0 or x = 8), we need to load bitplanes for the next 8 pixels

                    // If we're not at x = 0, then we need to pull data from the right tile (instead of the left)
                    if (x != 0)
                    {
                        curIndex += 2; // This will be cancelled by the assignment right before the for(x)
                    }
                    // Get the ID of the tile we're going to draw
                    curTileID = rawIDs[curIndex];

                    VRAMBank = (byte)((attributes[curIndex] & 0x08) == 0 ? 0 : 1);

                    // This holds the index at which we're going to read tile data from
                    byte tileRow = (byte)(lineID & 0x07);
                    // Apply vertical flip by changing the index
                    if ((attributes[curIndex] & 0x40) == 0x40)
                    {
                        tileRow = (byte)(7 - tileRow);
                    }

                    // Grab the bitplanes for conversion
                    tileLayer0 = rawTiles[VRAMBank][curTileID][tileRow * 2];
                    tileLayer1 = rawTiles[VRAMBank][curTileID][tileRow * 2 + 1];

                    // Apply horizontal flip by changing the mask used below (the way the planes will be shifted will also be changed)
                    planeMask = (byte)(((attributes[curIndex] & 0x20) == 0x20) ? 0x01 : 0x80);

                    curPalette = palettes[attributes[curIndex] & 0x7];

                    // Iterate over current tile (ie. until next tile edge is reached)
                    for( ; planeMask != 0; x++)
                    {
                        // Get the color ID of the current pixel
                        byte pixelColorIndex = 0;
                        if ((tileLayer0 & planeMask) == planeMask)
                        {
                            pixelColorIndex++;
                        }
                        if ((tileLayer1 & planeMask) == planeMask)
                        {
                            pixelColorIndex += 2;
                        }

                        // Shift the mask, depending on horizontal flip attribute
                        if((attributes[curIndex] & 0x20) == 0x20)
                        {
                            planeMask <<= 1;
                        } else
                        {
                            planeMask >>= 1;
                        }
                        
                        this.bmp.SetPixel(x, lineID, curPalette.GetColor(pixelColorIndex));
                    }
                }
            }
        }

        public Bitmap GetBMP()
        {
            return (Bitmap)this.bmp.Clone();
        }
    }
}
