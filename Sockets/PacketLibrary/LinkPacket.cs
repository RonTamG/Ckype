﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketLibrary
{
    public class LinkPacket : PacketStructure
    {

        public LinkPacket(Person dest, int port, ushort type = (ushort)type.LinkRequest)
            :base((ushort)(7 + dest.Data.Length), type)
        {
            WriteBool(false, 4);
            WriteUShort((ushort)port, 5);
            WriteByteArray(dest.Data, 7);
        }

        public LinkPacket(byte[] packet)
            : base(packet)
        {

        }

        public Person destClient
        {
            get { return new Person(ReadByteArray(7, Data.Length - 7)); }
        }

        public int port
        {
            get { return (int)(ReadUShort(5)); }
        }

        public bool AcceptedLink
        {
            get { return ReadBool(4); }
        }
    }
}
