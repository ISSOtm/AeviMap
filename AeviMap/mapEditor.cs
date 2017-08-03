
/*
 *   Copyright 2017 ISSOtm, Kai
 *
 *   Licensed under the Apache License, Version 2.0 (the "License");
 *   you may not use this file except in compliance with the License.
 *   You may obtain a copy of the License at
 *
 *       http://www.apache.org/licenses/LICENSE-2.0
 *
 *   Unless required by applicable law or agreed to in writing, software
 *   distributed under the License is distributed on an "AS IS" BASIS,
 *   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *   See the License for the specific language governing permissions and
 *   limitations under the License.
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace AeviMap
{
    public partial class AeviMapMainWindow : Form
    {
        // Reference vars, update with ROM
        public static byte numOfTilesets = 3;
        public static byte numOfMaps = 3;

        public static byte numOfBlocks = 64;

        private static byte mapPtrsBank = 2;
        private static UInt16 mapBanksPtr = 0x4000;
        private static UInt16 mapPtrsPtr = (UInt16)(mapBanksPtr + numOfMaps);
        private static byte tilesetPtrsBank = mapPtrsBank;
        private static UInt16 tilesetBanksPtr = 0x4300;
        private static UInt16 tilesetPtrsPtr = (UInt16)(tilesetBanksPtr + numOfTilesets);
        private static byte palettesBank = 1;

        private static UInt16 palette0Ptr = 0x5064;
        private static UInt16 palette1Ptr = (UInt16)(palette0Ptr + 3);

        private static byte sizeOfBlock = 64; // Size, in pixels, of a block
        private static UInt16 blockDataSize = 16 * 16 * 2; // Size, in bytes, of a block's BMP data

        private static string[] properties = {
            "numOfTilesets",
            "numOfMaps",
            "numOfBlocks",
            "mapPtrsBank",
            "mapBanksPtr",
            "mapPtrsPtr",
            "tilesetPtrsBank",
            "tilesetBanksPtr",
            "tilesetPtrsPtr",
            "palettesBank",
            "palette0Ptr",
            "palette1Ptr",
            "sizeOfBlock",
            "blockDataSize"
        };
        private bool isPropHex(string propName)
        {
            for(var i = 0; i < properties.Length; i++)
            {
                if(properties[i] == propName)
                {
                    return isHex[i];
                }
            }
            return false;
        }

        private static bool[] isHex = {
            false,
            false,
            false,
            false,
            true,
            true,
            false,
            true,
            true,
            false,
            true,
            true,
            false,
            false
        };


        public AeviMapMainWindow()
        {
            // Try reading the INI file to update the above info
            OpenINI();

            InitializeComponent();
            selectMapID.Maximum = numOfMaps - 1;
            blockPicker.Size = new Size(sizeOfBlock, sizeOfBlock * numOfBlocks);
        }


        // Global vars
        private bool isROMLoaded = false;
        private byte[] ROM { get; set; } // Will contain a dump of the ROM
        private byte[,] blocks = new byte[numOfBlocks, blockDataSize]; // Will contain the bitmaps of all the tileset's blocks
        private byte loadedMapROMBank = 0;
        private byte[] loadedMapBlocks { get; set; }
        private byte loadedMapWidth { get; set; }
        private byte loadedMapHeight { get; set; }

        private bool unsavedChanges = false;
        private byte selectedBlock = 0;
        private byte hoveredBlock = 0;
        private bool isHoveringBlockPicker = false;
        private uint hoveredBlockY = 0;
        private uint hoveredBlockX = 0;
        private bool isHoveringMap = false;

        private bool mapLoadingFailed = false;

        
        public char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        public Int16 hexDigitToInt(char digit)
        {
            Int16 val = 0;
            foreach(var chr in hexDigits)
            {
                if(chr == digit)
                {
                    return val;
                }
                val++;
            }
            return -1;
        }

        public bool hexToDec(string hex, out UInt16 output)
        {
            output = 0;
            Int16 digitValue;
            hex = hex.ToUpper();
            for(var i = 0; i < hex.Length; i++)
            {
                digitValue = hexDigitToInt(hex[i]);
                if(digitValue == -1)
                {
                    return false;
                }
                output = (UInt16)(output * 16 + digitValue);
            }
            return true;
        }

        public string decToHex(uint input, uint length)
        {
            string output = "";
            while(input != 0)
            {
                output = hexDigits[input % 16] + output;
                input /= 16;
            }
            return output.PadLeft((int)length, '0');
        }
        public string decToHex(UInt16 input)
        {
            return decToHex(input, 4);
        }

        private UInt16 getROMProperty(string propName)
        {
            switch(propName)
            {
                case "numOfTilesets":
                    return numOfTilesets;
                case "numOfMaps":
                    return numOfMaps;
                case "numOfBlocks":
                    return numOfBlocks;
                case "mapPtrsBank":
                    return mapPtrsBank;
                case "mapBanksPtr":
                    return mapBanksPtr;
                case "mapPtrsPtr":
                    return mapPtrsPtr;
                case "tilesetPtrsBank":
                    return tilesetPtrsBank;
                case "tilesetBanksPtr":
                    return tilesetBanksPtr;
                case "tilesetPtrsPtr":
                    return tilesetPtrsPtr;
                case "palettesBank":
                    return palettesBank;
                case "palette0Ptr":
                    return palette0Ptr;
                case "palette1Ptr":
                    return palette1Ptr;
                case "sizeOfBlock":
                    return sizeOfBlock;
                case "blockDataSize":
                    return blockDataSize;
                default:
                    throw new ArgumentException("Invalid ROM property", "propName");
            }
        }

        private void setROMProperty(string propName, UInt16 propValue)
        {
            switch (propName)
            {
                case "numOfTilesets":
                    numOfTilesets = (byte)propValue;
                    break;
                case "numOfMaps":
                    numOfMaps = (byte)propValue;
                    break;
                case "numOfBlocks":
                    numOfBlocks = (byte)propValue;
                    break;
                case "mapPtrsBank":
                    mapPtrsBank = (byte)propValue;
                    break;
                case "mapBanksPtr":
                    mapBanksPtr = propValue;
                    break;
                case "mapPtrsPtr":
                    mapPtrsPtr = propValue;
                    break;
                case "tilesetPtrsBank":
                    tilesetPtrsBank = (byte)propValue;
                    break;
                case "tilesetBanksPtr":
                    tilesetBanksPtr = propValue;
                    break;
                case "tilesetPtrsPtr":
                    tilesetPtrsPtr = propValue;
                    break;
                case "palettesBank":
                    palettesBank = (byte)propValue;
                    break;
                case "palette0Ptr":
                    palette0Ptr = propValue;
                    break;
                case "palette1Ptr":
                    palette1Ptr = propValue;
                    break;
                case "sizeOfBlock":
                    sizeOfBlock = (byte)propValue;
                    break;
                case "blockDataSize":
                    blockDataSize = propValue;
                    break;
                default:
                    // throw new ArgumentException("Invalid ROM property", "propName");
                    // Instead silently ignore property
                    break;
            }
        }
        
        /// <summary>
        /// Reads ROM-specific info from the .ini file if it exists, otherwise writes default info into a new one.
        /// </summary>
        private void OpenINI()
        {
            StreamReader ini;
            try
            {
                ini = File.OpenText("AeviMap.ini");
            }
            catch (FileNotFoundException)
            {
                StreamWriter defaultIni;
                try
                {
                    defaultIni = File.CreateText("AeviMap.ini");
                }
                catch (Exception)
                {
                    MessageBox.Show("The INI file could not be created !", ".ini file failure", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    // Leave default info in place
                    return;
                }

                // Write the default info to the INI file
                for (var i = 0; i < properties.Length; i++)
                {
                    string propName = properties[i];
                    if(isHex[i])
                    {
                        defaultIni.WriteLine("{0}: ${1}", propName, decToHex(getROMProperty(propName)));
                    }
                    else
                    {
                        defaultIni.WriteLine("{0}: {1}", propName, getROMProperty(propName));
                    }
                }
                defaultIni.Close();
                return;
            }

            // Read the info from the INI file
            string line;
            char[] seps = { ':', '=' };
            while ((line = ini.ReadLine()) != null)
            {
                var strs = line.Split(seps);
                if(strs.Length == 2) // Silently ignore malformed lines
                {
                    string propName = strs[0].Trim();
                    UInt16 propValue;
                    bool isPropertyCorrect;
                    string rawPropValue = strs[1].Trim();

                    if(!isPropHex(propName))
                    {
                        isPropertyCorrect = UInt16.TryParse(rawPropValue, out propValue);
                    }
                    else
                    {
                        string[] hexPrefixes = {
                            "0X",
                            "$",
                            "HEX:",
                            "HEX::"
                        };
                        byte i = 0;
                        bool isPrefixValid = false;
                        // Check if the string is correctly prefixed
                        while(i < hexPrefixes.Length && !isPrefixValid)
                        {
                            if(rawPropValue.IndexOf(hexPrefixes[i]) == 0)
                            {
                                rawPropValue = rawPropValue.Remove(0, hexPrefixes[i].Length);
                                isPrefixValid = true;
                            }
                            i++;
                        }
                        if(isPrefixValid)
                        {
                            isPropertyCorrect = hexToDec(rawPropValue, out propValue);
                        }
                        else
                        {
                            // If the value isn't hex-prefixed, check if it's suffixed instead
                            if(rawPropValue.Last() == 'H')
                            {
                                // Remove the suffix and parse
                                isPropertyCorrect = hexToDec(rawPropValue.Remove(rawPropValue.Length - 1), out propValue);
                            }
                            else
                            {
                                // Suppose it's a non-prefixed, non-suffixed hex value
                                isPropertyCorrect = hexToDec(rawPropValue, out propValue);
                            }
                        }
                    }

                    if(isPropertyCorrect) // Again, silently ignore malformed lines
                    {
                        setROMProperty(propName, propValue);
                    }
                }
            }
            ini.Close();
        }

        // "Engine" functions
        /// <summary>
        /// Load the ROM from a file, if it succeeds sets isROMLoaded to true. Required to perform any operation.
        /// </summary>
        private void OpenROM()
        {
            DialogResult shallOpen = openROMDialog.ShowDialog();
            if (shallOpen == DialogResult.OK)
            {
                ROM = File.ReadAllBytes(openROMDialog.FileName);
                isROMLoaded = true;
                loadedMapROMBank = 0;
                mapRenderer.Invalidate();
                blockPicker.Invalidate();
//                loadedfile.Text = savediag.FileName;
            }
        }

        /// <summary>
        /// Wrapper for OpenROM(), used as a Click() event.
        /// </summary>
        /// <param name="sender">Unused</param>
        /// <param name="e">Unused</param>
        private void OpenROM(object sender, EventArgs e)
        {
            OpenROM();
        }

        /// <summary>
        /// Save the current map blocks to a .blk file.
        /// </summary>
        private void SaveMap()
        {
            if(loadedMapROMBank == 0)
            {
                MessageBox.Show("Please load a map first.", "Can't save map !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult shallSave = saveMapDialog.ShowDialog();
            if(shallSave == DialogResult.OK)
            {
                FileStream mapFile;
                string mapFilePath = saveMapDialog.FileName;
                mapFile = File.OpenWrite(mapFilePath);
                mapFile.SetLength(0); // Empty the file
                mapFile.Write(loadedMapBlocks, 0, loadedMapHeight * loadedMapWidth);
                mapFile.Close();
                unsavedChanges = false;
            }
        }

        /// <summary>
        /// Wrapper for SaveMap(). Used as a Click() event.
        /// </summary>
        /// <param name="sender">Unused</param>
        /// <param name="e">Unused</param>
        private void SaveMap(object sender, EventArgs e)
        {
            SaveMap();
        }


        /// <summary>
        /// Read some bytes from the loaded ROM.
        /// If it attempts to read from outside the ROM, throws an exception.
        /// </summary>
        /// <param name="bank">ROM bank to read from.</param>
        /// <param name="addr">GB address to read from.</param>
        /// <param name="size">Number of bytes to read. Size of the returned array.</param>
        /// <returns>An array of `size` bytes from `bank`:`addr` to `bank`:`addr`+`size`-1</returns>
        private byte[] ReadBytesFromROM(byte bank, UInt16 addr, uint size)
        {
            if (addr >= 0x8000 || addr + size > 0x8000)
            {
                MessageBox.Show("Attempted to read " + size + " bytes from " + addr + " (decimal), which is impossible. The application is going to close.", "Bad read", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                Application.Exit();
            }

            if (size == 0)
            {
                MessageBox.Show("Attempted to read 0 bytes, which is impossible. The application is going to close.", "Bad size", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                Application.Exit();
            }

            UInt32 offset = 0;
            if (bank == 0)
            {
                if (addr >= 0x4000)
                {
                    // Actually ignore this, it can happen
                    // throw new Exception("Invalid address");
                }
                offset = addr;
            }
            else
            {
                offset = (addr < 0x4000) ? addr : (UInt32)((bank - 1) * 0x4000 + addr);
            }

            // Read `size` bytes from ROM and return them
            var output = new byte[size];

            for (uint i = 0; i < size; i++)
            {
                output[i] = ROM[offset];
                offset++;
            }

            return output;
        }
        
        /// <summary>
        /// Gets a palette from the ROM, and returns the "555" format of that palette's 4 colors.
        /// </summary>
        /// <param name="paletteAddr">Address to read from bank `palettesBank`</param>
        /// <returns>A 4*2 array, 4 colors, 2 bytes per color.</returns>
        private byte[,] GetPalette(UInt16 paletteAddr)
        {
            var rawPalette = ReadBytesFromROM(palettesBank, paletteAddr, 4 * 3);
            var palette = new byte[4,2];
            for(uint i = 0; i < 4; i++)
            {
                var red = rawPalette[i * 3];
                var green = rawPalette[i * 3 + 1];
                var blue = rawPalette[i * 3 + 2];
                // TODO : Compute GBC LCD transformation here
                palette[i,0] = (byte)(green << 5 | blue);
                palette[i,1] = (byte)(red << 2 | green >> 3);
            }
            return palette;
        }

        /// <summary>
        /// Load a tileset from the loaded ROM.
        /// </summary>
        /// <param name="tilesetID">The ID of the tileset to load.</param>
        private void LoadTileset(byte tilesetID)
        {
            var tilesetROMBank = ReadBytesFromROM(tilesetPtrsBank, (UInt16)(tilesetBanksPtr + tilesetID), 1)[0];
            var tilesetPointerRaw = ReadBytesFromROM(tilesetPtrsBank, (UInt16)(tilesetPtrsPtr + tilesetID * 2), 2);
            var tilesetPointer = (UInt16)(tilesetPointerRaw[1] * 256 + tilesetPointerRaw[0]);

            // Load tiles
            var numOfTiles = ReadBytesFromROM(tilesetROMBank, tilesetPointer, 1)[0]; // Get number of tiles
            tilesetPointer++; // Skip over the number we just read
            var tiles = new byte[257, 16];
            for(uint i = 0; i < numOfTiles; i++)
            {
                var tile = ReadBytesFromROM(tilesetROMBank, tilesetPointer, 16); // Read one tile from ROM
                tilesetPointer += 16; // Skip over the tile
                for(uint j = 0; j < 16; j++)
                {
                    tiles[i, j] = tile[j]; // Copy the tile's data into the array
                }
            }
            for(uint i = numOfTiles; i < 256; i++) // The game doesn't clear uninitialized tiles, so give them a checkerboard pattern to say "WARNING"
            {
                for(uint j = 0; j < 4; j++)
                {
                    // X = color 3, . = color 0, to create contrast
                    tiles[i, j * 4]     = 0xAA;
                    tiles[i, j * 4 + 1] = 0xAA; // X.X.X.X.
                    tiles[i, j * 4 + 2] = 0x55; // .X.X.X.X
                    tiles[i, j * 4 + 3] = 0x55;
                }
            }
            // Set special tiles (those not loaded by the tileset)
            for(uint j = 0; j < 16; j++)
            {
                tiles[256, j] = 0;
            }

            // Load block metadata
            var blockMetadata = new byte[numOfBlocks, 4, 2];
            for(uint i = 0; i < numOfBlocks; i++)
            {
                for(uint j = 0; j < 4; j++)
                {
                    var tile = ReadBytesFromROM(tilesetROMBank, tilesetPointer, 2);
                    tilesetPointer += 2;
                    blockMetadata[i, j, 0] = tile[0];
                    blockMetadata[i, j, 1] = tile[1];
                }
            }

            // Load palettes
            var palettePointers = ReadBytesFromROM(tilesetROMBank, (UInt16)(tilesetPointer - 2 * 2), 8 * 2); // Get pointers, plus two slots that will be filled manually
            palettePointers[0] = (byte)palette0Ptr; // Set default palette 0 pointer
            palettePointers[1] = (byte)(palette0Ptr >> 8);
            palettePointers[2] = (byte)palette1Ptr; // Set default palette 1 pointer
            palettePointers[3] = (byte)(palette1Ptr >> 8);
            var palettes = new byte[8,4,2]; // Make array
            for(uint i = 0; i < 8; i++) // Retrieve palettes pointed to
            {
                var palette = GetPalette((UInt16)(palettePointers[i * 2 + 1] * 256 + palettePointers[i * 2]));
                for(uint j = 0; j < 4; j++)
                {
                    // Copy palette into array
                    palettes[i, j, 0] = palette[j, 0];
                    palettes[i, j, 1] = palette[j, 1];
                }
            }

            // Now, we have an array of tiles (`tiles`), and array of computed color palettes (`palettes`), and an array of block metadatas (`blockMetadata`)
            // We're now going to use this to generate an array of 16x16 bitmaps that will then be rendered on-screen
            // This array is `blocks`.
            for (uint i = 0; i < numOfBlocks; i++)
            {
                var blockData = new byte[blockDataSize];

                var blockMetadatas = new byte[] { blockMetadata[i, 0, 1], blockMetadata[i, 2, 1], blockMetadata[i, 1, 1], blockMetadata[i, 3, 1]};
                var blockTiles = new UInt16[] { blockMetadata[i, 0, 0], blockMetadata[i, 2, 0], blockMetadata[i, 1, 0], blockMetadata[i, 3, 0]};
                for(uint j = 0; j < 4; j++)
                {
                    if(blockTiles[j] < 0x80)
                    {
                        // Filter special tiles here, currently they will be displayed blank
                        blockTiles[j] = 0x100;
                    }
                    else
                    {
                        // If we aren't in VRAM bank 1, go in the lower half of the table
                        if((blockMetadatas[j] & 0x08) != 0x08)
                        {
                            blockTiles[j] -= 0x80;
                        }
                    }
                }

                for(uint j = 0; j < 16; j++)
                {
                    var tileLayer0 = 0;
                    var tileLayer1 = 0;
                    var curTileID = 0;
                    var curIndex = j >> 2 & 2; // Tile IDs depend on which half we are on
                    var paletteID = 0;
                    for(uint k = 0; k < 16; k++)
                    {
                        // If we are on a tile edge, load the layers for processing
                        if(k % 8 == 0)
                        {
                            if(k != 0)
                            {
                                curIndex++; // Get on next index for next tile (second change will be cancelled by encompassing loop)
                            }
                            curTileID = blockTiles[curIndex];

                            var tileRow = j & 0x07;
                            // Apply vertical flip
                            if((blockMetadatas[curIndex] & 0x40) == 0x40)
                            {
                                tileRow = 7 - tileRow;
                            }
                            tileLayer0 = tiles[curTileID, tileRow * 2];
                            tileLayer1 = tiles[curTileID, tileRow * 2 + 1];
                            // Apply horizontal flip
                            if((blockMetadatas[curIndex] & 0x20) == 0x20)
                            {
                                var temp0 = tileLayer0;
                                var temp1 = tileLayer1;
                                tileLayer0 = 0;
                                tileLayer1 = 0;
                                for(var shiftCount = 0; shiftCount < 8; shiftCount++)
                                {
                                    tileLayer0 <<= 1;
                                    tileLayer1 <<= 1;

                                    if((temp0 & 1) == 1)
                                    {
                                        tileLayer0++;
                                    }
                                    if((temp1 & 1) == 1)
                                    {
                                        tileLayer1++;
                                    }

                                    temp0 >>= 1;
                                    temp1 >>= 1;
                                }
                            }

                            paletteID = blockMetadatas[curIndex] & 0x7;
                        }

                        var pixelColorIndex = 0;
                        if((tileLayer0 & 0x80) == 0x80)
                        {
                            pixelColorIndex++;
                        }
                        if((tileLayer1 & 0x80) == 0x80)
                        {
                            pixelColorIndex += 2;
                        }
                        tileLayer0 <<= 1;
                        tileLayer1 <<= 1;
                        
                        blockData[(j * 16 + k) * 2] = palettes[paletteID, pixelColorIndex, 0];
                        blockData[(j * 16 + k) * 2 + 1] = palettes[paletteID, pixelColorIndex, 1];
                    }
                }
                for (var j = 0; j < blockDataSize; j++) {
                    blocks[i, j] = blockData[j];
                }
            }
        }

        /// <summary>
        /// Load a map's (relevant here) data, given its map ID.
        /// </summary>
        /// <param name="mapID">The ID of the map to load.</param>
        private void LoadMap(byte mapID)
        {
            if(!isROMLoaded)
            {
                MessageBox.Show("Load a ROM first.", "Can't load map !", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                mapLoadingFailed = true;
                return;
            }

            if(mapID < 0 || mapID >= numOfMaps)
            {
                MessageBox.Show("Valid map IDs are integers between 0 and " + (numOfMaps - 1) + " !", "Invalid map ID !", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                mapLoadingFailed = true;
                return;
            }

            if(unsavedChanges)
            {
                DialogResult areFucksGiven = MessageBox.Show("There are unsaved changes !\nDo you want to save them before switching ?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if(areFucksGiven == DialogResult.Yes)
                {
                    SaveMap();
                    if(unsavedChanges)
                    {
                        DialogResult thisIsConfusing = MessageBox.Show("You didn't save your changes !", "Still unsaved changes", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                        if(thisIsConfusing == DialogResult.Cancel)
                        {
                            mapLoadingFailed = true;
                            return;
                        }
                    }
                }
                else if(areFucksGiven == DialogResult.Cancel)
                {
                    mapLoadingFailed = true;
                    return;
                }
                // else : 0 fucks given, don't do anything.
                unsavedChanges = false;
            }

            // Get the map's location
            var mapROMBank = ReadBytesFromROM(mapPtrsBank, (UInt16)(mapBanksPtr + mapID), 1)[0];
            var mapPointerRaw = ReadBytesFromROM(mapPtrsBank, (UInt16)(mapPtrsPtr + mapID * 2), 2);
            var mapPointer = (UInt16)(mapPointerRaw[1] * 256 + mapPointerRaw[0]);

            // Set this data if it's ever needed again
            loadedMapROMBank = mapROMBank;

            // Read the map's tileset ID, script (2 bytes), width, and height
            var essentialData = ReadBytesFromROM(mapROMBank, (UInt16)(mapPointer + 2), 5);
            var tilesetID = essentialData[0];
            loadedMapWidth = essentialData[3];
            loadedMapHeight = essentialData[4];

            // Load the map's tileset ID
            LoadTileset(tilesetID);

            // Now we're going to skip over data irrelevant to us
            // This requires multiple steps because each time we have to read a number of elements to skip over

            // Skip over map properties, music ID, tileset ID, script (2 bytes), width, height, and loading script (2 bytes)
            mapPointer += 9;
            // Skip over interactions & num of such
            mapPointer += (UInt16)(1 + 9 * ReadBytesFromROM(mapROMBank, mapPointer, 1)[0]);
            // Skip over NPCs & num of
            mapPointer += (UInt16)(1 + 12 * ReadBytesFromROM(mapROMBank, mapPointer, 1)[0]);
            // Skip over NPC scripts & num of
            mapPointer += (UInt16)(1 + 2 * ReadBytesFromROM(mapROMBank, mapPointer, 1)[0]);
            // Skip over NPC tiles & num of
            mapPointer += (UInt16)(1 + 2 * ReadBytesFromROM(mapROMBank, mapPointer, 1)[0]);
            // Skip over warp-tos & num of
            mapPointer += (UInt16)(1 + 16 * ReadBytesFromROM(mapROMBank, mapPointer, 1)[0]);

            // The reading pointer now is the block pointer
            loadedMapBlocks = ReadBytesFromROM(loadedMapROMBank, mapPointer, (uint)(loadedMapWidth * loadedMapHeight));
            
            mapRenderer.Size = new Size(sizeOfBlock * loadedMapWidth, sizeOfBlock * loadedMapHeight);

            // Update both of these since the tileset has been reloaded
            mapRenderer.Invalidate();
            blockPicker.Invalidate();
            mapLoadingFailed = false;
        }

        /// <summary>
        /// Function called when you click "Load Map"
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void LoadMap(object sender, EventArgs e)
        {
            // Get the map's ID from the box
            byte mapID = (byte)selectMapID.Value;
            
            // Ensure it's an integer
            if((decimal)mapID != selectMapID.Value)
            {
                MessageBox.Show("Valid map IDs are integers !", "Invalid map ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mapLoadingFailed = true;
                return;
            }
            
            // Let LoadMap check the ID is valid
            LoadMap(mapID);
        }


        private void MouseEnterBlockPicker(object sender, EventArgs e)
        {
            isHoveringBlockPicker = true;
        }
        private void MouseLeaveBlockPicker(object sender, EventArgs e)
        {
            isHoveringBlockPicker = false;
            blockPicker.Invalidate();
        }

        // Event to move the cursor on the block picker
        private void MouseMoveBlockPicker(object sender, MouseEventArgs e)
        {
            hoveredBlock = (byte)(e.Y / sizeOfBlock);

            // Have the block picker redraw the updated cursor
            blockPicker.Invalidate();
        }

        private void ModifyPickedBlock(byte newBlock)
        {
            selectedBlock = newBlock;
            blockPicker.Invalidate();
        }
        
        private void ClickBlockPicker(object sender, MouseEventArgs e)
        {
            ModifyPickedBlock(hoveredBlock);
        }

        private void MouseEnterMapRenderer(object sender, EventArgs e)
        {
            isHoveringMap = true;
        }
        private void MouseLeaveMapRenderer(object sender, EventArgs e)
        {
            isHoveringMap = false;
            mapRenderer.Invalidate();
        }
        
        private void MouseMoveMapRenderer(object sender, MouseEventArgs e)
        {
            hoveredBlockY = (uint)(e.Y / sizeOfBlock);
            hoveredBlockX = (uint)(e.X / sizeOfBlock);
            
            mapRenderer.Invalidate();
        }

        private void ModifyHoveredBlock(uint blockY, uint blockX)
        {
            unsavedChanges = true;
            loadedMapBlocks[blockY * loadedMapWidth + blockX] = selectedBlock;
            mapRenderer.Invalidate();
        }

        // Event to change the picked block
        private void ClickMapRenderer(object sender, MouseEventArgs e)
        {
            switch(e.Button)
            {
                case MouseButtons.Left:
                    if(loadedMapROMBank != 0)
                    {
                        ModifyHoveredBlock(hoveredBlockY, hoveredBlockX);
                    }
                break;

                case MouseButtons.Right:
                    selectedBlock = loadedMapBlocks[hoveredBlockY * loadedMapWidth + hoveredBlockX];
                break;
            }
        }


        private void DrawBlockOutline(Graphics gfx, int x, int y)
        {
            Pen pen = new Pen(Color.Red, 2);
            var points = new Point[] { new Point(x + 1, y + 1), new Point(x + sizeOfBlock - 1, y + 1), new Point(x + sizeOfBlock - 1, y + sizeOfBlock - 1), new Point(x + 1, y + sizeOfBlock - 1) };
            gfx.DrawPolygon(pen, points);
        }

        private Bitmap GetBlockBMP(byte ID)
        {
            byte[] blockData = new byte[blockDataSize];
            for(var i = 0; i < blockDataSize; i++)
            {
                blockData[i] = blocks[ID, i];
            }
            // That Marshal.UnsafeBlahBlahBlah comes from https://stackoverflow.com/questions/21555394/how-to-create-bitmap-from-byte-array
            return new Bitmap(16, 16, 16 * 2, System.Drawing.Imaging.PixelFormat.Format16bppRgb555, Marshal.UnsafeAddrOfPinnedArrayElement(blockData, 0));
        }

        // Paint event to render the map
        private void RenderMap(object sender, PaintEventArgs e)
        {
            if(loadedMapROMBank != 0)
            {
                Graphics gfx = e.Graphics;
                for(byte y = 0; y < loadedMapHeight; y++)
                {
                    var oy = y * sizeOfBlock;
                    for(byte x = 0; x < loadedMapWidth; x++)
                    {
                        var ox = x * sizeOfBlock;
                        if(gfx.IsVisible(ox, oy, sizeOfBlock, sizeOfBlock))
                        {
                            gfx.DrawImage(GetBlockBMP(loadedMapBlocks[y * loadedMapWidth + x]), ox, oy, sizeOfBlock, sizeOfBlock);
                        }
                    }
                }

                if(isHoveringMap && gfx.IsVisible(hoveredBlockX * sizeOfBlock, hoveredBlockY * sizeOfBlock, sizeOfBlock, sizeOfBlock))
                {
                    DrawBlockOutline(gfx, (int)(hoveredBlockX * sizeOfBlock), (int)(hoveredBlockY * sizeOfBlock));
                }
            }
        }

        private void RenderBlockPicker(object sender, PaintEventArgs e)
        {
            if(loadedMapROMBank != 0)
            {
                Graphics gfx = e.Graphics;
                for(byte y = 0; y < numOfBlocks; y++)
                {
                    var oy = y * sizeOfBlock;
                    if(gfx.IsVisible(0, oy, sizeOfBlock, sizeOfBlock))
                    {
                        gfx.DrawImage(GetBlockBMP(y), 0, oy, sizeOfBlock, sizeOfBlock);
                    }
                }


                if(gfx.IsVisible(0, selectedBlock * sizeOfBlock, sizeOfBlock, sizeOfBlock))
                {
                    DrawBlockOutline(gfx, 0, selectedBlock * sizeOfBlock);
                }

                if(isHoveringBlockPicker && gfx.IsVisible(0, hoveredBlock * sizeOfBlock, sizeOfBlock, sizeOfBlock))
                {
                    DrawBlockOutline(gfx, 0, hoveredBlock * sizeOfBlock);
                }
            }
        }


        private void closeApp(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ConfirmClose(object sender, FormClosingEventArgs e)
        {
            if(unsavedChanges)
            {
                DialogResult whoCares = MessageBox.Show("There are unsaved changes !\nDo you want to save them ?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if(whoCares == DialogResult.Yes)
                {
                    SaveMap();
                }
                else if(whoCares == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void LoadMapFromBlk(object sender, EventArgs e)
        {
            if(!isROMLoaded)
            {
                MessageBox.Show("Load a ROM first.", "Load map...", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            DialogResult isMapLoaded = openMapDialog.ShowDialog();
            if(isMapLoaded != DialogResult.OK)
            {
                MessageBox.Show("Loading aborted.", "Load map...", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                return;
            }

            FileStream mapFile = File.OpenRead(openMapDialog.FileName);
            
            LoadMap(sender, e);
            if(mapLoadingFailed)
            {
                MessageBox.Show("Loading from .blk aborted.", "Load map...", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
            }
            else
            {
                if(mapFile.Length != loadedMapHeight * loadedMapWidth)
                {
                    DialogResult loadAnyways = MessageBox.Show("The file's size doesn't match the map's size !\nLoad anyways ?", "Load map...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if(loadAnyways != DialogResult.Yes)
                    {
                        mapFile.Close();
                        return;
                    }
                }
                mapFile.Read(loadedMapBlocks, 0, loadedMapHeight * loadedMapWidth);
                unsavedChanges = true;
            }
            mapFile.Close();
        }

        private void aboutAeviMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            about abt = new about();
            abt.ShowDialog();
        }
    }
}
