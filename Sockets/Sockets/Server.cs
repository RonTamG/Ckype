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
        public  List<Socket> clientSockets = new List<Socket>(); 
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

            clientSockets.Add(clientSocket);
            _buffer = new byte[1024]; // 1 kb LENGTH
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
                clientSockets.Remove(clientSocket);
                return;
            }

            byte[] packet = new byte[bufferSize];
            Array.Copy(_buffer, packet, bufferSize);

            // Handle the packet.
             string statusUpdate;
             statusUpdate = PacketHandler.Handle(this, packet, clientSocket);

            
//             _buffer = new byte[1024]; // kb LENGTH ruined multi client
             if (!(statusUpdate == "closed client"))
                 clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceivedCallback, clientSocket); //0 is the beginning
             else return;
/*
            string text = Encoding.ASCII.GetString(packet);
            text = text.Substring(4);
            Console.WriteLine("Received Text: " + text);

            if (text.ToLower() == "get time") // Client requested time
            {
                Console.WriteLine("Text is a get time request");
                byte[] data = Encoding.ASCII.GetBytes(DateTime.Now.ToLongTimeString());
                clientSocket.Send(data);
                Console.WriteLine("Time sent to client");
            }
            else if (text.ToLower() == "exit") // Client wants to exit gracefully
            {
                // Always Shutdown before closing
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
                clientSockets.Remove(clientSocket);
                Console.WriteLine("Client disconnected");
                return;
            }
            else
            {
                Console.WriteLine("Text is an invalid request");
                byte[] data = Encoding.ASCII.GetBytes("Invalid request");
                clientSocket.Send(data);
                Console.WriteLine("Warning Sent");
            }

            clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceivedCallback, clientSocket);
              */                  
            /*
            ushort packetLength = BitConverter.ToUInt16(packet, 0);
            ushort packetType = BitConverter.ToUInt16(packet, 2);

            Console.WriteLine("Received packet: Length: {0} | Type: {1}", packetLength, packetType);

            switch (packetType)
            {
                case 2000:
                    MessagePacket msg = new MessagePacket(packet);
                    Console.WriteLine(msg.Text);

                    string response = string.Empty;

                    if (msg.Text.ToLower() == "exit")
                    {
                        // Always Shutdown before closing
                        clientSocket.Shutdown(SocketShutdown.Both);
                        clientSocket.Close();
                        this.clientSockets.Remove(clientSocket);
                        Console.WriteLine("Client disconnected");
                        statusUpdate = "closed client";
                        //return "closed client";
                    }
                    if (msg.Text.ToLower() != "get time")
                    {
                        response = "Invalid Request";
                    }
                    else
                    {
                        response = DateTime.Now.ToLongTimeString();
                    }

                    MessagePacket ToSend = new MessagePacket(response);
                    clientSocket.Send(ToSend.Data);
                    Console.WriteLine("Sent" + response);
                    break;
            }
            //_buffer = new byte[1024]; // kb LENGTH
            if (!(statusUpdate == "closed client"))
                clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceivedCallback, clientSocket); //0 is the beginning
            else return;*/
        }

        private static void SendCallBack(IAsyncResult ar)
        {
            Socket clientSocket = ar.AsyncState as Socket;
            clientSocket.EndSend(ar);
        }

        /// <summary>
        /// Close all connected client (we do not need to shutdown the server socket as its connections
        /// are already closed with the clients).
        /// </summary>
        public void CloseAllSockets()
        {
            foreach (Socket clientSocket in clientSockets)
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }

            _socket.Close();
        }
    }

}
