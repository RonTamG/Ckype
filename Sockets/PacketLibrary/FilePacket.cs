using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketLibrary
{
    public class FilePacket : PacketStructure
    {
        private string _filename;
        public FileStream fs;
        public bool status;
        const int bufferCapacity = 1024;

        public FilePacket(string filename, string ipAddress, ushort port)
            : base((ushort)(bufferCapacity), 3000, ipAddress, port) // 4 = 2length + 2type // packet type 3000 = file
        {
            _filename = filename;
            fs = new FileStream(_filename, FileMode.Open, FileAccess.Read);
            status = true;
            WriteUShort((ushort)filename.Length, 8 + ReadUShort(6));
            WriteString(filename, 10 + ReadUShort(6));
            ReadFileChunk();
        }

        public FilePacket(byte[] packet)
            : base(packet)
        {
            _filename = ReadString(10 + ReadUShort(6), ReadUShort(8));
        }

        public string FileContents
        {
            get { return ReadString(10 + _filename.Length + ReadUShort(6), bufferCapacity - (10 + _filename.Length + ReadUShort(6))); } // return ReadString(4 + _filename.Length, Data.Length - (4 + _filename.Length))
        }

        public void ReadFileChunk()
        {
            if (!(fs.Position == fs.Length))
                fs.Read(Data, 10 + _filename.Length + ReadUShort(6), bufferCapacity - (10 + _filename.Length + ReadUShort(6)));
            else
            {                
                fs.Close();
                status = false;
            }
        }
        
        public void WriteFile(string text)
        {
            try
            {

                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(_filename);

                //Write a line of text
                sw.WriteLine(text);

                //Close the file
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }

        
    }
}
