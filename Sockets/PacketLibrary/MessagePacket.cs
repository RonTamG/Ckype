﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketLibrary
{
    public class MessagePacket : PacketStructure
    {
        private string _message;

        public MessagePacket(string message, string ipAddress, ushort port)
            : base((ushort)(10 + ipAddress.Length + message.Length), 2000, ipAddress, port) // 4 = 2length + 2type // packet type 2000 = message/string
        {
            Text = message;
        }
/*        public MessagePacket(string message)
            : base((ushort)(4 + message.Length), 2000) // 4 = 2length + 2type // packet type 2000 = message/string
        {
            Text = message;
        }*/

        public MessagePacket(byte[] packet)
            : base(packet)
        {

        }

        public string Text
        {
            get { return ReadString(10 + ReadUShort(6), Data.Length - 10 + ReadUShort(6)); }
            set
            {
                _message = value;
                WriteString(value, 10 + ReadUShort(6));
            }
        }
    }
}