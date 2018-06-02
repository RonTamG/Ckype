using PacketLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    // Socket checks
    public class ServerSocket
    {
        public List<Person> connected = new List<Person>();
        private Socket _socket;
        private byte[] _buffer = new byte[1024]; // 1 kb LENGTH

        public ServerSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        
        public void Bind(int port)
        {
            _socket.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        public void Listen(int backlog)
        {
            _socket.Listen(1);
        }

        public void Accept()
        {
            _socket.BeginAccept(AcceptedCallback, null);
        }

        private void AcceptedCallback(IAsyncResult ar)
        {
            Socket clientSocket;
            try
            {
                clientSocket = _socket.EndAccept(ar);
            }
            catch (ObjectDisposedException)
            {
                return;
            }

            Accept();
            clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceivedCallback, clientSocket); //0 is the begnning

        }

        private void ReceivedCallback(IAsyncResult ar)
        {
            Socket clientSocket = ar.AsyncState as Socket;
            int bufferSize;
            try
            {
                if (clientSocket.Connected)
                    bufferSize = clientSocket.EndReceive(ar);
                else
                    throw new SocketException();
            }
            catch (SocketException)
            {
                Console.WriteLine("Client forcefully disconnected");
                clientSocket.Close();
                Person disconnected = FindPersonBySocket(clientSocket);
                disconnected.SetDisconnectedType();
                NewPersonOnlineOffline(disconnected);
                connected.Remove(disconnected);               
                return;
            }

            byte[] packet = new byte[bufferSize];
            Array.Copy(_buffer, packet, bufferSize);

            // Handle the packet.
             string statusUpdate;
             statusUpdate = PacketHandler.Handle(this, packet, clientSocket);

            if (!(statusUpdate == "closed client"))
            {
                clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceivedCallback, clientSocket);
            }                                    //0 is the beginning
            else return;
        }

        private static void SendCallBack(IAsyncResult ar)
        {
            Socket clientSocket = ar.AsyncState as Socket;
            clientSocket.EndSend(ar);
        }

        public void NewPersonOnlineOffline(Person newPerson)
        {
            for (int i = 0; i < this.connected.Count; i++)
                if(this.connected[i].ownSocket.Connected)
                    this.connected[i].ownSocket.Send(newPerson.Data);
        }

        public void SendConnections(Person SendTo)
        {

            //List without the person we will be sending the connections to.
            List<Person> Connections = new List<Person>();
            for (int i = 0; i < connected.Count; i++)
                if (!(connected[i].port == SendTo.port && connected[i].ip == SendTo.ip))
                    Connections.Add(connected[i]);

            ConnectionsPacket ConPacket = new ConnectionsPacket((ushort)(ConnectionsPacket.PersonListByteLength(Connections) + 6), Connections);
            SendTo.ownSocket.Send(ConPacket.Data);
        }

        public void SendFile(string filename, Person destClient)
        {
            
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            while (fs.Position != fs.Length)
            {
                ushort packetLength;
                ushort packetStart = (ushort)(filename.Length + 16 + destClient.Data.Length);

                if (fs.Length - fs.Position < 1024 - packetStart)
                {
                    packetLength = (ushort)(packetStart + (fs.Length - fs.Position));
                }
                else
                {
                    packetLength = 1024;
                }

                FilePacket packet = new FilePacket(filename, packetLength, (uint)fs.Length, packetStart, destClient);
                fs.Read(packet.Data, packetStart, packetLength - packetStart);
                
                connected[0].ownSocket.Send(packet.Data);
            }
            fs.Close();
        }

        public Person FindPersonBySocket(Socket clientS)
        {
            for (int i = 0; i < connected.Count; i++)
            {
                if (connected[i].ownSocket == clientS)
                    return connected[i];
            }
            return null;
        }
        /// <summary>
        /// Close all connected client (we do not need to shutdown the server socket as its connections
        /// are already closed with the clients).
        /// </summary>
        public void CloseAllSockets()
        {
            for(int i =  0; i<connected.Count; i++)
            {
                Person person = connected[i];
                MessagePacket packet = new MessagePacket("SecretDisconnectedServerMessage");
                person.ownSocket.Send(packet.Data);
                person.ownSocket.Shutdown(SocketShutdown.Both);
                person.ownSocket.Close();
            }

            _socket.Close();
        }
    }

}
