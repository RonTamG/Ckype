﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketLibrary
{
    public class CallPacket : PacketStructure
    {

        public CallPacket(Person dest, ushort type = 4000)
            :base((ushort)(5 + dest.Data.Length), type)
        {
            WriteBool(false, 4);
            WriteByteArray(dest.Data, 5);
        }

        public CallPacket(byte[] packet)
            : base(packet)
        {

        }

        public void SetAcceptedCall()
        {
            WriteBool(true, 4);
        }

        public void toCheckType()
        {
            WriteUShort(4500, 2); // change type.
        }

        public Person destClient
        {
            get { return new Person(ReadByteArray(5, Data.Length - 5)); }
        }

        public bool acceptedCall
        {
            get { return ReadBool(4); }
        }
    }
}