using System.IO;
using System.Net.Sockets;

namespace Ckype.Core.Helpers
{
    public static class SocketHelper
    {
        /// <summary>
        /// A method for sending a file through sockets
        /// </summary>
        /// <param name="Receiver">The socket receiving the file</param>
        /// <param name="Filepath">The path to the file we want to send</param>
        public static void SendFile(this Socket Receiver, string Filepath)
        {
            using (var fileStream = File.OpenRead(Filepath))
            {
                using (var ReceiverStream = new NetworkStream(Receiver))
                {
                    fileStream.CopyTo(ReceiverStream);
                    Logger.LogMessage($"Finished sending file {Filepath}");
                }

                Receiver.Dispose();
                Receiver.Close();
            }
        }

        /// <summary>
        /// A method for receiving a sent file from a another socket
        /// </summary>
        /// <param name="Sender">The socket who sent a file</param>
        /// <param name="Filepath">The name of file currently being sent</param>
        public static void ReceiveFile(this Socket Sender, string Filepath)
        {
            using (var SenderStream = new NetworkStream(Sender))
            {
                try
                { 
                    using (var fileStream = File.Open(Filepath, FileMode.Create))
                    {
                        SenderStream.CopyTo(fileStream);
                    }
                }
                catch (SocketException)
                {
                    Logger.LogMessage("An error receiving a file has occured");
                    throw;
                }
            }
        }
    }
}
