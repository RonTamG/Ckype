using PacketLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientSocket
    {
        public Socket serverSocket;
        public Socket _socket { get; private set; }
        private byte[] _buffer;
        public string nickname { get; set; }
        public List<Person> Friends = new List<Person>();

        public ClientSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(string ipAddress, int port)
        {
            _socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ipAddress), port), ConnectCallback, null);
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            if (_socket.Connected)
            {
                Console.WriteLine("Connected to the server!");
                serverSocket = _socket;
                _buffer = new byte[1024];
                _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceivedCallback, null);

                #region Initial Packet

                Person packet = new Person(_socket, nickname);
                _socket.Send(packet.Data);


                #endregion

            }
            else Console.WriteLine("Could not connect");
        }
        
        private void ReceivedCallback(IAsyncResult ar)
        {
            int bufferLength;
            try
            {
                bufferLength = _socket.EndReceive(ar);
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Server disconnected");
                serverSocket.Close();
                return;
            }          
            byte[] packet = new byte[bufferLength];
            Array.Copy(_buffer, packet, packet.Length);

            // Handle Packet
            string statusUpdate;
            statusUpdate = PacketHandler.Handle(this, packet, serverSocket);

            if (_socket.Connected)
                _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceivedCallback, null);
            else return;
        }
        
        public void Send(byte[] data)
        {
            _socket.Send(data);
        }

        public void SendFile(string filename, Person destClient)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            while(fs.Position != fs.Length)
            {
                ushort packetLength;
                ushort packetStart = (ushort)(filename.Length + 12 + destClient.Data.Length);
                if (fs.Length - fs.Position < 1024 - packetStart)
                    packetLength = (ushort)(packetStart + (fs.Length - fs.Position));
                else
                    packetLength = 1024;
                FilePacket packet = new FilePacket(filename, packetLength, packetStart, destClient);
                fs.Read(packet.Data, packetStart, packetLength - packetStart);
                _socket.Send(packet.Data);
            }
        }

        public Person FindFriendByIPandPort(string ip, int port)
        {
            for (int i = 0; i < Friends.Count; i++)
                if (Friends[i].port == port && Friends[i].ip == ip)
                    return Friends[i];
            return null;
        }

        /// <summary>
        /// Close socket
        /// </summary>
        public void Close()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }
    }
}
