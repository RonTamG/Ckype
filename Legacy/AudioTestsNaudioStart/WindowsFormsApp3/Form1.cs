using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            AudioControlLibrary.PlaySong(@"C:\Users\Owner\Desktop\AudioTestFile.wav");
            //OutputText. = AudioControlLibrary.currentPlaceInAudio;
            StopButton.Enabled = true;
        }


        private void OnFormClose(object sender, FormClosingEventArgs e)
        {
            AudioControlLibrary.ClearSongs();
            AudioControlLibrary.StopRecording();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            AudioControlLibrary.ClearSongs();
            StopButton.Enabled = false;
        }

        private void RecordButton_Click(object sender, EventArgs e)
        {
            try
            {
                AudioControlLibrary.Record(@"C:\Users\Owner\Desktop\AudioTestFile.wav");
            }
            catch (RecordingEndedException){ return; }
            StopRecordButton.Enabled = true;
        }

        private void StopRecordButton_Click(object sender, EventArgs e)
        {
            AudioControlLibrary.StopRecording();
            StopRecordButton.Enabled = false;
        }

    }
    
}
