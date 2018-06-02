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
        private uint _totalFilelength;

        public FilePacket(string filename, ushort length, uint totalFileLength, ushort startingPos, Person dest)
            : base((ushort)(length), (ushort)type.File) // 4 = 2length + 2type // packet type 3000 = file
        {
            _filename = filename;
            _startingPos = startingPos;
            _length = length;
            _totalFilelength = totalFileLength;
            WriteUInt(totalFileLength, 4);
            WriteUShort(startingPos, 8);
            WriteUShort(length, 10);
            WriteUShort((ushort)dest.Data.Length, 12);
            WriteByteArray(dest.Data, 14);
            WriteUShort((ushort)filename.Length, 14 + dest.Data.Length);
            WriteString(filename, 16 + dest.Data.Length);
        }

        public FilePacket(byte[] packet)
            : base(packet)
        {
            _startingPos = ReadUShort(8);
            _length = ReadUShort(10);
            _filename = ReadString(16 + ReadUShort(12), ReadUShort(14 + ReadUShort(12)));
            _totalFilelength = ReadUInt(4);
        }

        public uint TotalFileLength
        {
            get { return _totalFilelength; }
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
            get { return new Person(ReadByteArray(14, ReadUShort(12))); }
        }

        public void SetFinishedType()
        {
            WriteUShort((ushort)type.FileFinished, 2);
        }
    }
}
