using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using NAudio.Wave;


namespace AudioLibrary
{
    public static class AudioLibraryClass
    {
        private static WaveFileReader waveReader = null;
        private static DirectSoundOut soundOut = null;
        private static WaveInEvent SoundIn = null;
        public static void FilePlay(string filename)
        {
            waveReader = new WaveFileReader(filename);
            soundOut = new DirectSoundOut();
            soundOut.Init(new WaveChannel32(waveReader));
            soundOut.Play();
        }

        public static void PlayRecordedTest()
        {
            SoundIn = new WaveInEvent();
            soundOut = new DirectSoundOut();
            soundOut.Init(new WaveInProvider(SoundIn));

            SoundIn.StartRecording();
            soundOut.Play();
        }

        private static UdpClient udpListener;
        private static UdpClient udpSender;
        private static BufferedWaveProvider waveProvider;
        public static void PlayRecordedTestSockets()
        {
            var IpEndPoint = new IPEndPoint(new IPAddress(new byte[4] { 192, 168, 1, 17 }), 5000);
            udpSender = new UdpClient();
            SoundIn = new WaveInEvent();
            SoundIn.DataAvailable += (s, e) =>
            {
                udpSender.Send(e.Buffer, e.BytesRecorded);
            };
            SoundIn.StartRecording();
            udpListener = new UdpClient();
            udpListener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true); // test
            //
            var EndPointListen = new IPEndPoint(new IPAddress(new byte[4] { 0, 0, 0, 0 }), 5000);
            udpListener.Client.Bind(EndPointListen);
            //Console.ReadLine();
            udpSender.Connect(IpEndPoint);

            
            soundOut = new DirectSoundOut();
            waveProvider = new BufferedWaveProvider(SoundIn.WaveFormat);
            soundOut.Init(waveProvider);
            soundOut.Play();

            ThreadPool.QueueUserWorkItem(ListenerThread, EndPointListen);
        }

        private static void ListenerThread(object endPoint)
        {
            IPEndPoint endpoint2 = (IPEndPoint)endPoint;
            try
            {
                while (true)
                {
                    byte[] b = udpListener.Receive(ref endpoint2);
                    waveProvider.AddSamples(b, 0, b.Length);
                }
            }
            catch (SocketException)
            {
                // usually not a problem - just means we have disconnected
            }
        }
    }
}
