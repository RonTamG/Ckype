using System;

namespace Ckype.Core.Networking
{
    /// <summary>
    /// Packet used when someone wants to start a call
    /// </summary>
    [Serializable]
    public class CallPacket : IPacketStructure
    {
        /// <summary>
        /// The person who sent the request
        /// </summary>
        public Person Sender { get; set; }
        /// <summary>
        /// The person who receives the request
        /// </summary>
        public Person Receiver { get; set; }
        /// <summary>
        /// Dictates if the destination client accepted the call
        /// </summary>
        public bool AcceptedCall { get; private set; }
        /// <summary>
        /// The type of this packet
        /// </summary>
        public PacketType PacketType { get; private set; }

        /// <summary>
        /// Create a new calling request
        /// </summary>
        /// <param name="sender">The person who sends the request</param>
        /// <param name="destination">The person who receives the request</param>
        public CallPacket(Person sender, Person destination)
        {
            PacketType = PacketType.CallRequest;
            AcceptedCall = false;

            Sender = sender;
            Receiver = destination;
        }

        public CallPacket(Person sender, Person destination, PacketType type)
        {

        }

        /// <summary>
        /// Set a positive response to the call request
        /// </summary>
        public void AcceptCall()
        {
            AcceptedCall = true;
            PacketType = PacketType.CallResponse;
            SwitchReceiverAndSender();
        }
        /// <summary>
        /// Set a negative response to the call request
        /// </summary>
        public void DeclineCall()
        {
            AcceptedCall = false;
            PacketType = PacketType.CallResponse;
            SwitchReceiverAndSender();
        }

        /// <summary>
        /// Send a request to end the current call
        /// </summary>
        public void SetHangUp()
        {
            PacketType = PacketType.CallHangUp;
        }


        #region Helpers

        /// <summary>
        /// Switches the receiver and the sender for the response
        /// </summary>
        public void SwitchReceiverAndSender()
        {
            Person Temp = Sender;
            Sender = Receiver;
            Receiver = Temp;
        }

        #endregion
    }
}
