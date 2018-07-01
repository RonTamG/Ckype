namespace Ckype.Core.Networking
{
    /// <summary>
    /// Packet sent when connecting to server
    /// </summary>
    public class ConnectionPacket : IPacketStructure
    {
        #region Public properties

        /// <summary>
        /// The person sending the connection request
        /// </summary>
        public Person Sender { get; set; }
        /// <summary>
        /// Null because the server is the receiver
        /// </summary>
        public Person Receiver { get; set; } = null;
        /// <summary>
        /// The type of the packet, Connection
        /// </summary>
        public PacketType PacketType { get; private set; } = PacketType.Connection;

        #endregion
        #region Constructor

        /// <summary>
        /// Send a connection request to the server
        /// </summary>
        /// <param name="ConnectingPerson">The client wanting to connect</param>
        public ConnectionPacket(Person ConnectingPerson)
        {
            Sender = ConnectingPerson;
        }

        #endregion
    }
}
