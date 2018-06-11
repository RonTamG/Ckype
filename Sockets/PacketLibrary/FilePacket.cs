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
        private uint _length;
        private uint _totalFilelength;

        public FilePacket(string filename, uint length, uint totalFileLength, ushort startingPos, Person dest)
            : base((uint)(length), (ushort)type.File) // 4 = 2length + 2type // packet type 3000 = file
        {
            _filename = filename;
            _startingPos = startingPos;
            _length = length;
            _totalFilelength = totalFileLength;
            WriteUInt(totalFileLength, 6);
            WriteUShort(startingPos, 10);
            WriteUInt(length, 12);
            WriteUShort((ushort)dest.Data.Length, 16);
            WriteByteArray(dest.Data, 18);
            WriteUShort((ushort)filename.Length, 18 + dest.Data.Length);
            WriteString(filename, 20 + dest.Data.Length);
        }

        public FilePacket(byte[] packet)
            : base(packet)
        {
            _startingPos = ReadUShort(10);
            _length = ReadUInt(12);
            _filename = ReadString(20 + ReadUShort(16), ReadUShort(18 + ReadUShort(16)));
            _totalFilelength = ReadUInt(6);
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
            get { return ReadByteArray(_startingPos, (int)(_length - _startingPos)); }
        }
        public Person destClient
        {
            get { return new Person(ReadByteArray(18, ReadUShort(16))); }
        }

        public void SetFinishedType()
        {
            WriteUShort((ushort)type.FileFinished, 4);
        }
    }
}
