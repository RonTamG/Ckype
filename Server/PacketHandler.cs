using Ckype.Core.Networking;

namespace Server
{
    /// <summary>
    /// The servers packet handler, each packet goes through here is handles according to it's type
    /// </summary>
    public static class PacketHandler
    {
        /// <summary>
        /// Handle a received packet
        /// </summary>
        /// <param name="packet">The packet to handle</param>
        /// <param name="Server">The server who received the packet</param>
        /// <param name="Client">The client who sent the packet</param>
        public static void Handle(IPacketStructure packet, ServerSocket Server, Client Client)
        {

            switch (packet.PacketType)
            {
                // The Packet received when a client first connects to the server
                case PacketType.Connection:
                    AddNewConnection(packet, Server, Client);
                    break;

                case PacketType.Disconnection:
                    {

                    }; break;
            }
        }


        private static void AddNewConnection(IPacketStructure packet, ServerSocket Server, Client Client)
        {
            // Get packet as correct type
            var Packet = (packet as ConnectionPacket);

            Server.AddPerson(Packet.Sender, Client);

            // Send back to client
            Server.SendListOfPeople(Packet.Sender);
        }

        private static void DisconnnectClient(IPacketStructure packet, ServerSocket Server, Client Client)
        {   // Get packet as correct type
            var Packet = (packet as DisconnectionPacket);

            // TODO: Disconection
        }
    }
}