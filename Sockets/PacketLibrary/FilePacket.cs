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
        private string _fileContents;

        public FilePacket(string filename)
            : base((ushort)(4 + filename.Length), 3000) // 4 = 2length + 2type // packet type 3000 = file
        {
            _filename = filename;
            ReadFile();
        }

        public FilePacket(byte[] packet)
            : base(packet)
        {

        }

        public string FileContents
        {
            get { return ReadString(4 , Data.Length - (4)); } // return ReadString(4 + _filename.Length, Data.Length - (4 + _filename.Length))
        }

        public void ReadFile()
        {
            _fileContents = "";
            String line;
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(_filename); // change to filename TEST

                //Read the first line of text
                line = sr.ReadLine();

                //Continue to read until you reach end of file
                while (line != null)
                {
                    //write the line to console window
//                    Console.WriteLine(line);
                    _fileContents += line;
                    //Read the next line
                    line = sr.ReadLine();
                }

                //close the file
                sr.Close();
                WriteString(_fileContents, 4 + _filename.Length);
                Console.ReadLine();
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
