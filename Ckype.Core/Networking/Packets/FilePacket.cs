namespace Ckype.Core.Networking
{
    /// <summary>
    /// The packet a client sends to another
    /// in order to inform them they are sending a file
    /// </summary>
    public class FilePacket : IPacketStructure
    {
        #region Public properties

        /// <summary>
        /// The client who is sending the file
        /// </summary>
        public Person Sender { get; set; }
        /// <summary>
        /// The client who receives the file
        /// </summary>
        public Person Receiver { get; set; }
        /// <summary>
        /// The port on which the file will be sent
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// The type of the packet
        /// </summary>
        public PacketType PacketType { get; set; } = PacketType.File;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new packet to inform of a file request
        /// </summary>
        /// <param name="sender">The client sending the file</param>
        /// <param name="receiver">The client receiving the file</param>
        /// <param name="port">The port on which the file will be sent</param>
        public FilePacket(Person sender, Person receiver, int port)
        {
            Sender = sender;
            Receiver = receiver;
            Port = port;
        }

        #endregion
    }
}
