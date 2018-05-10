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

    public static class PacketHandler
    {
        public static event FriendEvent FriendAddedEvent;
        public static event FriendEvent FriendRemovedEvent;

        public static string Handle(ClientSocket clientSocket, byte[] packet, Socket serverSocket)
        {
            ushort packetLength = BitConverter.ToUInt16(packet, 0);
            ushort packetType = BitConverter.ToUInt16(packet, 2);

            Console.WriteLine("Received packet: Length: {0} | Type: {1}", packetLength, packetType);
            switch (packetType)
            {
                case 1000:
                    Person person = new Person(packet);
                    clientSocket.Friends.Add(person);
                    FriendAddedEvent?.Invoke(person);
                    Console.WriteLine("name: " + person.name + " Connected from: " + person.ip + ":" + person.port);
                    break;
                case 1500:
                    Person disconnected = new Person(packet);
                    clientSocket.Friends.Remove(Person.FindPersonByIPandPort(disconnected, clientSocket.Friends));
                    FriendRemovedEvent?.Invoke(disconnected);
                    Console.WriteLine(disconnected + " Disconnected from the server");
                    break;
                case 2000:
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
                        Console.WriteLine(msg.destClient + " Sent: " + msg.Text);
                    }
                    break;
                case 3000:
                    FilePacket newFile = new FilePacket(packet);
                    Console.WriteLine("Received a new file '{0}' from: {1}", newFile.Filename, newFile.destClient);
                    using (FileStream fs = new FileStream(Path.GetFileName(newFile.Filename), FileMode.Append))
                    {
                        fs.Write(newFile.FileContents, 0, newFile.FileContents.Length);
                    }
                    Console.WriteLine("Saved!");
                    break;
                case 4000:
                    CallPacket callP = new CallPacket(packet);
                    Console.WriteLine("Received a call request from: {0}", callP.destClient);
                    Console.WriteLine("Declining request");
                    callP.toCheckType();
                    serverSocket.Send(callP.Data);
                    break;
                case 4500:
                    CallPacket checkCallP = new CallPacket(packet);
                    if(checkCallP.acceptedCall)
                        Console.WriteLine("Your friend: {0} has accepted the call!", checkCallP.destClient);
                    else
                        Console.WriteLine("Your friend: {0} has declined the call.", checkCallP.destClient);
                    break;
                default:
                    Console.WriteLine(Encoding.UTF8.GetString(packet));
                    break;
            }
            return "OK";
        }
    }
}
