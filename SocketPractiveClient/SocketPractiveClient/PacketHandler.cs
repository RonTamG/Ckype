using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PacketLibrary;
using System.IO;

namespace Client
{
    public delegate void FriendEvent(Person person);
    public delegate void FriendsEvent(List<Person> people);
    public delegate void MessageEvent(Person person, string message);
    public delegate void CallingEvent(ref CallPacket packet);

    public static class PacketHandler
    {
        public static event FriendEvent FriendAddedEvent;
        public static event FriendEvent FriendRemovedEvent;
        public static event FriendsEvent FriendsReceivedEvent;
        public static event MessageEvent FriendMessageReceivedEvent;
        public static event MessageEvent FileReceivedEvent;
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
                case type.PersonConnected: // might need to send the server a message when we received the info.
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
                        Environment.Exit(0);
                    }
                    else
                    {
                        FriendMessageReceivedEvent(msg.destClient, msg.Text);
                        Console.WriteLine(msg.destClient + " Sent: " + msg.Text);
                    }
                    break;

                case type.File:
                    FilePacket newFile = new FilePacket(packet);
                    Console.WriteLine("Received a new file '{0}' from: {1}", newFile.Filename, newFile.destClient);
                    using (FileStream fs = new FileStream(Path.GetFileName(newFile.Filename), FileMode.Append))
                    {
                        fs.Write(newFile.FileContents, 0, newFile.FileContents.Length);
                    }
                    
                    Console.WriteLine("Saved!");
                    break;

                case type.CallRequest:
                    CallPacket callP = new CallPacket(packet);
                    CallingEvent(ref callP); // event to get input from user
                    Console.WriteLine("Received a call request from: {0}", callP.destClient);
                    Console.WriteLine("Declining request"); // here need to check if the receiver wants to accept or decline. rn declines automatically.
                    // if (receiver == accepted)s
                    //     callP.SetAcceptedCall(); // No need to add the else
                    break;

                case type.CallResponse:
                    CallPacket checkCallP = new CallPacket(packet);
                    if (checkCallP.acceptedCall)
                    {
                        Console.WriteLine("Your friend: {0} has accepted the call!", checkCallP.destClient);
                        AcceptedCallEvent(ref checkCallP);
                        //  call friend do the calling thing
                    }
                    else
                    {
                        Console.WriteLine("Your friend: {0} has declined the call.", checkCallP.destClient);
                        DeclinedCallEvent(ref checkCallP);
                    }
                    break;

                case type.CallHangUp:
                    CallPacket hangUp = new CallPacket(packet);
                    Console.WriteLine("Need to disconnect now.");
                    CancelledCallEvent(ref hangUp);
                    break;

                default:
                    Console.WriteLine(Encoding.UTF8.GetString(packet));
                    break;

            }
            return "OK";
        }
    }
}
