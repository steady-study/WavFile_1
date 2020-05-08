using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{    
    public partial class Form1 : Form
    {
        MP3Player mp3Player;
        WaveReader waveReader;

        public Form1()
        {
            InitializeComponent();

            mp3Player = new MP3Player();
            waveReader = new WaveReader();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog open = new OpenFileDialog())
            {
                open.Filter = "Mp3 File|*.mp3|Wav File|*.wav";

                open.InitialDirectory = Environment.CurrentDirectory;

                if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string fileName = open.FileName;

                    mp3Player.Open(fileName);
                    //MP3TrackBar.Maximum = mp3Player.GetLength();
                    MP3Timer.Enabled = true;
                   

                }




            }
            //chunkID = new byte[4];
        }

        private void MP3Timer_Tick(object sender, EventArgs e)
        {
            if (!mp3Player.isOpened)
                return;

            SetMusicTimer();
        }

        private void SetMusicTimer()
        {
            if (mp3Player.isOpened)
            {
                TimeSpan t = TimeSpan.FromMilliseconds(mp3Player.GetPosition());
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if(MP3Timer.Enabled == true)
            {
                mp3Player.Play();
                ChartTimer.Start();
            }
            else
            {
                MessageBox.Show("파일을 선택하지 않았습니다.");
            }
            
        }

        private void ChartTimer_Tick(object sender, EventArgs e)
        {
            waveReader.ReadData();


        }
    }
}
