using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace WindowsFormsApp3
{
    public class AudioPosition : INotifyPropertyChanged
    {
        private AudioFileReader _fileReader { get; set; }
        private long _currentPos { get; set; }

        public AudioPosition(AudioFileReader fileReader)
        {
            this.fileReader = fileReader;
            this.currentPos = fileReader.Position;
        }

        public event PropertyChangedEventHandler PropertyChanged = (s, e) => { };

        public AudioFileReader fileReader
        {
            get { return _fileReader; }
            set
            {
                if (value == _fileReader)
                    return;

                _fileReader = value;
                PropertyChanged(this, new PropertyChangedEventArgs("fileReader"));
            }
        }

        public long currentPos
        {
            get { return _currentPos; }
            set
            {
                if (value == _currentPos)
                    return;

                _currentPos = value;
                PropertyChanged(this, new PropertyChangedEventArgs("currentPos"));
            }
        }
    }
}
