using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeviMap
{
    class GB_ROM
    {
        private byte[] rawData;
        // These store the loaded assets to avoid re-loading them twice (especially useful for tilesets)
        private Tileset[] tilesets;
        private Map[] maps;

        private byte nbOfTilesets;
        private byte nbOfMaps;


        public GB_ROM(string ROMPath, INI_File ini)
        {
            // May fail
            this.rawData = File.ReadAllBytes(ROMPath);

            this.nbOfTilesets = (byte)ini.GetProperty("nboftilesets");
            this.nbOfMaps = (byte)ini.GetProperty("nbofmaps");

            this.tilesets = new Tileset[this.nbOfTilesets];
            this.maps = new Map[this.nbOfMaps];
        }


        public byte[] GetBytes(byte bank, UInt16 addr, UInt16 len)
        {
            if(addr >= 0x8000 || addr + len > 0x8000)
            {
                throw new IndexOutOfRangeException("Attempted to read " + len + " bytes from 0x" + MapEditor.decToHex(addr));
            }
            if(len == 0)
            {
                throw new ArgumentOutOfRangeException("Attempted to read 0 bytes");
            }

            byte[] data = new byte[len];
            uint offset = addr;
            if(addr > 0x3FFF && bank > 1)
            {
                offset += (uint)(bank - 1) * 0x4000;
            }

            for(UInt16 i = 0; i < len; i++)
            {
                data[i] = this.rawData[offset];
                offset++;
            }

            return data;
        }

        public byte GetByte(byte bank, UInt16 addr)
        {
            return this.GetBytes(bank, addr, 1)[0];
        }

        public UInt16 GetShort(byte bank, UInt16 addr)
        {
            byte[] rawShort = this.GetBytes(bank, addr, 2);
            return (UInt16)(rawShort[1] * 256 + rawShort[0]);
        }


        public Map GetMap(byte mapID, INI_File ini)
        {
            if(this.maps[mapID] == null)
            {
                this.maps[mapID] = new Map(ini, this, mapID);
            }
            return this.maps[mapID];
        }

        public Tileset GetTileset(byte tilesetID, INI_File ini)
        {
            if(this.tilesets[tilesetID] == null)
            {
                this.tilesets[tilesetID] = new Tileset(ini, this, tilesetID);
            }
            return this.tilesets[tilesetID];
        }
    }
}
