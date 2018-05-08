using NAudio.Wave;

namespace WindowsFormsApp3
{
    class AudioControlLibrary
    {
        private static WaveOut output;
        private static AudioFileReader audioFile;
        private static WaveIn waveIn;
        private static WaveFileWriter writer = null;
        public static AudioPosition currentPlaceInAudio = null;
        public static System.TimeSpan currentPlaceInAudio2;
        
        // recording


        public static void Record(string outputFilePath)
        {
            waveIn = new WaveIn();
            writer = new WaveFileWriter(outputFilePath, waveIn.WaveFormat);
            
            waveIn.RecordingStopped += (s, a) =>
            {
                writer?.Dispose();
                writer = null;
                waveIn.Dispose();
            };
            waveIn.StartRecording();
            waveIn.DataAvailable += (s, a) =>
            {
                writer.Write(a.Buffer, 0, a.BytesRecorded);
            };

        }


        public static void StopRecording()
        {
            waveIn?.StopRecording();
        }

        #region Play


        /// <summary>
        /// Starts playing the given file
        /// </summary>
        /// <param name="filename">The path of the file to play</param>
        public static void PlaySong(string filename)
        {
            if (output == null)
            {
                output = new WaveOut();
                output.PlaybackStopped += (s, e) => {
                    // clear output device
                    output.Dispose();
                    output = null;
                };
            }
            if (audioFile == null)
            {
                audioFile = new AudioFileReader(filename);
                output.Init(audioFile);
            }
            if (output.PlaybackState == PlaybackState.Paused)
            {
                output.Resume();
            }
            else
            output.Play();
        }

        /// <summary>
        /// Clear the currently playing file
        /// </summary>
        public static void ClearSongs()
        {

            // if the output device is not null
            if (output != null)
            {
                // stop playing the file
                if (output.PlaybackState == PlaybackState.Playing)
                {
                    output.Pause();
                    return;
                }
            }
            if (audioFile != null)
            {
                audioFile.Dispose();
                audioFile = null;
            }
        }


        #endregion
    }
}
