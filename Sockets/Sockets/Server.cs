using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketsPractice
{
    // Socket checks
    public class ServerSocket
    {
        private Socket _socket;
        private byte[] _buffer = new byte[1024]; //kb LENGTH

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
            Socket clientSocket = _socket.EndAccept(ar);
            if (clientSocket != null)
            {
                _buffer = new byte[1024]; // kb LENGTH
                Accept();
                clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceivedCallback, clientSocket); //0 is the begnning
            }
        }

        private void ReceivedCallback(IAsyncResult ar)
        {
            Socket clientSocket = ar.AsyncState as Socket;
            SocketError SE;
            int bufferSize = clientSocket.EndReceive(ar, out SE);
            //if (SE != SocketError.Success)
                
            byte[] packet = new byte[bufferSize];
            Array.Copy(_buffer, packet, packet.Length);

            // Handle the packet.
            PacketHandler.Handle(packet, clientSocket);

            _buffer = new byte[1024]; // kb LENGTH
            clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceivedCallback, clientSocket); //0 is the begnning
            
        }
    }

}
