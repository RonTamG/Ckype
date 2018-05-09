using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketLibrary
{
    public class FilePacket : PacketStructure
    {
        private string _filename;
        private int _startingPos;
        private ushort _length;

        public FilePacket(string filename, ushort length, ushort startingPos, Person dest)
            : base((ushort)(length), 3000) // 4 = 2length + 2type // packet type 3000 = file
        {
            _filename = filename;
            _startingPos = startingPos;
            _length = length;
            WriteUShort(startingPos, 4);
            WriteUShort(length, 6);
            WriteUShort((ushort)dest.Data.Length, 8);
            WriteByteArray(dest.Data, 10);
            WriteUShort((ushort)filename.Length, 10 + dest.Data.Length);
            WriteString(filename, 12 + dest.Data.Length);
        }

        public FilePacket(byte[] packet)
            : base(packet)
        {
            _startingPos = ReadUShort(4);
            _length = ReadUShort(6);
            _filename = ReadString(12 + ReadUShort(8), ReadUShort(10 + ReadUShort(8)));
        }

        public string Filename
        {
            get {return _filename; }
        }

        public byte[] FileContents
        {
            get { return ReadByteArray(_startingPos, _length - _startingPos); }
        }
        public Person destClient
        {
            get { return new Person(ReadByteArray(10, ReadUShort(8))); }
        }
    }
}
