using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioLibrary;

namespace Ckype.ViewModels
{
    public class ShellViewModel : BaseViewModel
    {
        private AudioFile Player = null;
        private AudioFile Recorder = null;

        private string RecordFile = @"C:\Users\Owner\Desktop\AudioTestFile.wav";
        private string MusicFile = @"C:\Users\Owner\Music\Nightcore Man I Think I Love Her w-lyrics on screen.mp3";

        public void record()
        {
            Recorder = new AudioFile(RecordFile);
            Recorder.Record();
        }
        public void StopRecord()
        {
            Recorder.StopRecording();   
        }
        public void Play()
        {
            if (Player == null)
            {
                Player = new AudioFile(MusicFile);
                Player.PlaybackEnded += PlaybackStopped;
            }

            Player.PlayFile();
            CanPlay = false;
            CanPause = Player.IsFilePlaying;
            
        }
        public void Pause()
        {
            Player?.PauseFile();
            CanPlay = true;
            CanPause = false;
        }

        public bool CanPause { get; set; } = false;
        public bool CanPlay { get; set; } = true;

        private void PlaybackStopped()
        {
            CanPlay = true;
            CanPause = false;
        }
    }
}
