using NAudio.Wave;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace AudioLibrary
{
    public class NetworkAudio
    {
        #region Variables

        #region Socket
        private UdpClient udpListener;
        private UdpClient udpSender;
        private IPEndPoint connectionEndPoint;
        #endregion
        #region Sound
        private BufferedWaveProvider waveProvider;
        private DirectSoundOut soundOut = null;
        private WaveInEvent SoundIn = null;
        #endregion
        #region User
        private bool connected = false;
        private bool muted = false;
        #endregion

        #endregion

        #region Constructor

        public NetworkAudio(IPEndPoint ConnectToEndPoint)
        {
            connectionEndPoint = ConnectToEndPoint;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Toggle if audio from this computer is sent
        /// </summary>
        public void ToggleSelfMute()
        {
            muted = !muted;
        }

        /// <summary>
        /// Disconnect from other client and dispose of all variables
        /// </summary>
        public void Disconnect()
        {
            if (connected)
            {
                soundOut.Dispose();
                soundOut = null;
                SoundIn.DataAvailable -= OnAudioCaptured;

                SoundIn.StopRecording();
                SoundIn.Dispose();
                SoundIn = null;

                udpListener?.Close();
                udpListener = null;
                udpSender?.Close();
                udpSender = null;
            }
            connected = false;
        }

        /// <summary>
        /// Connects to other client and starts transmitting audio
        /// </summary>
        public void Start()
        {
            udpSender = new UdpClient();
            SoundIn = new WaveInEvent();
            SoundIn.DataAvailable += OnAudioCaptured;
            SoundIn.StartRecording();
            udpListener = new UdpClient();
            udpListener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true); // test
            //
            var EndPointListen = new IPEndPoint(IPAddress.Any, connectionEndPoint.Port);
            udpListener.Client.Bind(EndPointListen);

            udpSender.Connect(connectionEndPoint);


            soundOut = new DirectSoundOut();
            waveProvider = new BufferedWaveProvider(SoundIn.WaveFormat);
            soundOut.Init(waveProvider);
            soundOut.Play();

            ThreadPool.QueueUserWorkItem(ListenerThread, EndPointListen);

            connected = true;
        } 
        #endregion

        #region Private Methods

        /// <summary>
        /// Function to run the listener on a different thread
        /// </summary>
        /// <param name="endPoint">The endpoint the listener is connected to [TYPE:IPEndPoint]</param>
        private void ListenerThread(object endPoint)
        {
            var endPointTemp = (IPEndPoint)endPoint;
            try
            {
                while (true)
                {
                    byte[] buffer = udpListener.Receive(ref endPointTemp);
                    waveProvider?.AddSamples(buffer, 0, buffer.Length);
                }
            }
            catch (SocketException)
            {
                // usually not a problem - just means we have disconnected

            }
        }

        private void OnAudioCaptured(object sender, WaveInEventArgs e)
        {
            if (!muted)
                udpSender.Send(e.Buffer, e.BytesRecorded);
        } 

        #endregion
    }
}
