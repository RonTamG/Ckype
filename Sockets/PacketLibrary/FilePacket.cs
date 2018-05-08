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

        public FilePacket(string filename, ushort length, ushort startingPos)
            : base((ushort)(length), 3000) // 4 = 2length + 2type // packet type 3000 = file
        {
            _filename = filename;
            _startingPos = startingPos;
            _length = length;
            WriteUShort(startingPos, 4);
            WriteUShort(length, 6);
            WriteUShort((ushort)filename.Length, 8);
            WriteString(filename, 10);
        }

        public FilePacket(byte[] packet)
            : base(packet)
        {
            _startingPos = ReadUShort(4);
            _length = ReadUShort(6);
            _filename = ReadString(6, ReadUShort(4));
        }

        public string Filename
        {
            get {return _filename; }
        }

        public string FileContents
        {
            get { return ReadString(_startingPos, _length - _startingPos); } // return ReadString(4 + _filename.Length, Data.Length - (4 + _filename.Length))
        }
    }
}
