namespace Ckype.Core.Networking
{
    /// <summary>
    /// A request to disconnect from the server
    /// </summary>
    public class DisconnectionPacket : IPacketStructure
    {
        /// <summary>
        /// The client sending the request to disconnect
        /// </summary>
        public Person Sender { get; set; }
        /// <summary>
        /// The type of packet, disconnection packet
        /// </summary>
        public PacketType PacketType { get; } = PacketType.Disconnection;
        /// <summary>
        /// Null because the server is the receiver
        /// </summary>
        public Person Receiver { get; set; } = null;

        /// <summary>
        /// Send a request to disconnect from the server
        /// </summary>
        /// <param name="sender">The client wanting to disconnect</param>
        public DisconnectionPacket(Person sender)
        {
            Sender = sender;
        }
    }
}
