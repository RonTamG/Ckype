using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PacketLibrary;
using System.IO;
using System.Threading;
using Server;
using System.Net;

namespace Client
{
    public delegate void FriendEvent(Person person);
    public delegate void FriendsEvent(List<Person> people);
    public delegate void MessageEvent(Person person, string message);
    public delegate void CallingEvent(ref CallPacket packet);
    public delegate void LinkEvent(Person person);
    public delegate void FileEvent(string filepath, Person person);

    public static class PacketHandler
    {
        public static event FriendEvent FriendAddedEvent;
        public static event FriendEvent FriendRemovedEvent;
        public static event FriendsEvent FriendsReceivedEvent;
        public static event MessageEvent FriendMessageReceivedEvent;
        public static event FileEvent FileReceivedEvent;
        public static event CallingEvent CallingEvent;
        public static event CallingEvent AcceptedCallEvent;
        public static event CallingEvent DeclinedCallEvent;
        public static event CallingEvent CancelledCallEvent;

        public static string Handle(ClientSocket clientSocket, byte[] packet, Socket serverSocket)
        {
            ushort packetLength = BitConverter.ToUInt16(packet, 0);
            ushort packetType = BitConverter.ToUInt16(packet, 2);
            
            Console.WriteLine("Received packet: Length: {0} | Type: {1}", packetLength, packetType);
            switch ((type)packetType)
            {
                case type.PersonConnected:
                    Person person = new Person(packet);
                    clientSocket.Friends.Add(person);
                    FriendAddedEvent?.Invoke(person);
                    Console.WriteLine("name: " + person.name + " Connected from: " + person.ip + ":" + person.port);
                    break;

                case type.PersonDisconnected:
                    Person disconnected = new Person(packet);
                    clientSocket.Friends.Remove(Person.FindPersonByIPandPort(disconnected, clientSocket.Friends));
                    FriendRemovedEvent?.Invoke(disconnected);
                    Console.WriteLine(disconnected + " Disconnected from the server");
                    break;

                case type.PersonRefresh:
                    ConnectionsPacket ConPacket = new ConnectionsPacket(packet);
                    clientSocket.Friends = ConPacket.People;
                    FriendsReceivedEvent?.Invoke(clientSocket.Friends);
                    Console.WriteLine("Received All Friends");
                    break;

                case type.Message:
                    MessagePacket msg = new MessagePacket(packet);

                    if (msg.Text == "SecretDisconnectedServerMessage")
                    {
                        Console.WriteLine("Server has disconnected");
                        clientSocket.Close();
                        Console.WriteLine("Program will exit now press any key to continue...");
                    }
                    else
                    {
                        FriendMessageReceivedEvent(msg.destClient, msg.Text);
                        Console.WriteLine(msg.destClient + " Sent: " + msg.Text);
                    }
                    break;

                case type.CallRequest:
                    CallPacket callP = new CallPacket(packet);
                    CallingEvent(ref callP); // event to get input from user
                    Console.WriteLine("Received a call request from: {0}", callP.destClient);
                    break;

                case type.CallResponse:
                    CallPacket checkCallP = new CallPacket(packet);
                    if (checkCallP.acceptedCall)
                    {
                        Console.WriteLine("Your friend: {0} has accepted the call!", checkCallP.destClient);
                        // Call friend
                        AcceptedCallEvent(ref checkCallP);
                    }
                    else
                    {
                        Console.WriteLine("Your friend: {0} has declined the call.", checkCallP.destClient);
                        // Decline call
                        DeclinedCallEvent(ref checkCallP);
                    }
                    break;

                case type.CallHangUp:
                    CallPacket hangUp = new CallPacket(packet);
                    Console.WriteLine("Need to disconnect now.");
                    CancelledCallEvent(ref hangUp);
                    break;

                case type.LinkRequest:
                    LinkPacket linkRequest = new LinkPacket(packet);                  

                    Task.Run(() =>
                    {
                        Socket FileSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        FileSocket.Bind(new IPEndPoint(IPAddress.Parse(linkRequest.destClient.ip), linkRequest.port));
                        FileSocket.Listen(1);
                        Socket ClientFileSocket = FileSocket.Accept();

                        const int buffer_length = 1024 * 5000; //5000kb
                        bool ReceivedAllData = false;
                        byte[] FileRecvBuffer = new byte[buffer_length];
                        byte[] FileDataBuffer = new byte[buffer_length];
                        int DataIndex = 0;
                        FilePacket FileDataPacket = null;
                        while (!ReceivedAllData)
                        {
                            int ReceivedLength = ClientFileSocket.Receive(FileRecvBuffer);
                            if (ReceivedLength > 0)
                            {
                                Array.Copy(FileRecvBuffer, 0, FileDataBuffer, DataIndex, ReceivedLength);
                                DataIndex += ReceivedLength;
                                FileRecvBuffer = new byte[buffer_length];
                            }
                            else
                            {
                                byte[] FileData = new byte[DataIndex];
                                Array.Copy(FileDataBuffer, FileData, DataIndex);
                                FileDataPacket = new FilePacket(FileData);
                                string SavedFilesFolder = @"..\..\SavedFiles\";

                                using (FileStream fs = new FileStream((SavedFilesFolder + Path.GetFileName(FileDataPacket.Filename)), FileMode.Append))
                                {
                                    fs.Write(FileDataPacket.FileContents, 0, FileDataPacket.FileContents.Length);
                                    Console.WriteLine("Received {0} out of {1}", fs.Length, FileDataPacket.TotalFileLength);
                                };
                                ReceivedAllData = true;
                                FileReceivedEvent(Path.GetFileName(FileDataPacket.Filename), FileDataPacket.destClient);
                            }
                        }
                        FileSocket.Close();
                    });

                    break;

                default:
                    Console.WriteLine(Encoding.UTF8.GetString(packet));
                    break;

            }
            return "OK";
        }
    }
}
