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
                    serverSocket.ConnectedSockets(person);
                    serverSocket.NewPersonOnlineOffline(person);
                    serverSocket.connected.Add(person);
                    break;
                case 2000:
                    MessagePacket msg = new MessagePacket(packet);
                    Console.WriteLine("Received message: " + msg.Text);

                    //string response = string.Empty;

                    if (msg.Text.ToLower() == "exit")
                    {
                        // Always Shutdown before closing
                        clientSocket.Shutdown(SocketShutdown.Both);
                        clientSocket.Close();
                        Person disconnected = serverSocket.FindPersonBySocket(clientSocket);
                        disconnected.SetDisconnectedType();
                        serverSocket.NewPersonOnlineOffline(disconnected);
                        serverSocket.connected.Remove(serverSocket.FindPersonBySocket(clientSocket));
                        Console.WriteLine("Client disconnected");
                        return "closed client";
                    }
                    else
                    {
                        MessagePacket ToSend = new MessagePacket(msg.Text, serverSocket.FindPersonBySocket(clientSocket));
                        Person.FindPersonByIPandPort(msg.destClient, serverSocket.connected).ownSocket.Send(ToSend.Data);
                        Console.WriteLine("Sent: " + msg.Text + " To: " + msg.destClient + " From: " + serverSocket.FindPersonBySocket(clientSocket));
                    }
                    break;
                case 3000:
                    FilePacket file = new FilePacket(packet);
                    Console.WriteLine("Received file called: '{0}' was sent to: {1}", file.Filename, file.destClient );
                    Person.FindPersonByIPandPort(file.destClient, serverSocket.connected).ownSocket.Send(file.Data);
                    Console.WriteLine("Sent!");
                    break;
                case 4000:
                    CallPacket callRequest = new CallPacket(packet);
                    Console.WriteLine("Received call request");
                    CallPacket revRequest = new CallPacket(serverSocket.FindPersonBySocket(clientSocket));
                    Person.FindPersonByIPandPort(callRequest.destClient, serverSocket.connected).ownSocket.Send(revRequest.Data);
                    break;
                case 4500:
                    CallPacket callRequestBack = new CallPacket(packet);
                    Console.WriteLine("Received call request");
                    CallPacket revRequestBack = new CallPacket(serverSocket.FindPersonBySocket(clientSocket), 4500);
                    Person.FindPersonByIPandPort(callRequestBack.destClient, serverSocket.connected).ownSocket.Send(revRequestBack.Data);
                    break;
            }
            return "OK";
        }
    }
}
