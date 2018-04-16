using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeviMap
{
    /// <summary>
    /// A Map's header. Simple collection of all of a map's attributes, to simplify creation and editing of Maps.
    /// </summary>
    internal class MapHeader
    {
        public byte flags;
        public MusicID musicID;
        public Tileset tileset;
        public UInt16 MapScriptPtr;
        public uint height;
        public uint width;
        public UInt16 LoadingPtr;

        public byte nbOfInteractions;
        public Interaction[] Interactions = Array.Empty<Interaction>();

        public byte nbOfNPCs;
        public NPC[] NPCs = Array.Empty<NPC>();
        public byte nbOfNPCScripts;
        public UInt16 NPCScriptsPtr;
        public byte nbOfNPCTiles;
        public byte[] NPCTileBanks = Array.Empty<byte>();
        public UInt16[] NPCTilePtrs = Array.Empty<UInt16>();

        public CGBPalette[] OBJPalettes = new CGBPalette[8];

        public byte nbOfWarpToPoints;
        public WarpTo[] WarpToPoints = Array.Empty<WarpTo>();


        public MapHeader Clone()
        {
            MapHeader Clone = new MapHeader();

            Clone.flags = flags;
            Clone.musicID = musicID;
            Clone.tileset = tileset;
            Clone.MapScriptPtr = MapScriptPtr;
            Clone.height = height;
            Clone.width = width;
            Clone.LoadingPtr = LoadingPtr;

            Clone.nbOfInteractions = nbOfInteractions;
            Clone.Interactions = (Interaction[])Interactions.Clone();

            Clone.nbOfNPCs = nbOfNPCs;
            Clone.NPCs = (NPC[])NPCs.Clone();
            Clone.nbOfNPCScripts = nbOfNPCScripts;
            Clone.NPCScriptsPtr = NPCScriptsPtr;
            Clone.nbOfNPCTiles = nbOfNPCTiles;
            Clone.NPCTileBanks = (byte[])NPCTileBanks.Clone();
            Clone.NPCTilePtrs = (UInt16[])NPCTilePtrs.Clone();

            Clone.OBJPalettes = (CGBPalette[])OBJPalettes.Clone();

            Clone.nbOfWarpToPoints = nbOfWarpToPoints;
            Clone.WarpToPoints = (WarpTo[])WarpToPoints.Clone();

            return Clone;
        }
    }
}
