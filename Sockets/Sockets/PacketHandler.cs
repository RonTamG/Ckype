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

            switch ((type)packetType)
            {
                case type.PersonConnected:
                    //Send all connected clients information of the newly connected one.
                    Console.WriteLine("Client connected");
                    Person person = new Person(packet);
                    person.ownSocket = clientSocket;
                    serverSocket.SendConnections(person);
                    serverSocket.NewPersonOnlineOffline(person);
                    serverSocket.connected.Add(person);
                    break;

                case type.PersonDisconnected:
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    Person disconnected = new Person(packet);
                    Console.WriteLine("Disconnected: " + disconnected);
                    serverSocket.connected.Remove(serverSocket.FindPersonBySocket(clientSocket));
                    serverSocket.NewPersonOnlineOffline(disconnected);
                    Console.WriteLine("Client disconnected");
                    return "closed client";

                case type.PersonRefresh:
                    Person ConnectionsRequester = serverSocket.FindPersonBySocket(clientSocket);
                    ConnectionsRequester.ownSocket = clientSocket;
                    serverSocket.SendConnections(ConnectionsRequester);
                    break;

                case type.Message:
                    MessagePacket msg = new MessagePacket(packet);
                    Console.WriteLine("Received message: " + msg.Text);

                    MessagePacket ToSend = new MessagePacket(msg.Text, serverSocket.FindPersonBySocket(clientSocket));
                    Person.FindPersonByIPandPort(msg.destClient, serverSocket.connected).ownSocket.Send(ToSend.Data);
                    Console.WriteLine("Sent: " + msg.Text + " To: " + msg.destClient + " From: " + serverSocket.FindPersonBySocket(clientSocket));
                    break;

                case type.File:
                    FilePacket file = new FilePacket(packet);
                    Console.WriteLine("Received file called: '{0}' was sent to: {1}", file.Filename, file.destClient);
                    Person.FindPersonByIPandPort(file.destClient, serverSocket.connected).ownSocket.Send(file.Data);
                    Console.WriteLine("Sent!");
                    break;

                case type.CallRequest:
                    CallPacket callRequest = new CallPacket(packet);
                    Console.WriteLine("Received call request");
                    CallPacket revRequest = new CallPacket(serverSocket.FindPersonBySocket(clientSocket));
                    Person.FindPersonByIPandPort(callRequest.destClient, serverSocket.connected).ownSocket.Send(revRequest.Data);
                    break;

                case type.CallResponse:
                    CallPacket callRequestBack = new CallPacket(packet);
                    Console.WriteLine("Received call request");
                    CallPacket revRequestBack = new CallPacket(serverSocket.FindPersonBySocket(clientSocket), (ushort)type.CallResponse);
                    Person.FindPersonByIPandPort(callRequestBack.destClient, serverSocket.connected).ownSocket.Send(revRequestBack.Data);
                    break;

                case type.CallHangUp:
                    CallPacket hangUp = new CallPacket(packet);
                    Console.WriteLine("Sending hangup request to: {0}", hangUp.destClient);
                    CallPacket hangUpRequest = new CallPacket(serverSocket.FindPersonBySocket(clientSocket), (ushort)type.CallHangUp);
                    Console.WriteLine("From: {0}", hangUpRequest.destClient);
                    Person.FindPersonByIPandPort(hangUp.destClient, serverSocket.connected).ownSocket.Send(hangUpRequest.Data);
                    break;
            }
            return "OK";
        }
    }
}