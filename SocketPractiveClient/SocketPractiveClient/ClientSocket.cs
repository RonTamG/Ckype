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
        private Socket _socket;
        private byte[] _buffer;
        public string nickname;

        public ClientSocket(string nickname)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.nickname = nickname;
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
                _buffer = new byte[1024];
                _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceivedCallback, null);
/*
                #region Initial Packet

                byte[] packet = new byte[4];
                byte[] packetLength = BitConverter.GetBytes((ushort) packet.Length);
                Random rnd = new Random();
                byte[] packetType = BitConverter.GetBytes((ushort) 1000);
                Array.Copy(packetLength, packet, 2);
                Array.Copy(packetType, 0, packet, 2, 2);
                _socket.Send(packet);


                #endregion
*/
                IPPacket packet2 = new IPPacket(_socket, nickname);
                _socket.Send(packet2.Data);

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

        public void SendFile(string filename)
        {
            FilePacket myfile = new FilePacket(filename);
            while(myfile.status)
            {
                _socket.Send(myfile.Data);
                myfile.ReadFileChunk();
            }
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
