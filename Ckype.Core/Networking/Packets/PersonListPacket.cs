using System.Collections.Generic;

namespace Ckype.Core.Networking
{
    /// <summary>
    /// A refresh of a client's list of people
    /// </summary>
    public class PersonListPacket : IPacketStructure
    {
        #region Public properties
        /// <summary>
        /// The list of currently connected people
        /// </summary>
        public List<Person> People { get; set; }
        /// <summary>
        /// The person who sent the packet, null if the server is the sender
        /// </summary>
        public Person Sender { get; set; }
        /// <summary>
        /// The receiving person for this packet, null if we send to the server
        /// </summary>
        public Person Receiver { get; set; }
        /// <summary>
        /// The type of this packet. PersonList
        /// </summary>
        public PacketType PacketType { get; set; } = PacketType.PersonList;
        #endregion
        #region Constructors
        /// <summary>
        /// A constructor for a client that is requesting a refresh
        /// </summary>
        /// <param name="sender">The person sending the request</param>
        public PersonListPacket(Person sender)
        {
            Sender = sender;
            Receiver = null; // we are only sending to the server
        }

        /// <summary>
        /// A constructor for a server responding to a refresh request
        /// </summary>
        /// <param name="receiver">The person this packet is being sent to, the requestor</param>
        /// <param name="people">The list of people currently connected to the server</param>
        public PersonListPacket(Person receiver, List<Person> people)
        {
            People = people;
            Receiver = receiver;
            Sender = null; // the server is the sender
        }
        #endregion
    }
}
