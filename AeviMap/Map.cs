using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeviMap
{
    class Map
    {
        private byte mapID;
        private byte bank;
        private UInt16 ptr;

        private byte flags;
        private byte musicID;
        private Tileset tileset;
        private uint height;
        private uint width;
        private byte nbOfInteractions;
        private byte nbOfNPCs;
        private byte[,] rawMap;

        public Map(INI_File properties, GB_ROM ROM, byte mapID)
        {
            this.mapID = mapID;

            // Get the map's data location
            byte bank = (byte)properties.GetProperty("mapptrsbank");
            UInt16 addr = (UInt16)(properties.GetProperty("mapbanksptr") + mapID);
            this.bank = ROM.GetByte(bank, addr);

            addr = (UInt16)(properties.GetProperty("mapptrsptr") + mapID * 2);
            this.ptr = ROM.GetShort(bank, addr);

            // Read header
            this.flags = ROM.GetByte(this.bank, this.ptr);
            this.ptr++;

            this.musicID = ROM.GetByte(this.bank, this.ptr);
            this.ptr++;

            if(ROM.GetByte(this.bank, this.ptr++) == 0)
            {
                // Static tileset, easy, the ID is just there to be read
                this.tileset = ROM.GetTileset(ROM.GetByte(this.bank, this.ptr), properties);
            }
            else
            {
                // Dynamic tileset, this depends on a function... uh oh. We'll fall back to a default tileset
                // TODO : implement fallback tilesets
                this.tileset = ROM.GetTileset(0, properties);
                this.ptr++; // Skip extra byte (func *ptr* instead of tileset ID *byte*)
            }

            this.ptr += 3; // Skip over tileset byte and map script ptr

            this.width = ROM.GetByte(this.bank, this.ptr);
            this.ptr++;
            this.height = ROM.GetByte(this.bank, this.ptr);

            this.ptr += 3; // Skip height and loading ptr

            // Skip interactions (that's non-trivial, sadly)
            this.nbOfInteractions = ROM.GetByte(this.bank, this.ptr);
            this.ptr++;
            for (byte i = 0; i < this.nbOfInteractions; i++)
            {
                // If there is a flag dep, then skip it
                if((ROM.GetByte(this.bank, this.ptr++) & 0x80) != 0) {
                    this.ptr += 2;
                }
                this.ptr += 16;
            }

            // Skip NPCs
            this.nbOfNPCs = ROM.GetByte(this.bank, this.ptr++);
            if(nbOfNPCs != 0)
            {
                // Maybe the map editor will be able to edit NPCs at some point?
                // If so, that would be implemented here

                // Skip NPCs proper                and NPC scripts
                this.ptr += (UInt16)(nbOfNPCs * 14 + 3);

                // Skip NPC tiles (another non-trivial skip...)
                for(byte i = 0, nbOfNPCTiles = ROM.GetByte(this.bank, this.ptr++); i < nbOfNPCTiles; i++)
                {
                    // 0 is a special trap; otherwise, there's also a 2-byte pointer following it
                    if(ROM.GetByte(this.bank, this.ptr++) != 0) {
                        this.ptr += 2;
                    }
                }
            }

            // Skip OBJ palette ptrs
            this.ptr += 7 * 2;

            // Skip warp-to points
            this.ptr += (UInt16)(ROM.GetByte(this.bank, this.ptr) * 16 + 1);


            // Read raw map
            byte[] rawMap = ROM.GetBytes(this.bank, this.ptr, (UInt16)(this.height * this.width));
            this.rawMap = new byte[this.height, this.width];

            for (byte y = 0; y < this.height; y++)
            {
                for (byte x = 0; x < this.width; x++)
                {
                    this.rawMap[y, x] = rawMap[x + y * this.width];
                }
            }
        }


        public byte GetBlockIDAt(uint x, uint y)
        {
            return this.rawMap[y, x];
        }

        public Bitmap GetBlockBMP(byte ID)
        {
            return this.tileset.GetBlockBMP(ID);
        }

        public Bitmap GetBlockBMPAt(uint x, uint y)
        {
            return this.GetBlockBMP(this.GetBlockIDAt(x, y));
        }

        public void SetBlockIDAt(uint x, uint y, byte ID)
        {
            this.rawMap[y, x] = ID;
        }


        public byte[] GetBlocks()
        {
            byte[] blocks = new byte[this.rawMap.Length];
            uint index = 0;

            for(uint y = 0; y < this.height; y++)
            {
                for(uint x = 0; x < this.width; x++)
                {
                    blocks[index] = this.GetBlockIDAt(x, y);
                    index++;
                }
            }

            return blocks;
        }


        public Size GetSize()
        {
            return new Size((int)this.width, (int)this.height);
        }
    }
}
