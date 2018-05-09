﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketLibrary
{
    public abstract class PacketStructure
    {
        private byte[] _buffer;

        public PacketStructure(ushort length, ushort type) // Person NEEDS THIS ONE
        {
            _buffer = new byte[length];
            WriteUShort(length, 0);
            WriteUShort(type, 2);
        }

        public PacketStructure(byte[] packet)
        {
            _buffer = packet;
        }

        public void WriteUShort(ushort value, int offset)
        {
            byte[] tempBuf = new byte[2];
            tempBuf = BitConverter.GetBytes(value);
            Array.Copy(tempBuf, 0, _buffer, offset, 2); // Can be improved, marshal and so on instead of array.copy
        }

        public void WriteUInt(uint value, int offset)
        {
            byte[] tempBuf = new byte[4];
            tempBuf = BitConverter.GetBytes(value);
            Array.Copy(tempBuf, 0, _buffer, offset, 4);
        }

        public void WriteString(string value, int offset)
        {
            byte[] tempBuf = new byte[value.Length];
            tempBuf = Encoding.UTF8.GetBytes(value);
            Array.Copy(tempBuf, 0, _buffer, offset, value.Length);
        }

        public void WriteByteArray(byte[] arr, int offset)
        {
            Array.Copy(arr, 0, _buffer, offset, arr.Length);
        }

        public byte[] ReadByteArray(int offset, int length)
        {
            byte[] result = new byte[length];
            Array.Copy(_buffer, offset, result, 0, length);
            return result;
        }

        public ushort ReadUShort(int offset)
        {
            return BitConverter.ToUInt16(_buffer, offset);
        }

        public string ReadString(int offset, int count)
        {
            return Encoding.UTF8.GetString(_buffer, offset, count);
        }

        public byte[] Data { get { return _buffer; } }
    }
}
