using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeviMap
{
    class InteractionBox
    {
        public UInt16 XPos { get; }
        public UInt16 YPos { get; }
        public byte XSize { get; }
        public byte YSize { get; }


        public InteractionBox(UInt16 YPos, UInt16 XPos, byte YSize, byte XSize)
        {
            this.YPos = YPos;
            this.XPos = XPos;
            this.YSize = YSize;
            this.XSize = XSize;
        }
    }
}
