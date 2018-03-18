using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AeviMap
{
    public partial class mapCreator : Form
    {
        private GB_ROM ROM;
        private INI_File ini;

        private bool hasCreatedMap = false;
        private byte TilesetID {
            get
            {
                return (byte)tilesetSel.SelectedIndex;
            }
        }
        private byte MapHeight
        {
            get
            {
                return (byte)mapXBox.Value;
            }
        }
        private byte MapWidth
        {
            get
            {
                return (byte)mapYBox.Value;
            }
        }
        private byte FillerID {
            get
            {
                return (byte)FillerBlockID.Value;
            }
        }
        private Map CreatedMap = null;

        public string CreatedMapName
        {
            get
            {
                return mapNameBox.Text.Trim();
            }
        }


        public mapCreator(GB_ROM ROM, INI_File ini)
        {
            this.ROM = ROM;
            this.ini = ini;


            InitializeComponent();

            this.tilesetSel.Items.Clear();
            this.tilesetSel.Items.AddRange(this.ini.GetTilesetNames().ToArray());

            int size = this.ini.GetProperty("sizeofblock") * 4;
            Rectangle Bounds = new Rectangle(FillerBlockPreview.Bounds.X, FillerBlockPreview.Bounds.Y, size, size);
            FillerBlockPreview.Bounds = Bounds; // For some reason FillerBlockPreview.SetBounds() doesn't work. Go figure.

            this.updateBlockPreview(null, null);
        }

        private void updateTilesetPreview(object sender, EventArgs e)
        {
            if (this.TilesetID >= this.ini.GetProperty("nboftilesets"))
            {
                tilesetPrev.Image = tilesetDB.unknown_tileset;
            }
            else
            {
                tilesetPrev.Image = this.ROM.GetTileset(this.TilesetID, this.ini).GetPreviewImage();
                this.updateBlockPreview(null, null);
            }
        }

        private void CloseDialog(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CreateMap(object sender, EventArgs e)
        {
            string ErrorMessage = null;
            if (this.MapHeight * this.MapWidth > 0x1000)
            {
                ErrorMessage = "Total map size must not exceed 0x1000 (4096) blocks!";
            }
            else if(this.TilesetID >= this.ini.GetProperty("nboftilesets"))
            {
                ErrorMessage = "Please select a valid tileset.";
            }
            else if(this.CreatedMapName == "")
            {
                ErrorMessage = "Please enter a non-empty name.";
            }
            else
            {
                this.hasCreatedMap = true;
                this.CloseDialog(sender, e);
                return;
            }

            MessageBox.Show(ErrorMessage, "Invalid map!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }


        public bool HasCreatedMap()
        {
            return this.hasCreatedMap;
        }

        public Map GetCreatedMap()
        {
            if(this.CreatedMap == null)
            {
                this.CreatedMap = new Map(this.ini, this.ROM, this.TilesetID, this.MapHeight, this.MapWidth);
            }
            return this.CreatedMap;
        }

        private void updateBlockPreview(object sender, EventArgs e)
        {
            if(this.TilesetID < this.ini.GetProperty("nboftilesets"))
            {
                this.FillerBlockPreview.Image = new Bitmap(this.ROM.GetTileset(this.TilesetID, this.ini).GetBlockBMP(this.FillerID), FillerBlockPreview.Size);
            }
        }
    }
}
