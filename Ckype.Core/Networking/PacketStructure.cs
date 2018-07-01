namespace Ckype.Core.Networking
{
    /// <summary>
    /// Describes the type of the packet being sent
    /// </summary>
    public enum PacketType
    {
        Connection = 1,
        Disconnection = 2,
        PersonList = 3,
        Message = 4,
        File = 5,
        FileFinished = 6,
        CallRequest = 7,
        CallResponse = 8,
        CallHangUp = 9,
        ServerShutdown = 10
    };

    /// <summary>
    /// Something each packet must have
    /// </summary>
    public interface IPacketStructure
    {
        /// <summary>
        /// The type of the packet
        /// </summary>
        PacketType PacketType { get; }
        /// <summary>
        /// The client who sent the packet
        /// </summary>
        Person Sender { get; set; }
        /// <summary>
        /// The client who receives the packet
        /// </summary>
        Person Receiver { get; set; }
    }
}
