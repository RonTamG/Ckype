using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketLibrary
{
    public class MessagePacket : PacketStructure
    {
        private string _message;

        public MessagePacket(string message)
            : base((ushort)(6 + message.Length), 2000) // 4 = 2length + 2type // packet type 2000 = message/string
        {
            Text = message;
        }

        public MessagePacket(string message, Person dest)
    : base((ushort)(6 + message.Length + dest.Data.Length), 2000) // 4 = 2length + 2type + 2destlength packet type 2000 = message/string
        {           
            WriteUShort((ushort)dest.Data.Length, 4);
            WriteByteArray(dest.Data, 6);
            Text = message;
        }

        public MessagePacket(byte[] packet)
            : base(packet)
        {

        }

        public Person destClient
        {
            get {return new Person(ReadByteArray(6, ReadUShort(4))); }
        }

        public string Text
        {
            get { return ReadString(6 + ReadUShort(4), Data.Length - (6+ReadUShort(4))); }
            set
            {
                _message = value;
                WriteString(value, 6 + ReadUShort(4));
            }
        }
    }
}