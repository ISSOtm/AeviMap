using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeviMap
{
    public class NPC
    {
        private InteractionBox Box;

        public byte InteractionID { get; }
        public byte SpriteID { get; }
        public byte[] Palettes { get; }

        private bool CanTurnVert;
        private bool CanTurnHoriz;
        private bool CanMoveVert;
        private bool CanMoveHoriz;
        private bool HasMovement;
        public byte MovementSpeed { get; }


        public NPC(GB_ROM ROM, byte bank, UInt16 ptr)
        {
            UInt16 YPos = ROM.GetShort(bank, ptr);
            ptr += 2;
            UInt16 XPos = ROM.GetShort(bank, ptr);
            ptr += 2;
            byte YSize = ROM.GetByte(bank, ptr++);
            byte XSize = ROM.GetByte(bank, ptr++);
            this.Box = new InteractionBox(YPos, XPos, YSize, XSize);

            this.InteractionID = ROM.GetByte(bank, ptr++);
            this.SpriteID = ROM.GetByte(bank, ptr++);

            UInt16 rawPalettes = ROM.GetShort(bank, ptr);
            ptr += 2;
            this.Palettes = new byte[4] { (byte)((rawPalettes & 0x7000) >> 12), (byte)((rawPalettes & 0x700) >> 8), (byte)((rawPalettes & 0x70) >> 4), (byte)(rawPalettes & 0x7) };

            this.SetMovementFlags(ROM.GetByte(bank, ptr++));
            this.MovementSpeed = ROM.GetByte(bank, ptr++);
        }


        public byte GetMovementFlags()
        {
            byte flags = 0;

            if (this.CanTurnVert) { flags |= 0x80; }
            if (this.CanTurnHoriz) { flags |= 0x40; }
            if (this.CanMoveVert) { flags |= 0x20; }
            if (this.CanMoveHoriz) { flags |= 0x10; }
            if (this.HasMovement) { flags |= 0x04; }

            return flags;
        }

        public void SetMovementFlags(byte flags)
        {
            this.CanTurnVert = (flags & 0x80) != 0;
            this.CanTurnHoriz = (flags & 0x40) != 0;
            this.CanMoveVert = (flags & 0x20) != 0;
            this.CanMoveHoriz = (flags & 0x10) != 0;

            this.HasMovement = (flags & 0x04) != 0;
        }
    }
}
