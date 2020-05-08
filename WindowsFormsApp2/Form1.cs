using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        bool OpenCheck = false;
        private List<double> dataList;

        WaveReaderPC waveReaderPC;
        public Form1()
        {
            InitializeComponent();

            waveReaderPC = new WaveReaderPC();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog open = new OpenFileDialog())
            {
                open.Filter = "Wav File|*.wav";

                open.InitialDirectory = Environment.CurrentDirectory;

                if(open.ShowDialog() == DialogResult.OK)
                {
                    string fileName = open.FileName;

                    OpenCheck = waveReaderPC.Open(fileName);                                        
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {           
           
            if (OpenCheck == true)
            {
                chart1.ChartAreas[0].AxisX.Minimum = 0;
                chart1.ChartAreas[0].AxisY.Maximum = 120;
                chart1.ChartAreas[0].AxisY.Minimum = 30;

                Debug.WriteLine("{0}", waveReaderPC.fmtChunkID.Length);
                for (int i = 0; i < waveReaderPC.fmtChunkID.Length; i++)
                {
                    int j = i + 1;

                    Debug.WriteLine("{0}", i);
                    Debug.WriteLine("{0}", j);
                    
                    chart1.Series[0].Points.AddXY(j, waveReaderPC.fmtChunkID[i]);

                    Debug.WriteLine("data16[i] = {0}", waveReaderPC.fmtChunkID[i]);
                }
                /*
                //int b = Convert.ToInt32(waveReaderPC.data16L);
                //int c = BitConverter.ToInt32(waveReaderPC.data8R,0);
                //int value = Convert.ToInt32(waveReaderPC.data16L);

                Debug.WriteLine("{0}", waveReaderPC.data16L[0]);
                Debug.WriteLine("{0}", waveReaderPC.data16R[0]);
                //double a = waveReaderPC.data16L
                //chart1.Series[0].Points.AddY(1);
                
                *//*chart1.Series[0].Points.AddXY(3, c);
                chart1.Series[0].Points.AddXY(4, c);
                chart1.Series[0].Points.AddXY(5, c);
                chart1.Series[0].Points.AddXY(6, c);
                chart1.Series[0].Points.AddXY(7, c);*//*

                timer1.Enabled = true;*/
            }
            else
            {
                MessageBox.Show("파일을 열어주세요");
            } 

            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ChartData();
        }

        public void ChartData()
        {
            //dataList.Add();
        }

        
    }
}
