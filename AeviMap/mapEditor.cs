
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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace AeviMap
{
    public partial class MapEditor : Form
    {

        private INI_File INIFile = new INI_File();
        private GB_ROM ROM;

        private UInt16 sizeOfBlock;
        private UInt16 nbOfBlocks;


        // State vars
        private bool UnsavedChanges = false;
        private bool IsMapLoaded = false;
        private int LoadedMapID;

        private byte SelectedBlock = 0;
        private byte HoveredBlock = 0;
        private bool IsHoveringBlockPicker = false;
        private uint HoveredBlockY = 0;
        private uint HoveredBlockX = 0;
        private bool IsHoveringMap = false;


        public MapEditor()
        {
            // Try reading the INI file
            this.INIFile.ParseFile("AeviMap.ini");
            // Write the full INI file (TODO : this isn't always necessary)
            this.INIFile.WriteFile("AeviMap.ini");

            // Init constants
            this.sizeOfBlock = this.INIFile.GetProperty("sizeofblock");
            this.nbOfBlocks = this.INIFile.GetProperty("nbofblocks");


            // Init gfx components
            InitializeComponent();
            selectMapID.Maximum = this.INIFile.GetProperty("nbofmaps") - 1;
            blockPicker.Size = new Size(sizeOfBlock, sizeOfBlock * this.INIFile.GetProperty("nbofblocks"));

            // Add map names to the picker
            selectMapName.Items.Clear();
            for(byte mapID = 0; mapID < this.INIFile.GetProperty("nbofmaps"); mapID++)
            {
                selectMapName.Items.Add(this.INIFile.GetMapName(mapID));
            }
        }

        
        // Hex / dec conversion functions
        // Didn't find any in the framework, but if there are any, please replace these with the lib funcs
        private static char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        /// <summary>
        /// Converts a hex digit into its decimal equivalent.
        /// </summary>
        /// <param name="digit">The hex digit to be converted, as a char.</param>
        /// <returns>Its decimal equivalent if the digit is valid, or -1 if not.</returns>
        public static Int16 hexDigitToInt(char digit)
        {
            Int16 val = 0;
            foreach(var chr in MapEditor.hexDigits)
            {
                if(chr == digit)
                {
                    return val;
                }
                val++;
            }
            return -1;
        }

        /// <summary>
        /// Converts a hex number into its decimal representation.
        /// Limited to 16-bit values.
        /// </summary>
        /// <param name="hex">The hex number to be converted, in string format. Prefix-less, case-insensitive.</param>
        /// <param name="output">The number to be written to.</param>
        /// <returns>True if the number was a valid hexadecimal number. If false, `output` is left undefined.</returns>
        public static bool hexToDec(string hex, out UInt16 output)
        {
            output = 0;
            Int16 digitValue;
            hex = hex.ToUpper();
            for(var i = 0; i < hex.Length; i++)
            {
                digitValue = MapEditor.hexDigitToInt(hex[i]);
                if(digitValue == -1)
                {
                    return false;
                }
                output = (UInt16)(output * 16 + digitValue);
            }
            return true;
        }

        /// <summary>
        /// Convert a decimal number into its hexadecimal equivalent.
        /// </summary>
        /// <param name="input">The number to be converted.</param>
        /// <param name="length">The minimal number of hex digits.</param>
        /// <returns>The hexadecimal number. All letters lowercase, prefix-less.</returns>
        public static string decToHex(uint input, uint length)
        {
            string output = "";
            while(input != 0)
            {
                output = hexDigits[input % 16] + output;
                input /= 16;
            }
            return output.PadLeft((int)length, '0');

        }
        /// <summary>
        /// Convert a decimal number into its hexadecimal equivalent, with at least 4 hexadecimal digits.
        /// </summary>
        /// <param name="input">The number to be converted.</param>
        /// <returns>The hexadecimal number. All letters lowercase, prefix-less.</returns>
        public static string decToHex(UInt16 input)
        {
            return decToHex(input, 4);
        }



        private Map LoadedMap()
        {
            return this.ROM.GetMap((byte)this.LoadedMapID, this.INIFile);
        }



        // ===== "Engine" functions =====

        /// <summary>
        /// Load the ROM from a file, if it succeeds sets isROMLoaded to true. Required to perform any operation.
        /// </summary>
        private void OpenROM()
        {
            if(UnsavedChanges)
            {
                DialogResult shallSaveChanges = MessageBox.Show("There are unsaved changes ! Do you wish to change them before loading a ROM ?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if(shallSaveChanges == DialogResult.Cancel)
                {
                    return;
                }
                else if(shallSaveChanges == DialogResult.Yes)
                {
                    SaveMap();
                    if(UnsavedChanges)
                    {
                        MessageBox.Show("Please save your changes to load the ROM.", "Still unsaved changes", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        return;
                    }
                }
            }

            DialogResult shallOpen = openROMDialog.ShowDialog();
            if (shallOpen == DialogResult.OK)
            {
                ROM = new GB_ROM(openROMDialog.FileName, this.INIFile);
                mapRenderer.Invalidate();
                blockPicker.Invalidate();
                UnsavedChanges = false;
            }
        }

        /// <summary>
        /// Save the current map blocks to a .blk file.
        /// </summary>
        private void SaveMap()
        {
            if(!IsMapLoaded)
            {
                MessageBox.Show("Please load a map first.", "Can't save map !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult shallSave = saveMapDialog.ShowDialog();
            if(shallSave == DialogResult.OK)
            {
                FileStream mapFile;
                string mapFilePath = saveMapDialog.FileName;
                byte[] blocks = this.LoadedMap().GetBlocks();

                mapFile = File.OpenWrite(mapFilePath);
                mapFile.SetLength(0); // Empty the file
                mapFile.Write(blocks, 0, blocks.Length);
                mapFile.Close();

                UnsavedChanges = false;
            }
        }


        /// <summary>
        /// Load a map's (relevant here) data, given its map ID.
        /// </summary>
        /// <param name="mapID">The ID of the map to load.</param>
        private void LoadMap(byte mapID)
        {
            if(this.ROM == null)
            {
                MessageBox.Show("Load a ROM first.", "Can't load map !", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            if(mapID < 0 || mapID >= this.INIFile.GetProperty("nbofmaps"))
            {
                MessageBox.Show(String.Format("Valid map IDs are integers between 0 and {0} !", this.INIFile.GetProperty("nbofmaps")), "Invalid map ID !", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            if(UnsavedChanges)
            {
                DialogResult areFucksGiven = MessageBox.Show("There are unsaved changes!\nDo you want to save them before switching?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if(areFucksGiven == DialogResult.Yes)
                {
                    SaveMap();
                    if(UnsavedChanges)
                    {
                        MessageBox.Show("Please save your changes.", "Still unsaved changes", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        return;
                    }
                }
                else if(areFucksGiven == DialogResult.Cancel)
                {
                    return;
                }
                // else : 0 fucks given, don't do anything.
                UnsavedChanges = false;
            }



            IsMapLoaded = true;
            this.LoadedMapID = mapID;

            Size LoadedMapSize = this.LoadedMap().GetSize();
            mapRenderer.Size = new Size(LoadedMapSize.Width * sizeOfBlock, LoadedMapSize.Height *  sizeOfBlock);

            // Update both of these since the tileset has been reloaded
            mapRenderer.Invalidate();
            blockPicker.Invalidate();
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
        /// Wrapper for OpenROM(), used as a Click() event.
        /// </summary>
        /// <param name="sender">Unused</param>
        /// <param name="e">Unused</param>
        private void OpenROM(object sender, EventArgs e)
        {
            OpenROM();
        }

        /// <summary>
        /// Function called when you click "Load Map"
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void LoadMap(object sender, EventArgs e)
        {
            // Get the map's ID from the box
            byte mapID = (byte)selectMapName.SelectedIndex;
            if (selectMapName.Text == "Select Map")
            {
                MessageBox.Show("Please select a map.");
                return;
            }
            selectMapID.Value = selectMapName.SelectedIndex;
            
            // Ensure it's an integer
            if((decimal)mapID != selectMapName.SelectedIndex)
            {
                MessageBox.Show("Valid map IDs are integers !", "Invalid map ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            // Let LoadMap check the ID is valid
            LoadMap(mapID);
        }

        private void LoadMapID(object sender, EventArgs e)
        {
            byte mapID = (byte)selectMapID.Value;

            if ((decimal)mapID != selectMapID.Value)
            {
                MessageBox.Show("Valid map IDs are integers !", "Invalid map ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            selectMapName.SelectedIndex = Convert.ToInt16(selectMapID.Value);
            LoadMap(mapID);
        }


        private void MouseEnterBlockPicker(object sender, EventArgs e)
        {
            IsHoveringBlockPicker = true;
        }
        private void MouseLeaveBlockPicker(object sender, EventArgs e)
        {
            IsHoveringBlockPicker = false;
            blockPicker.Invalidate();
        }

        // Event to move the cursor on the block picker
        private void MouseMoveBlockPicker(object sender, MouseEventArgs e)
        {
            HoveredBlock = (byte)(e.Y / sizeOfBlock);

            // Have the block picker redraw the updated cursor
            blockPicker.Invalidate();
        }

        private void ModifyPickedBlock(byte newBlock)
        {
            SelectedBlock = newBlock;
            blockPicker.Invalidate();
        }
        
        private void ClickBlockPicker(object sender, MouseEventArgs e)
        {
            ModifyPickedBlock(HoveredBlock);
        }


        private void MouseEnterMapRenderer(object sender, EventArgs e)
        {
            IsHoveringMap = true;
        }
        private void MouseLeaveMapRenderer(object sender, EventArgs e)
        {
            IsHoveringMap = false;
            mapRenderer.Invalidate();
        }
        
        private void MouseMoveMapRenderer(object sender, MouseEventArgs e)
        {
            HoveredBlockY = (uint)(e.Y / sizeOfBlock);
            HoveredBlockX = (uint)(e.X / sizeOfBlock);
            
            mapRenderer.Invalidate();
        }


        private void ModifyHoveredBlock(uint blockY, uint blockX)
        {
            UnsavedChanges = true;
            this.LoadedMap().SetBlockIDAt(blockX, blockY, SelectedBlock);
            mapRenderer.Invalidate();
        }

        // Event to change the picked block
        private void ClickMapRenderer(object sender, MouseEventArgs e)
        {
            switch(e.Button)
            {
                case MouseButtons.Left:
                    if(IsMapLoaded)
                    {
                        ModifyHoveredBlock(HoveredBlockY, HoveredBlockX);
                    }
                break;

                case MouseButtons.Right:
                    ModifyPickedBlock(this.LoadedMap().GetBlockIDAt(HoveredBlockX, HoveredBlockY));
                break;
            }
        }


        private void DrawBlockOutline(Graphics gfx, int x, int y)
        {
            Pen pen = new Pen(Color.Red, 2);
            var points = new Point[] { new Point(x + 1, y + 1), new Point(x + sizeOfBlock - 1, y + 1), new Point(x + sizeOfBlock - 1, y + sizeOfBlock - 1), new Point(x + 1, y + sizeOfBlock - 1) };
            gfx.DrawPolygon(pen, points);
        }

        // Paint event to render the map
        private void RenderMap(object sender, PaintEventArgs e)
        {
            if(IsMapLoaded)
            {
                Graphics gfx = e.Graphics;
                Size LoadedMapSize = this.LoadedMap().GetSize();

                for(byte y = 0; y < LoadedMapSize.Height; y++)
                {
                    var oy = y * sizeOfBlock;
                    for(byte x = 0; x < LoadedMapSize.Width; x++)
                    {
                        var ox = x * sizeOfBlock;
                        if(gfx.IsVisible(ox, oy, sizeOfBlock, sizeOfBlock))
                        {
                            gfx.DrawImage(this.LoadedMap().GetBlockBMPAt(x, y), ox, oy, sizeOfBlock, sizeOfBlock);
                        }
                    }
                }

                if(IsHoveringMap && gfx.IsVisible(HoveredBlockX * sizeOfBlock, HoveredBlockY * sizeOfBlock, sizeOfBlock, sizeOfBlock))
                {
                    DrawBlockOutline(gfx, (int)(HoveredBlockX * sizeOfBlock), (int)(HoveredBlockY * sizeOfBlock));
                }
            }
        }

        private void RenderBlockPicker(object sender, PaintEventArgs e)
        {
            if(IsMapLoaded)
            {
                Graphics gfx = e.Graphics;
                for(byte y = 0; y < nbOfBlocks; y++)
                {
                    var oy = y * sizeOfBlock;
                    if(gfx.IsVisible(0, oy, sizeOfBlock, sizeOfBlock))
                    {
                        gfx.DrawImage(this.LoadedMap().GetBlockBMP(y), 0, oy, sizeOfBlock, sizeOfBlock);
                    }
                }


                if(gfx.IsVisible(0, SelectedBlock * sizeOfBlock, sizeOfBlock, sizeOfBlock))
                {
                    DrawBlockOutline(gfx, 0, SelectedBlock * sizeOfBlock);
                }

                if(IsHoveringBlockPicker && gfx.IsVisible(0, HoveredBlock * sizeOfBlock, sizeOfBlock, sizeOfBlock))
                {
                    DrawBlockOutline(gfx, 0, HoveredBlock * sizeOfBlock);
                }
            }
        }


        private void closeApp(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ConfirmClose(object sender, FormClosingEventArgs e)
        {
            if(UnsavedChanges)
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

        /* This function will be replaced with the "New map" function
        private void LoadMapFromBlk(object sender, EventArgs e)
        {
            if(this.ROM == null)
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
        */

        private void aboutAeviMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            about abt = new about();
            abt.ShowDialog();
        }

    }
}
