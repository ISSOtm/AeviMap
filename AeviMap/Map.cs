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
        private UInt16 addr;

        private byte flags;
        private MusicID musicID;
        private Tileset tileset;
        private UInt16 MapScriptPtr;
        private uint height;
        private uint width;
        private UInt16 LoadingPtr;

        private byte nbOfInteractions;
        private Interaction[] Interactions;

        private byte nbOfNPCs;
        private NPC[] NPCs;
        private byte nbOfNPCScripts;
        private UInt16 NPCScriptsPtr;
        private byte nbOfNPCTiles;
        private byte[] NPCTileBanks;
        private UInt16[] NPCTilePtrs;

        private CGBPalette[] OBJPalettes = new CGBPalette[8];

        private byte nbOfWarpToPoints;
        private WarpTo[] WarpToPoints;


        private byte[,] rawMap;

        public Map(INI_File properties, GB_ROM ROM, byte mapID)
        {
            this.mapID = mapID;

            // Get the map's data location
            byte bank = (byte)properties.GetProperty("mapptrsbank");
            UInt16 addr = (UInt16)(properties.GetProperty("mapbanksptr") + mapID);
            this.bank = ROM.GetByte(bank, addr);

            addr = (UInt16)(properties.GetProperty("mapptrsptr") + mapID * 2);
            UInt16 ptr = ROM.GetShort(bank, addr);
            this.addr = ptr;

            // Read header
            this.flags = ROM.GetByte(this.bank, ptr);
            ptr++;

            this.musicID = (MusicID)ROM.GetByte(this.bank, ptr);
            ptr++;

            if(ROM.GetByte(this.bank, ptr++) == 0)
            {
                // Static tileset, easy, the ID is just there to be read
                this.tileset = ROM.GetTileset(ROM.GetByte(this.bank, ptr), properties);
            }
            else
            {
                // Dynamic tileset, this depends on a function... uh oh. We'll fall back to a default tileset
                // TODO : implement fallback tilesets
                this.tileset = ROM.GetTileset(0, properties);
                ptr++; // Skip extra byte (func *ptr* instead of tileset ID *byte*)
            }
            ptr++;

            this.MapScriptPtr = ROM.GetShort(this.bank, ptr);
            ptr += 2;

            this.width = ROM.GetByte(this.bank, ptr);
            ptr++;
            this.height = ROM.GetByte(this.bank, ptr);
            ptr++;

            this.LoadingPtr = ROM.GetShort(this.bank, ptr);
            ptr += 2;

            // Read interactions
            this.nbOfInteractions = ROM.GetByte(this.bank, ptr++);
            this.Interactions = new Interaction[this.nbOfInteractions];

            for (byte i = 0; i < this.nbOfInteractions; i++)
            {
                byte type = ROM.GetByte(this.bank, ptr);
                this.Interactions[i] = (type & 2) != 0 ? (Interaction)new LoadingZone(ROM, this.bank, ptr) : (Interaction)new InteractionTrigger(ROM, this.bank, ptr);

                byte size = this.Interactions[i].size;
                ptr += size;
            }

            // Skip NPCs
            this.nbOfNPCs = ROM.GetByte(this.bank, ptr++);
            if(this.nbOfNPCs != 0)
            {
                this.NPCs = new NPC[this.nbOfNPCs];
                for (byte i = 0; i < this.nbOfNPCs; i++)
                {
                    this.NPCs[i] = new NPC(ROM, this.bank, ptr);
                    ptr += 14;
                }

                this.nbOfNPCScripts = ROM.GetByte(this.bank, ptr++);
                this.NPCScriptsPtr = ROM.GetShort(this.bank, ptr);
                ptr += 2;

                this.nbOfNPCTiles = ROM.GetByte(this.bank, ptr++);
                this.NPCTileBanks = new byte[this.nbOfNPCTiles];
                this.NPCTilePtrs = new UInt16[this.nbOfNPCTiles];
                for(byte i = 0; i < this.nbOfNPCTiles; i++)
                {
                    // 0 is a special trap; otherwise, there's also a 2-byte pointer following it
                    this.NPCTileBanks[i] = ROM.GetByte(this.bank, ptr++);
                    if (this.NPCTileBanks[i] != 0) {
                        this.NPCTilePtrs[i] = ROM.GetShort(this.bank, ptr);
                        ptr += 2;
                    } else
                    {
                        this.NPCTileBanks[i] = (byte)properties.GetProperty("evietilesbank");
                        this.NPCTilePtrs[i] = properties.GetProperty("evietilesptr");
                    }
                }
            }
            
            this.OBJPalettes[0] = new CGBPalette(ROM.GetBytes(1, properties.GetProperty("objpalette0ptr"), CGBPalette.paletteSize));
            for(byte i = 1; i < 8; i++)
            {
                UInt16 PalettePtr = ROM.GetShort(this.bank, ptr);
                ptr += 2;
                if(PalettePtr == 0)
                {
                    PalettePtr = properties.GetProperty("siblingpaletteptr");
                }

                this.OBJPalettes[i] = new CGBPalette(ROM.GetBytes(1, PalettePtr, CGBPalette.paletteSize));
            }

            // Skip warp-to points
            this.nbOfWarpToPoints = ROM.GetByte(this.bank, ptr++);
            this.WarpToPoints = new WarpTo[this.nbOfWarpToPoints];
            for(byte i = 0; i < this.nbOfWarpToPoints; i++)
            {
                this.WarpToPoints[i] = new WarpTo(ROM, this.bank, ptr);
                ptr += 16;
            }


            // Read raw map
            byte[] rawMap = ROM.GetBytes(this.bank, ptr, (UInt16)(this.height * this.width));
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
