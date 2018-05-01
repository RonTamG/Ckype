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
            }
            return "OK";
        }
    }
}
