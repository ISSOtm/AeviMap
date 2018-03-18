using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeviMap
{
    public class GB_ROM
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


        /// <summary>
        /// Retrieves an array of bytes from the ROM.
        /// </summary>
        /// <param name="bank">The bank from which to fetch data from.</param>
        /// <param name="addr">The address at which to fetch data from.</param>
        /// <param name="len">The number of bytes to fetch.</param>
        /// <exception cref="ArgumentOutOfRangeException">When the data is accessed outside of the ROM.</exception>
        /// <returns>The array of bytes.</returns>
        public byte[] GetBytes(byte bank, UInt16 addr, UInt16 len)
        {
            if(addr >= 0x8000 || addr + len > 0x8000)
            {
                throw new ArgumentOutOfRangeException(String.Format("Attempted to read {0} bytes from 0x{1}.", len, MapEditor.decToHex(addr)), (Exception)null);
            }
            if(len == 0)
            {
                throw new ArgumentOutOfRangeException("Attempted to read 0 bytes.", (Exception)null);
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

        /// <summary>
        /// Retrieves a byte from the ROM.
        /// </summary>
        /// <param name="bank">The bank from which to fetch the byte from.</param>
        /// <param name="addr">The address at which to fetch the byte from.</param>
        /// <exception cref="ArgumentOutOfRangeException">When the byte is accessed outside of the ROM.</exception>
        /// <returns>The byte.</returns>
        public byte GetByte(byte bank, UInt16 addr)
        {
            return this.GetBytes(bank, addr, 1)[0];
        }

        /// <summary>
        /// Retrieves a short from the ROM.
        /// </summary>
        /// <param name="bank">The bank from which to fetch the short from.</param>
        /// <param name="addr">The address at which to fetch the short from.</param>
        /// <exception cref="ArgumentOutOfRangeException">When the data is accessed outside of the ROM.</exception>
        /// <returns>The low-endian short.</returns>
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
