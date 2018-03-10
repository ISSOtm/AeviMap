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
        public mapCreator()
        {
            InitializeComponent();
        }

        private void updatePreview(object sender, EventArgs e)
        {
            switch (tilesetSel.SelectedIndex)
            {
                case 0:
                    tilesetPrev.Image = tilesetDB.outdoor_tileset;
                    break;
                case 1:
                    tilesetPrev.Image = tilesetDB.beach_tileset;
                    break;
                case 2:
                    tilesetPrev.Image = tilesetDB.indoor_tileset;
                    break;
                case 3:
                    tilesetPrev.Image = tilesetDB.tilesetruins;
                    break;
                default:
                    tilesetPrev.Image = tilesetDB.unknown_tileset;
                    break;
            }
        }
    }
}
