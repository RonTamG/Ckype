using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketsPractice
{
    class Program
    {
        public static ServerSocket serverSocket = new ServerSocket();

        static void Main(string[] args)
        {
            serverSocket.Bind(6556);
            serverSocket.Listen(1);
            serverSocket.Accept();

            Console.WriteLine("Server started!");

            while (true)
                Console.ReadLine();
        }
    }
}
