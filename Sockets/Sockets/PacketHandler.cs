using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PacketLibrary;
using System.Net;

namespace Server
{
    public static class PacketHandler
    {
        public static string Handle(ServerSocket serverSocket, byte[] packet, Socket clientSocket)
        {
            ushort packetLength = BitConverter.ToUInt16(packet, 0);
            ushort packetType = BitConverter.ToUInt16(packet, 2);

            Console.WriteLine("Received packet: Length: {0} | Type: {1}", packetLength, packetType);

            switch (packetType)
            {
                case 1000:
                    //Send all connected clients information of the newly connected one.
                    Person person = new Person(packet);
                    person.ownSocket = clientSocket;
                    serverSocket.connected.Add(person);
                    serverSocket.ConnectedSockets();
                    break;
                case 2000:
                    MessagePacket msg = new MessagePacket(packet);
                    Console.WriteLine(msg.Text);

                    string response = string.Empty;

                    if (msg.Text.ToLower() == "exit")
                    {
                        // Always Shutdown before closing
                        clientSocket.Shutdown(SocketShutdown.Both);
                        clientSocket.Close();
                        serverSocket.connected.Remove(serverSocket.FindPersonBySocket(clientSocket));
                        serverSocket.ConnectedSockets();
                        Console.WriteLine("Client disconnected");
                        return "closed client";
                    }
                    if (msg.Text.ToLower() != "get time")
                    {
                        response = "Invalid Request";
                    }
                    else
                    {
                        response = DateTime.Now.ToLongTimeString();
                    }

                    MessagePacket ToSend = new MessagePacket(response);
                    clientSocket.Send(ToSend.Data);
                    Console.WriteLine("Sent" + response);
                    break;
                case 3000:
                    FilePacket file = new FilePacket(packet);
                    Console.WriteLine(file.FileContents);
                    break;
            }
            return "OK";
        }
    }
}
