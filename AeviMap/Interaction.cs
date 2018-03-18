using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeviMap
{
    public enum InteractionType
    {
        WALK_INTERACT,
        BTN_INTERACT,
        WALK_LOADZONE,
        BTN_LOADZONE
    }


    public abstract class Interaction
    {
        public InteractionType Type { get; }

        public bool FlagDep { get; }
        public bool FlagDepType { get; }
        public UInt16 FlagID { get; }

        private InteractionBox Box;

        public byte size { get; }


        protected byte bank;
        protected UInt16 ptr;


        public Interaction(GB_ROM ROM, byte bank, UInt16 ptr)
        {
            this.size = 16 + 1;

            byte type = ROM.GetByte(bank, ptr++);
            this.Type = (InteractionType)(type & 3);

            this.FlagDep = (type & 0x80) != 0;
            if(this.FlagDep)
            {
                this.size += 2;

                UInt16 ID = ROM.GetShort(bank, ptr);
                ptr += 2;
                this.FlagDepType = (ID & 0x8000) != 0;
                this.FlagID = (UInt16)(ID & 0x7FFF);
            }

            UInt16 YPos = ROM.GetShort(bank, ptr);
            ptr += 2;
            UInt16 XPos = ROM.GetShort(bank, ptr);
            ptr += 2;
            byte YSize = ROM.GetByte(bank, ptr++);
            byte XSize = ROM.GetByte(bank, ptr++);
            this.Box = new InteractionBox(YPos, XPos, YSize, XSize);


            this.bank = bank;
            this.ptr = ptr;
        }


        public UInt16 XPos()
        {
            return this.Box.XPos;
        }

        public UInt16 YPos()
        {
            return this.Box.YPos;
        }

        public byte XSize()
        {
            return this.Box.XSize;
        }

        public byte YSize()
        {
            return this.Box.YSize;
        }
    }


    public class LoadingZone : Interaction
    {
        public Thread2Function Thread2Func { get; }
        public byte TargetWarpID { get; }
        public byte TargetMapID { get; }
        public SFX_ID SFX { get; }


        public LoadingZone(GB_ROM ROM, byte bank, UInt16 ptr) : base(ROM, bank, ptr)
        {
            this.Thread2Func = (Thread2Function)ROM.GetByte(this.bank, this.ptr++);
            this.TargetWarpID = ROM.GetByte(this.bank, this.ptr++);
            this.TargetMapID = ROM.GetByte(this.bank, this.ptr++);
            this.SFX = (SFX_ID)ROM.GetByte(this.bank, this.ptr++);
        }
    }


    public class InteractionTrigger : Interaction
    {
        public UInt16 TextScriptPtr { get; }


        public InteractionTrigger(GB_ROM ROM, byte bank, UInt16 ptr) : base(ROM, bank, ptr)
        {
            this.TextScriptPtr = ROM.GetShort(this.bank, this.ptr);
            this.ptr += 2;
        }
    }
}
