using PacketLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{

    class Program
    {

        private static ClientSocket clientSocket;

        static void Main(string[] args)
        {
            Console.Title = "Client";
            Console.WriteLine("Client started!");
            Console.WriteLine("Enter nickname");
            string nickname = Console.ReadLine();
            clientSocket = new ClientSocket(nickname);

            clientSocket.Connect("127.0.0.1", 6556);


            while (true)
            {
                string msg = Console.ReadLine();
//                Console.WriteLine("Sending: " + msg);
//                MessagePacket packet = new MessagePacket(msg);
//                clientSocket.Send(packet.Data);
                if (msg.ToLower() == "exit")
                    Exit();
                
                if (msg.ToLower() == "send file")
                {
                    string filename = Console.ReadLine();
                    clientSocket.SendFile(filename);
                }
                else
                    Console.WriteLine("Sending: " + msg);
                    MessagePacket packet = new MessagePacket(msg);
                    clientSocket.Send(packet.Data);
            }
        }

        /// <summary>
        /// Close socket and exit program.
        /// </summary>
        private static void Exit()
        {
            MessagePacket packet = new MessagePacket("exit");
            clientSocket.Send(packet.Data);
            clientSocket.Close();
            Environment.Exit(0);
        }
    }
}
