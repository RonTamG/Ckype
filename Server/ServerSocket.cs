using Ckype.Core;
using Ckype.Core.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace Server
{
    /// <summary>
    /// The main server that connects all clients
    /// </summary>
    public class ServerSocket
    {
        /// <summary>
        /// The list of people that are connected to the server
        /// </summary>
        private Dictionary<Person, Client> _ConnectedPeople { get; set; }

        /// <summary>
        /// The socket that the server communicates through
        /// </summary>
        private Socket _Socket { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ServerSocket()
        {
            _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// Listen for new clients
        /// </summary>
        /// <param name="backlog">The maximum number of clients that can wait to be accepted</param>
        public void Listen(int backlog)
        {
            _Socket.Listen(1);
        }

        /// <summary>
        /// Start accepting new clients
        /// </summary>
        public void Accept()
        {
            Logger.LogMessage($"Server started on port: {(_Socket.LocalEndPoint as IPEndPoint).Port}");
            _Socket.BeginAccept(AcceptedCallback, null);
        }


        /// <summary>
        /// When a client is being accepted, this is what is being run
        /// </summary>
        /// <param name="ar"></param>
        private void AcceptedCallback(IAsyncResult ar)
        {
            Socket clientSocket;
            try
            {
                clientSocket = _Socket.EndAccept(ar);
            }
            catch (ObjectDisposedException)
            {
                return;
            }

            var NewClient = new Client(clientSocket);
            // Continue accepting clients
            Accept();
            // Begin receiving data from the newly connected client
            HandleClient(NewClient);
        }


        /// <summary>
        /// Handles incoming packets from the client
        /// </summary>
        /// <param name="Client">The handled client</param>
        private void HandleClient(Client Client)
        {
            var Formatter = new BinaryFormatter();

            while (Client.CanReceive)
            {
                var packet = (IPacketStructure)Formatter.Deserialize(Client.Stream);
                PacketHandler.Handle(packet, this, Client);
            }
        }



        #region Public methods

        /// <summary>
        /// Send list of currently connected list of people to one person
        /// </summary>
        /// <param name="Receiver">The person receiving the list of people</param>
        public void SendListOfPeople(Person Receiver)
        {
            SendPacket(new PersonListPacket(Receiver, GetConnectedPeopleList()));
        }
        /// <summary>
        /// Send the list of connected people to everyone who is connected
        /// </summary>
        public void SendPeopleListUpdate()
        {
            var PeopleList = GetConnectedPeopleList();
            foreach (var person in PeopleList)
            {
                SendPacket(new PersonListPacket(person, PeopleList));
            }
        }
        /// <summary>
        /// Add a person to the dictionary of connected people
        /// </summary>
        /// <param name="person">The person to add</param>
        /// <param name="client">Their client that is connected to the server</param>
        public void AddPerson(Person person, Client client)
        {
            _ConnectedPeople.Add(person, client);
        }

        /// <summary>
        /// Send a serialized packet to it's destination
        /// </summary>
        /// <param name="packet">The packet we want to send</param>
        public void SendPacket(IPacketStructure packet)
        {
            var Client = _ConnectedPeople[packet.Receiver];
            var Formatter = new BinaryFormatter();

            Formatter.Serialize(Client.Stream, packet);
        }

        /// <summary>
        /// Get the list of currently connected people
        /// </summary>
        /// <returns>The currently connected people as a list</returns>
        public List<Person> GetConnectedPeopleList()
        {
            return _ConnectedPeople.Keys.ToList();
        }

        public void RemovePerson(Person person)
        {
            // TODO: Add remove person logic
        }

        #endregion
    }
}
