namespace Ckype.Core.Networking
{
    /// <summary>
    /// A packet for sending a basic text message between 2 clients
    /// </summary>
    public class MessagePacket : IPacketStructure
    {
        /// <summary>
        /// The person sending the message
        /// </summary>
        public Person Sender { get; set; }
        /// <summary>
        /// The person receiving the message
        /// </summary>
        public Person Receiver { get; set; }
        /// <summary>
        /// The message itself
        /// </summary>
        public string MessageText { get; set; }
        /// <summary>
        /// The type of this packet, Message
        /// </summary>
        public PacketType PacketType { get; set; } = PacketType.Message;

        /// <summary>
        /// Send a message to another client
        /// </summary>
        /// <param name="sender">The client sending the packet</param>
        /// <param name="receiver">The client receiving the packet</param>
        /// <param name="message">The message being sent</param>
        public MessagePacket(Person sender, Person receiver, string message)
        {
            Sender = sender;
            Receiver = receiver;
            MessageText = message;
        }
    }
}