using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            //AudioLibraryClass.FilePlay(@"C:\Users\Owner\Downloads\The-Sound-of-Silence-Original-Version-from-1964.wav");
            AudioLibraryClass.PlayRecordedTestSockets();
            Console.ReadLine();
        }
    }
}
