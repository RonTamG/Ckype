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
    public static class AudioFile
    {
        #region Private Variables

        private static AudioFileReader audioReader = null;
        private static DirectSoundOut soundOut = null;
        private static WaveInEvent soundIn;
        private static WaveFileWriter waveWriter = null;

        #endregion

        #region Playing

        /// <summary>
        /// Starts playing the given file
        /// 
        /// If you input a filename then the file will open and start playing
        /// Otherwise it plays the currently open file
        /// </summary>
        /// <param name="filename">The path of the file to play, DEFAULT = NULL</param>
        public static void PlayFile(string filename = null)
        {
            if (filename != null)
            {
                soundOut = new DirectSoundOut();
                soundOut.PlaybackStopped += PlaybackStopped;
                audioReader = new AudioFileReader(filename);
                soundOut.Init(audioReader);
            }
            soundOut.Play();
        }

        /// <summary>
        /// Pauses the currently open audio file, if it exists
        /// </summary>
        public static void PauseFile()
        {
            soundOut?.Pause();
        }

        /// <summary>
        /// Disposes of all current active file resources
        /// </summary>
        public static void CloseFile()
        {
            soundOut?.Pause();
            soundOut.PlaybackStopped -= PlaybackStopped;
            soundOut?.Dispose();
            audioReader?.Dispose();
        }

        private static void PlaybackStopped(object sender, StoppedEventArgs e)
        {
            // clear output device
            soundOut.Dispose();
            soundOut = null;
        }
        #endregion

        #region Recording

        /// <summary>
        /// Record audio to file
        /// </summary>
        /// <param name="outputFilePath">Path to recorded file</param>
        public static void Record(string outputFilePath)
        {
            soundIn = new WaveInEvent();
            waveWriter = new WaveFileWriter(outputFilePath, soundIn.WaveFormat);

            soundIn.StartRecording();
            soundIn.DataAvailable += DataAvailable;
        }

        /// <summary>
        /// Stop recording if the sound in variable exists
        /// </summary>
        public static void StopRecording()
        {
            soundIn?.StopRecording();
            soundIn.DataAvailable -= DataAvailable;
            waveWriter?.Dispose();
            waveWriter = null;
            soundIn.Dispose();
        }

        private static void DataAvailable(object sender, WaveInEventArgs e)
        {
            waveWriter.Write(e.Buffer, 0, e.BytesRecorded);
        }
        #endregion
    }
}
