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


        public headerEditor(GB_ROM ROM, INI_File ini)
        {
            this.ROM = ROM;
            this.ini = ini;


            InitializeComponent();

            this.tilesetSel.Items.Clear();
            this.tilesetSel.Items.AddRange(this.ini.GetTilesetNames().ToArray());
        }

        private void updatePreview(object sender, EventArgs e)
        {
            byte tilesetID = (byte)tilesetSel.SelectedIndex;
            if (tilesetID > this.ini.GetProperty("nboftilesets"))
            {
                tilesetPrev.Image = tilesetDB.unknown_tileset;
            }
            else
            {
                tilesetPrev.Image = this.ROM.GetTileset(tilesetID, this.ini).GetPreviewImage();
            }
        }

        private void CloseDialog(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
