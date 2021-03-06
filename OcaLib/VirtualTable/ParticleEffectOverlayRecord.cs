﻿using mzxrules.Helper;
using System.IO;

namespace mzxrules.OcaLib
{
    public class ParticleOverlayRecord : OverlayRecord
    {
        //0x00 VROM
        //0x08 VRAM
        //0x10 RamStart
        public int Id { get; protected set; }
        public N64Ptr UnknownPtr { get; private set; }
        public uint Unknown1 { get; private set; }

        public const int LENGTH = 0x1C;

        public ParticleOverlayRecord()
        { }

        public ParticleOverlayRecord(ParticleOverlayRecord p)
        {
            Id = p.Id;
            VRom = p.VRom;
            VRam = p.VRam;

            RamStart = p.RamStart;
            UnknownPtr = p.UnknownPtr;
            Unknown1 = p.Unknown1;
        }

        public ParticleOverlayRecord(int index, BinaryReader br)
        {
            Initialize(index, br);
        }

        public ParticleOverlayRecord(int index, byte[] data)
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
                Initialize(index, br);
        }

        private void Initialize(int index, BinaryReader br)
        {
            Id = index;
            VRom = new FileAddress(br.ReadBigUInt32(), br.ReadBigUInt32());
            VRam = new N64PtrRange(br.ReadBigUInt32(), br.ReadBigUInt32());

            RamStart = br.ReadBigUInt32();
            UnknownPtr = br.ReadBigUInt32();
            Unknown1 = br.ReadBigUInt32();
        }
    }
}
