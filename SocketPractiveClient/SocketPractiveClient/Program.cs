using SocketsPractice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketsPractice
{

    class Program
    {

        private static ClientSocket clientSocket = new ClientSocket();

        static void Main(string[] args)
        {
            Console.Title = "Client";
            Console.WriteLine("Client started!");

            clientSocket.Connect("127.0.0.1", 6556);


            while (true)
            {
                string msg = Console.ReadLine();
                MessagePacket packet = new MessagePacket(msg);
                clientSocket.Send(packet.Data);
                if (msg.ToLower() == "exit")
                    Exit();
            }
        }

        /// <summary>
        /// Close socket and exit program.
        /// </summary>
        private static void Exit()
        {
            clientSocket.Close();
            Environment.Exit(0);
        }
    }
}
