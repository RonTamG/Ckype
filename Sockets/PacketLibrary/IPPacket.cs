using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PacketLibrary
{
    public class IPPacket : PacketStructure
    {
        private Socket _socket;
       public IPPacket(Socket s, string name) //string name, maybe add name too.
            : base((ushort)(18 + name.Length + 20), 1000) // +=name.Length
        {
            string _ip = ((IPEndPoint)s.LocalEndPoint).Address.ToString();
            int _port = ((IPEndPoint)s.LocalEndPoint).Port;
            WriteUShort((ushort)_port, 4);
            WriteUShort((ushort)_ip.Length, 6);
            WriteString(_ip, 8);
            WriteUShort((ushort)name.Length, 8 + _ip.Length);
            WriteString(name, 10 + _ip.Length);
            _socket = s;
        }

        public IPPacket(byte[] packet)
            : base(packet)
        {

        }

        public Socket ownSocket
        {
            get {return _socket; }
            set
            {
                _socket = value;
            }
        }

        public string ip
        {
            get { return ReadString(8, ReadUShort(6)); }
        }

        public int port
        {
            get { return ReadUShort(4); }
        }

        public string name
        {
            get { return ReadString(10 + ReadUShort(6), ReadUShort(8 + ReadUShort(6))); }
        }

    }
}
