using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Client
{
    /// <summary>
    /// The client used to connect to the server
    /// </summary>
    public class ClientSocket
    {
        /// <summary>
        /// The socket used for communication
        /// </summary>
        private Socket _Socket { get; set; }

        public ClientSocket()
        {
            _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// Connects to a client to the server
        /// </summary>
        /// <param name="ip">The ip of the server</param>
        /// <param name="port">The port the server is listening on</param>
        /// <returns>True, if managed to connect, else false</returns>
        public async Task<bool> ConnectAsync(string ip, int port)
        {
            bool parsed = IPAddress.TryParse(ip, out IPAddress IP);

            await Task.Run(() =>
            {
                if (parsed)
                {
                    _Socket.Connect(new IPEndPoint(IP, port));
                }
            });

            if (!parsed || !_Socket.Connected)
                return false;
            else
                return true;

        }
    }
}
