using System.Net.Sockets;

namespace Server
{
    /// <summary>
    /// A client that communicates with the server
    /// </summary>
    public class Client
    {
        #region Public properties

        /// <summary>
        /// Dictates if this client can receive data
        /// </summary>
        public bool CanReceive { get; set; }
        /// <summary>
        /// This clients network stream, through it we send data
        /// </summary>
        public NetworkStream Stream { get; private set; }
        /// <summary>
        /// The socket this client communicates with the server through
        /// </summary>
        public Socket MySocket { get; set; }

        #endregion
        #region Constructor
        /// <summary>
        /// Create a client object from a socket in order to 
        /// access the sockets network stream
        /// </summary>
        /// <param name="socket">The socket the client will use</param>
        public Client(Socket socket)
        {
            Stream = new NetworkStream(socket);
            MySocket = socket;
            CanReceive = true;
        }

        #endregion
        #region Public methods

        /// <summary>
        /// Close the current socket and free resources
        /// </summary>
        public void Close()
        {
            CanReceive = false;
            Stream.Dispose();
            MySocket.Shutdown(SocketShutdown.Both);
            MySocket.Dispose();
            MySocket.Close();
        }

        #endregion
    }
}
