﻿using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace PacketLibrary
{
    public class Person : PacketStructure
    {
        private Socket _socket;
       public Person(Socket Socket, string name)
            : base((ushort)(18 + name.Length + 20), (ushort)type.PersonConnected)
        {
            string _ip = ((IPEndPoint)Socket.LocalEndPoint).Address.ToString();
            int _port = ((IPEndPoint)Socket.LocalEndPoint).Port;
            WriteUShort((ushort)_port, 4);
            WriteUShort((ushort)_ip.Length, 6);
            WriteString(_ip, 8);
            WriteUShort((ushort)name.Length, 8 + _ip.Length);
            WriteString(name, 10 + _ip.Length);
            _socket = Socket;
        }

        public Person(string name, string ip, int port)
     : base((ushort)(18 + name.Length + 20), (ushort)type.PersonConnected)
        {
            string _ip = ip;
            int _port = port;
            WriteUShort((ushort)_port, 4);
            WriteUShort((ushort)_ip.Length, 6);
            WriteString(_ip, 8);
            WriteUShort((ushort)name.Length, 8 + _ip.Length);
            WriteString(name, 10 + _ip.Length);
        }

        public Person(byte[] packet)
            : base(packet)
        {

        }

        public Socket OwnSocket
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

        public void SetDisconnectedType()
        {
            WriteUShort((ushort)type.PersonDisconnected, 2);
        }

        public override string ToString()
        {
            return "Name: " + name + " Address: " + ip + ":" + port;
        }

        public static Person FindPersonByIPandPort(Person p, List<Person> PersonList)
        {
            for (int i = 0; i < PersonList.Count; i++)
                if (PersonList[i].port == p.port && PersonList[i].ip == p.ip)
                    return PersonList[i];
            return null;
        }

        public static bool operator== (Person first, Person second)
        {
            return (first.ip == second.ip && first.port == second.port);
        }

        public static bool operator !=(Person first, Person second)
        {
            return !(first.ip == second.ip && first.port == second.port);
        }
    }
}
