using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PacketLibrary;
using Ckype.Core;

namespace Server
{
    public static class PacketHandler
    {
        public static string Handle(ServerSocket serverSocket, byte[] packet, Socket clientSocket)
        {
            ushort packetLength = BitConverter.ToUInt16(packet, 0);
            ushort packetType = BitConverter.ToUInt16(packet, 2);

            Logger.LogMessage($"Received packet: Length: {packetLength} | Type: {packetType}");

            switch ((type)packetType)
            {
                case type.PersonConnected:
                    //Send to all connected clients the information of the newly connected one.
                    Logger.LogMessage("Client connected");
                    Person person = new Person(packet)
                    {
                        OwnSocket = clientSocket
                    };
                    serverSocket.SendConnections(person);
                    serverSocket.NewPersonOnlineOffline(person);
                    serverSocket.connected.Add(person);
                    break;

                case type.PersonDisconnected:
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    Person disconnected = new Person(packet);
                    Logger.LogMessage("Disconnected: " + disconnected);
                    serverSocket.connected.Remove(serverSocket.FindPersonBySocket(clientSocket));
                    serverSocket.NewPersonOnlineOffline(disconnected);
                    Logger.LogMessage("Client disconnected");
                    return "Closed Client";

                case type.PersonRefresh:
                    Person ConnectionsRequester = serverSocket.FindPersonBySocket(clientSocket);
                    ConnectionsRequester.OwnSocket = clientSocket;
                    serverSocket.SendConnections(ConnectionsRequester);
                    break;

                case type.Message:
                    MessagePacket msg = new MessagePacket(packet);
                    Logger.LogMessage("Received message: " + msg.Text);

                    MessagePacket ToSend = new MessagePacket(msg.Text, serverSocket.FindPersonBySocket(clientSocket));
                    Person.FindPersonByIPandPort(msg.destClient, serverSocket.connected).OwnSocket.Send(ToSend.Data);
                    Logger.LogMessage("Sent: " + msg.Text + " To: " + msg.destClient + " From: " + serverSocket.FindPersonBySocket(clientSocket));
                    break;

                case type.File:
                    FilePacket file = new FilePacket(packet);
                    Logger.LogMessage($"Received file called: '{file.Filename}' was sent to: {file.destClient}");
                    Person.FindPersonByIPandPort(file.destClient, serverSocket.connected).OwnSocket.Send(file.Data);
                    Logger.LogMessage("Sent!");
                    break;

                case type.CallRequest:
                    CallPacket callRequest = new CallPacket(packet);
                    Logger.LogMessage("Received call request");
                    CallPacket revRequest = new CallPacket(serverSocket.FindPersonBySocket(clientSocket));
                    Person.FindPersonByIPandPort(callRequest.destClient, serverSocket.connected).OwnSocket.Send(revRequest.Data);
                    break;

                case type.CallResponse:
                    CallPacket callRequestBack = new CallPacket(packet);
                    Logger.LogMessage("Received call request");
                    CallPacket revRequestBack = new CallPacket(serverSocket.FindPersonBySocket(clientSocket), (ushort)type.CallResponse);
                    if (callRequestBack.acceptedCall)
                        revRequestBack.SetAcceptedCall();
                    Person.FindPersonByIPandPort(callRequestBack.destClient, serverSocket.connected).OwnSocket.Send(revRequestBack.Data);
                    break;

                case type.CallHangUp:
                    CallPacket hangUp = new CallPacket(packet);
                    Logger.LogMessage($"Sending hangup request to: {hangUp.destClient}");
                    CallPacket hangUpRequest = new CallPacket(serverSocket.FindPersonBySocket(clientSocket), (ushort)type.CallHangUp);
                    Logger.LogMessage($"From: {hangUpRequest.destClient}");
                    Person.FindPersonByIPandPort(hangUp.destClient, serverSocket.connected).OwnSocket.Send(hangUpRequest.Data);
                    break;

                case type.LinkRequest:
                    LinkPacket linkRequest = new LinkPacket(packet);
                    LinkPacket revLinkRequest = new LinkPacket(serverSocket.FindPersonBySocket(clientSocket), linkRequest.port);
                    Person.FindPersonByIPandPort(linkRequest.destClient, serverSocket.connected).OwnSocket.Send(revLinkRequest.Data);
                    break;
            }
            return "OK";
        }
    }
}