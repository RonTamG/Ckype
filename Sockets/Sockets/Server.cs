using Ckype.Core;
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
        private byte[] _buffer = new byte[1024]; // 1 kb

        #region Constructor

        /// <summary>
        /// Default constructor for server socket
        /// </summary>
        public ServerSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        #endregion
        #region Public Methods

        /// <summary>
        /// Binds the socket as a server on the current computer
        /// </summary>
        /// <param name="port">The port to listen on</param>
        public void Bind(int port)
        {
            _socket.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        /// <summary>
        /// Listen for new clients
        /// </summary>
        /// <param name="backlog">The maximum number of clients that can wait to be accepted</param>
        public void Listen(int backlog)
        {
            _socket.Listen(1);
        }

        /// <summary>
        /// Start accepting new clients
        /// </summary>
        public void Accept()
        {
            Logger.LogMessage($"Server started on port: {(_socket.LocalEndPoint as IPEndPoint).Port}");
            _socket.BeginAccept(AcceptedCallback, null);
        }

        /// <summary>
        /// Send to all connected clients the person whose connection status changed
        /// </summary>
        /// <param name="newPerson">The person that either connected or disconnected</param>
        public void NewPersonOnlineOffline(Person newPerson)
        {
            for (int i = 0; i < this.connected.Count; i++)
                if (this.connected[i].OwnSocket.Connected)
                    this.connected[i].OwnSocket.Send(newPerson.Data);
        }

        public void SendConnections(Person SendTo)
        {

            //List without the person we will be sending the connections to.
            List<Person> Connections = new List<Person>();
            for (int i = 0; i < connected.Count; i++)
                if (!(connected[i].port == SendTo.port && connected[i].ip == SendTo.ip))
                    Connections.Add(connected[i]);

            ConnectionsPacket ConPacket = new ConnectionsPacket((ushort)(ConnectionsPacket.PersonListByteLength(Connections) + 6), Connections);
            SendTo.OwnSocket.Send(ConPacket.Data);
        }

        public Person FindPersonBySocket(Socket clientS)
        {
            for (int i = 0; i < connected.Count; i++)
            {
                if (connected[i].OwnSocket == clientS)
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
            for (int i = 0; i < connected.Count; i++)
            {
                Person person = connected[i];
                MessagePacket packet = new MessagePacket("SecretDisconnectedServerMessage");
                person.OwnSocket.Send(packet.Data);
                person.OwnSocket.Shutdown(SocketShutdown.Both);
                person.OwnSocket.Close();
            }

            _socket.Close();
        }

        #endregion
        #region Private Methods

        /// <summary>
        /// When a client is being accepted, this is what is being run
        /// </summary>
        /// <param name="ar"></param>
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

            // Continue accepting clients
            Accept();
            // Begin receiving data from the newly connected client
            clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceivedCallback, clientSocket);
        }

        /// <summary>
        /// When the server receives data from a client this method is called.
        /// </summary>
        /// <param name="ar"></param>
        private void ReceivedCallback(IAsyncResult ar)
        {
            // get correct parameters
            Socket clientSocket = ar.AsyncState as Socket;
     
            int bufferSize;
            try
            {
                if (clientSocket.Connected)
                    bufferSize = clientSocket.EndReceive(ar);
                else
                    throw new SocketException();
            }
            catch (SocketException) // only happens when a client disconnects without the proper procedure
            {
                Logger.LogMessage("Client forcefully disconnected");
                // close disconnected client's socket
                clientSocket.Close();
                // remove them from the connected people list
                Person disconnected = FindPersonBySocket(clientSocket);
                disconnected.SetDisconnectedType();
                connected.Remove(disconnected);
                // send the change to all connected clients
                NewPersonOnlineOffline(disconnected);
                return;
            }

            // In case we received data successfully...
            byte[] packet = new byte[bufferSize];
            Array.Copy(_buffer, packet, bufferSize);

            // Handle the packet.
            string statusUpdate;
            statusUpdate = PacketHandler.Handle(this, packet, clientSocket);

            // Check if this client disconnected now, if not...
            if (!(statusUpdate == "Closed Client"))
            {
                // continue receiving data
                clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceivedCallback, clientSocket);
            }
            else return;
        }

        private static void SendCallBack(IAsyncResult ar)
        {
            Socket clientSocket = ar.AsyncState as Socket;
            clientSocket.EndSend(ar);
        }

        #endregion
    }

}
