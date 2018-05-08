using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PacketLibrary;


namespace Client
{
    public static class PacketHandler
    {
        public static string Handle(ClientSocket clientSocket, byte[] packet, Socket serverSocket)
        {
            ushort packetLength = BitConverter.ToUInt16(packet, 0);
            ushort packetType = BitConverter.ToUInt16(packet, 2);

            Console.WriteLine("Received packet: Length: {0} | Type: {1}", packetLength, packetType);
            switch (packetType)
            {
                case 1000:
                    Person addr = new Person(packet);
                    Console.WriteLine("name: " + addr.name + " Connected from: " + addr.ip + ":" + addr.port);
                    break;
                case 2000:
                    MessagePacket msg = new MessagePacket(packet);

                    string response = string.Empty;

                    if (msg.Text == "Invalid Request")
                    {
                        Console.WriteLine("Invalid Input please enter a different message -->");
                    }
                    else
                    {
                        Console.WriteLine(msg.Text);
                    }
                    break;
                case 4000: // Not in use yet
                    Console.WriteLine("Server has disconnected");
                    serverSocket.Shutdown(SocketShutdown.Both);
                    serverSocket.Close();
                    clientSocket.Close();
                    Console.WriteLine("Program will exit now press any key to continue...");
                    Console.ReadLine();
                    break;              
                default:
                    Console.WriteLine(Encoding.UTF8.GetString(packet));
                    break;
            }
            return "OK";
        }
    }
}
