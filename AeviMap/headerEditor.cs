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
    public partial class headerEditor : Form
    {
        private INI_File ini;
        private GB_ROM ROM;
        public Map map { get; private set; }
        internal MapHeader header { get; private set; }

        private byte TilesetID
        {
            get
            {
                return (byte)tilesetSel.SelectedIndex;
            }
        }
        private byte MapHeight
        {
            get
            {
                return (byte)mapYBox.Value;
            }
        }
        private byte MapWidth
        {
            get
            {
                return (byte)mapXBox.Value;
            }
        }
        private byte FillerID
        {
            get
            {
                return (byte)FillerBlockID.Value;
            }
        }
        // A bit of roundabout : "None" is internally encoded as 0xFF, but displayed as the 0th element
        // Perform the translation here
        private MusicID SongID
        {
            get
            {
                var userIndex = this.songSel.SelectedIndex;
                return (userIndex == 0) ? MusicID.MUSIC_NONE : (MusicID)(userIndex - 1);
            }
            set
            {
                this.songSel.SelectedIndex = (value == MusicID.MUSIC_NONE) ? 0 : (int)value + 1;
            }
        }


        public headerEditor(GB_ROM ROM, INI_File ini, Map map)
        {
            this.ROM = ROM;
            this.ini = ini;
            this.map = map;
            this.header = map.Header;


            InitializeComponent();

            // Create tileset list
            this.tilesetSel.Items.Clear();
            this.tilesetSel.Items.AddRange(this.ini.GetTilesetNames().ToArray());

            // Create song list
            this.songSel.Items.Clear();
            this.songSel.Items.AddRange(this.ini.GetSongNames().ToArray());

            // Set fields according to the current map's values
            this.mapXBox.Value = this.header.width;
            this.mapYBox.Value = this.header.height;

            // Special case : the tileset ID can be grabbed from the map 
            this.tilesetSel.SelectedIndex = this.map.TilesetID;
            this.updateTilesetPreview();

            this.SongID = this.header.musicID;
        }


        private void updateTilesetPreview()
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

        private void updateTilesetPreview(object sender, EventArgs e)
        {
            this.updateTilesetPreview();
        }

        private void updateBlockPreview(object sender, EventArgs e)
        {
            if (this.TilesetID < this.ini.GetProperty("nboftilesets"))
            {
                this.FillerBlockPreview.Image = new Bitmap(this.ROM.GetTileset(this.TilesetID, this.ini).GetBlockBMP(this.FillerID), FillerBlockPreview.Size);
            }
        }


        private void ApplyModifications(object sender, EventArgs e)
        {
            this.header.height = this.MapHeight;
            this.header.width = this.MapWidth;
            this.map.Resize(new Rectangle((int)this.mapXOrigin.Value, (int)this.mapYOrigin.Value, this.MapWidth, this.MapHeight), this.FillerID);

            this.header.tileset = ROM.GetTileset(this.TilesetID, this.ini);

            this.header.musicID = this.SongID;
            
            this.map.Header = this.header;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
