using PacketLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
                bufferSize = clientSocket.EndReceive(ar);
            }
            catch (SocketException)
            {
                Console.WriteLine("Client forcefully disconnected");
                clientSocket.Close();
                connected.Remove(FindPersonBySocket(clientSocket));
                this.ConnectedSockets();
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

        public void ConnectedSockets()
        {
            for (int i =0;i<this.connected.Count;i++)
            {
                for (int j =0; j<this.connected.Count;j++)
                {
                    if (!(connected[i] == connected[j]))
                        this.connected[j].ownSocket.Send(connected[i].Data);
                }
            }
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
            foreach (Person person in connected)
            {
//                person.ownSocket.Send();
                person.ownSocket.Shutdown(SocketShutdown.Both);
                person.ownSocket.Close();
            }

            _socket.Close();
        }
    }

}
