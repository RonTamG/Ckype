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
    public delegate void EndOfPlaybackEvent();
    /// <summary>
    /// Class for manipulating Audio Files
    /// </summary>
    public class AudioFile
    {
        #region Private Variables

        private AudioFileReader audioReader = null;
        private DirectSoundOut soundOut = null;
        private WaveInEvent soundIn;
        private WaveFileWriter waveWriter = null;

        #endregion

        #region Public Variables

        public bool IsFilePlaying { get; private set; }
        public bool IsFileClosed { get; private set; }
        public string Filename { get; private set; }
        public event EndOfPlaybackEvent PlaybackEnded;

        #endregion

        #region Constructor

        public AudioFile(string filename)
        {
            this.Filename = filename;

            IsFileClosed = true;
            IsFilePlaying = false;

        }

        #endregion

        #region Playing

        /// <summary>
        /// Starts playing the given file
        /// 
        /// If you input a filename then the file will open and start playing
        /// Otherwise it plays the currently open file
        /// </summary>
        public void PlayFile()
        {
            if (IsFileClosed)
            {
                soundOut = new DirectSoundOut();
                soundOut.PlaybackStopped += PlaybackStopped;
                audioReader = new AudioFileReader(this.Filename);
                soundOut.Init(audioReader);
                IsFileClosed = false;
            }
            soundOut.Play();

            IsFilePlaying = true;
        }

        /// <summary>
        /// Pauses the currently open audio file, if it exists
        /// </summary>
        public void PauseFile()
        {
            soundOut?.Pause();
            IsFilePlaying = false;
        }

        /// <summary>
        /// Disposes of all current active file resources
        /// </summary>
        public void CloseFile()
        {
            soundOut?.Pause();
            soundOut.PlaybackStopped -= PlaybackStopped;
            soundOut?.Dispose();
            audioReader?.Dispose();

            IsFilePlaying = false;
            IsFileClosed = true;
        }

        private void PlaybackStopped(object sender, StoppedEventArgs e)
        {
            this.PlaybackEnded();
            this.CloseFile();
        }
        #endregion

        #region Recording

        /// <summary>
        /// Record audio to file
        /// </summary>
        public void Record()
        {
            if (IsFileClosed)
            {
                soundIn = new WaveInEvent();
                waveWriter = new WaveFileWriter(this.Filename, soundIn.WaveFormat);

                soundIn.StartRecording();
                soundIn.DataAvailable += DataAvailable;
                IsFileClosed = false;
            }
        }

        /// <summary>
        /// Stop recording if the sound in variable exists
        /// </summary>
        public void StopRecording()
        {
            soundIn?.StopRecording();
            soundIn.DataAvailable -= DataAvailable;
            waveWriter?.Dispose();
            waveWriter = null;
            soundIn.Dispose();

            IsFileClosed = true;
        }

        private void DataAvailable(object sender, WaveInEventArgs e)
        {
            waveWriter.Write(e.Buffer, 0, e.BytesRecorded);
        }
        #endregion
    }
}
