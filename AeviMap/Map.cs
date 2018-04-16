using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeviMap
{
    public class Map
    {
        private byte mapID;
        private byte bank;
        private UInt16 addr;
        private bool UnsavedChanges = false;

        private MapHeader Header_Internal = new MapHeader();
        /// <summary>
        /// The map's header, collecting all the metadata encoded within the game
        /// (Metadata only relevant to the manipulation of the map, such as the address, is encoded separately in the Map object)
        /// </summary>
        /// <remarks>
        /// This allows external modifications of the map's header, while allowing them to be atomic.
        /// ...Of course, this kinda seems to defeat encapsulation, but there's a Header Editor functionality anyways, so lol
        /// </remarks>
        internal MapHeader Header
        {
            get {
                return this.Header_Internal.Clone();
            }
            set
            {
                this.Header_Internal = value;
            }
        }

        // Please tell me there's a better way than to do this... monstrosity... of getters and setters.
        private byte flags {
            get
            {
                return this.Header_Internal.flags;
            }
            set
            {
                this.Header_Internal.flags = value;
            }
        }
        private MusicID musicID
        {
            get
            {
                return this.Header_Internal.musicID;
            }
            set
            {
                this.Header_Internal.musicID = value;
            }
        }
        private Tileset tileset
        {
            get
            {
                return this.Header_Internal.tileset;
            }
            set
            {
                this.Header_Internal.tileset = value;
            }
        }
        public byte TilesetID
        {
            get
            {
                return this.tileset.ID;
            }
        }
        private UInt16 MapScriptPtr
        {
            get
            {
                return this.Header_Internal.MapScriptPtr;
            }
            set
            {
                this.Header_Internal.MapScriptPtr = value;
            }
        }
        private uint height
        {
            get
            {
                return this.Header_Internal.height;
            }
            set
            {
                this.Header_Internal.height = value;
            }
        }
        private uint width
        {
            get
            {
                return this.Header_Internal.width;
            }
            set
            {
                this.Header_Internal.width = value;
            }
        }
        private UInt16 LoadingPtr
        {
            get
            {
                return this.Header_Internal.LoadingPtr;
            }
            set
            {
                this.Header_Internal.LoadingPtr = value;
            }
        }

        private byte nbOfInteractions
        {
            get
            {
                return this.Header_Internal.nbOfInteractions;
            }
            set
            {
                this.Header_Internal.nbOfInteractions = value;
            }
        }
        private Interaction[] Interactions
        {
            get
            {
                return this.Header_Internal.Interactions;
            }
            set
            {
                this.Header_Internal.Interactions = value;
            }
        }

        private byte nbOfNPCs
        {
            get
            {
                return this.Header_Internal.nbOfNPCs;
            }
            set
            {
                this.Header_Internal.nbOfNPCs = value;
            }
        }
        private NPC[] NPCs
        {
            get
            {
                return this.Header_Internal.NPCs;
            }
            set
            {
                this.Header_Internal.NPCs = value;
            }
        }
        private byte nbOfNPCScripts
        {
            get
            {
                return this.Header_Internal.nbOfNPCScripts;
            }
            set
            {
                this.Header_Internal.nbOfNPCScripts = value;
            }
        }
        private UInt16 NPCScriptsPtr
        {
            get
            {
                return this.Header_Internal.NPCScriptsPtr;
            }
            set
            {
                this.Header_Internal.NPCScriptsPtr = value;
            }
        }
        private byte nbOfNPCTiles
        {
            get
            {
                return this.Header_Internal.nbOfNPCTiles;
            }
            set
            {
                this.Header_Internal.nbOfNPCTiles = value;
            }
        }
        private byte[] NPCTileBanks
        {
            get
            {
                return this.Header_Internal.NPCTileBanks;
            }
            set
            {
                this.Header_Internal.NPCTileBanks = value;
            }
        }
        private UInt16[] NPCTilePtrs
        {
            get
            {
                return this.Header_Internal.NPCTilePtrs;
            }
            set
            {
                this.Header_Internal.NPCTilePtrs = value;
            }
        }

        private CGBPalette[] OBJPalettes
        {
            get
            {
                return this.Header_Internal.OBJPalettes;
            }
            set
            {
                this.Header_Internal.OBJPalettes = value;
            }
        }

        private byte nbOfWarpToPoints
        {
            get
            {
                return this.Header_Internal.nbOfWarpToPoints;
            }
            set
            {
                this.Header_Internal.nbOfWarpToPoints = value;
            }
        }
        private WarpTo[] WarpToPoints
        {
            get
            {
                return this.Header_Internal.WarpToPoints;
            }
            set
            {
                this.Header_Internal.WarpToPoints = value;
            }
        }


        private byte[,] rawMap;


        // Init a barebones map
        public Map(INI_File ini, GB_ROM ROM, byte TilesetID, byte height, byte width, byte FillerBlockID)
        {
            this.tileset = ROM.GetTileset(TilesetID, ini);
            this.height = height;
            this.width = width;

            this.rawMap = new byte[this.height, this.width];
            for(byte y = 0; y < this.height; y++)
            {
                for(byte x = 0; x < this.width; x++)
                {
                    this.rawMap[y, x] = FillerBlockID;
                }
            }
        }

        // Init an even more barebones map -- only use this to clone a map!!
        private Map()
        {
            this.UnsavedChanges = true;
        }

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
            this.UnsavedChanges = true;
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

        public void SavedChanges()
        {
            this.UnsavedChanges = false;
        }

        public bool HasUnsavedChanges()
        {
            return this.UnsavedChanges;
        }


        public Size GetSize()
        {
            return new Size((int)this.width, (int)this.height);
        }

        public void Resize(Rectangle NewSize, byte DefaultBlock)
        {
            if((byte)NewSize.Height != NewSize.Height || (byte)NewSize.Width != NewSize.Width)
            {
                throw new ArgumentOutOfRangeException("The new map size is invalid! Width and height must be within byte range");
            }

            byte[,] NewRawMap = new byte[NewSize.Height, NewSize.Width];
            for(byte dy = 0; dy < NewSize.Height; dy++)
            {
                var y = NewSize.Y + dy;
                for(byte dx = 0; dx < NewSize.Width; dx++)
                {
                    var x = NewSize.X + dx;
                    NewRawMap[dy, dx] = (y < 0 || x < 0 || y >= this.height || x >= this.width) ? DefaultBlock : this.rawMap[y,x];
                }
            }

            this.rawMap = NewRawMap;
        }


        public Map Clone()
        {
            Map Clone = new Map();
            Clone.Header = this.Header_Internal.Clone();
            Clone.rawMap = (byte[,])this.rawMap.Clone();

            return Clone;
        }
    }
}
