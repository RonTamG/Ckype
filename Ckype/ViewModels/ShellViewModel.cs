using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioLibrary;

namespace Ckype.ViewModels
{
    public class ShellViewModel
    {
        public void record()
        {
            AudioFile.Record(@"C:\Users\Owner\Desktop\AudioTestFile.wav");
        }
        public void StopRecord()
        {
            AudioFile.StopRecording();
        }
        public void play()
        {
            AudioFile.PlayFile(@"C:\Users\Owner\Desktop\AudioTestFile.wav");
        }
    }
}
