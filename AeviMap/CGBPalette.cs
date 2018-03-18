using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeviMap
{
    public class CGBPalette
    {
        public static byte paletteSize = 4 * 3;


        private Color[] colors = new Color[4];

        public CGBPalette(byte[] rawBytes)
        {
            if(rawBytes.Length != 4 * 3)
            {
                throw new FormatException("Tried to load a palette with a wrong number of bytes (12 expected)");
            }

            byte[] correctedValues = new byte[3];
            for (uint i = 0; i < 4; i++)
            {
                for(byte j = 0; j < 3; j++)
                {
                    correctedValues[j] = (byte)((rawBytes[i * 3 + j] & 0x1F) << 3);
                }

                // TODO : Compute GBC LCD transformation here

                colors[i] = Color.FromArgb(
                    correctedValues[0],
                    correctedValues[1],
                    correctedValues[2]
                );
            }
        }


        public Color GetColor(byte colorID)
        {
            return this.colors[colorID];
        }
    }
}
