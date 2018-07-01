using Ckype.Core;
using PacketLibrary;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    public class ClientSocket
    {
        public Socket Socket { get; private set; }
        private byte[] _buffer;
        public string Nickname { get; set; }
        public List<Person> Friends = new List<Person>();
        public Person me;

        public ClientSocket()
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(string ipAddress, int port)
        {
            Socket.Connect(new IPEndPoint(IPAddress.Parse(ipAddress), port));
            if (Socket.Connected)
            {
                Logger.LogMessage("Connected to the server!");
                _buffer = new byte[1024];
                Socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceivedCallback, null);

                #region Initial Packet

                Person packet = new Person(Socket, Nickname);
                Socket.Send(packet.Data);

                #endregion

                IPEndPoint MyIpAddr = (IPEndPoint)Socket.LocalEndPoint;
                string MyAddress = MyIpAddr.Address.ToString();
                int MyPort = MyIpAddr.Port;
                me = new Person(Nickname, MyAddress, MyPort);
            }
            else Logger.LogMessage("Could not connect");
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            if (Socket.Connected)
            {
                Logger.LogMessage("Connected to the server!");
                _buffer = new byte[1024];
                Socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceivedCallback, null);

                #region Initial Packet

                Person packet = new Person(Socket, Nickname);
                Socket.Send(packet.Data);


                #endregion

            }
            else Logger.LogMessage("Could not connect");
        }

        private void ReceivedCallback(IAsyncResult ar)
        {
            int bufferLength;
            try
            {
                bufferLength = Socket.EndReceive(ar);
            }
            catch (ObjectDisposedException)
            {
                Logger.LogMessage("Server disconnected");
                Socket.Close();
                return;
            }
            if (bufferLength > 0)
            {
                byte[] packet = new byte[bufferLength];
                Array.Copy(_buffer, packet, packet.Length);

                // Handle Packet
                string statusUpdate;
                statusUpdate = PacketHandler.Handle(this, packet, Socket);

                if (Socket.Connected)
                    Socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceivedCallback, null);
                else return;
            }
        }

        public void Send(byte[] data)
        {
            Socket.Send(data);
        }

        public void RefreshRequest()
        {
            Friends.Clear();
            ConnectionsPacket packet = new ConnectionsPacket();
            Socket.Send(packet.Data);
        }

        public void CallPerson(Person person)
        {
            Logger.LogMessage($"You have chosen to call {person}");
            CallPacket callP = new CallPacket(person);
            Socket.Send(callP.Data);
        }

        public void EndCurrentCall(Person person)
        {
            Logger.LogMessage("You are hanging up now!");
            CallPacket callP = new CallPacket(person);
            callP.SetHangUp();
            Socket.Send(callP.Data);
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
        public void Close() // maybe should be made private
        {
            Socket.Shutdown(SocketShutdown.Both);
            Socket.Close();
        }

        public void Disconnect()
        {
            Person packet = new Person(this.Socket, Nickname);
            packet.SetDisconnectedType();
            this.Send(packet.Data);
            this.Close();
        }
    }
}
