using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeviMap
{
    public class WarpTo
    {
        public UInt16 YPos { get; }
        public UInt16 XPos { get; }
        public Direction Dir { get; }
        public byte Flags { get; }
        public byte CameramanID { get; }
        public Thread2Function Thread2 { get; }
        public UInt16 LoadingFuncPtr { get; }


        public WarpTo(GB_ROM ROM, byte bank, UInt16 addr)
        {
            this.YPos = ROM.GetShort(bank, addr);
            addr += 2;
            this.XPos = ROM.GetShort(bank, addr);
            addr += 2;

            this.Dir = (Direction)ROM.GetByte(bank, addr++);
            this.Flags = ROM.GetByte(bank, addr++);
            this.CameramanID = ROM.GetByte(bank, addr++);
            this.Thread2 = (Thread2Function)ROM.GetByte(bank, addr++);
            this.LoadingFuncPtr = ROM.GetShort(bank, addr);
        }
    }
}
