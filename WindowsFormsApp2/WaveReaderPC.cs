using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    public class WaveReaderPC
    {
        FileInfo m_fInfo;       //파일 읽고 쓰고 FileStream개체만드는거 도와줌.
        FileStream m_fStream;   //파일개체 불러오기..,,
        BinaryReader m_binReader;       //기본 데이터 형식을 이진값으로 읽어오는거.


        //wav파일 
        // = Header(Sampling Rate, 샘플당 바이트 수, 채널과 같은 정보) + PCM 데이터(실제 소리에 대한 데이터).
        //wav파일은 RIFF, fmt, Data부분으로 나뉨.

        // RIFF chunk
        byte[] chunkID;     // 이곳에 RIFF 들어감.
        UInt32 chunkSize;   // 파일크기에서 Chunk ID와 Chunk Size(8)을 뺀 값 == 8
        byte[] format;      //파일의 포맷. 그냥 대문자로 "WAVE" 들어가있음.

        // fmt subchunk
        public byte[] fmtChunkID;      //재생에 필요한 해당 PCM의 형식을 저장하는 chunk의 시작부분.
        UInt32 fmtChunkSize;    //실제 기록되는 데이터 포맷의 크기.,, 16bytes의 크기를 갖고있음.
        UInt16 audioFormat;     //PCM의 경우 1.
        public UInt16 numChannels;      //채널 수. Mono는 1, Streo는 2. 2byte
        UInt32 sampleRate;              //1초에 몇 조각으로 세분화 할 것인지. 4byte로 구성.
        UInt32 byteRate;        //SampleRate * NumChannles * BitsPerSample / 8
                            //1초의 소리가 차지하는 바이트 수. 
        
        UInt16 blockAssign;     //전체 채널을 포함하는 한 샘플의 크기, 
                            
        public UInt16 BitsPerSample; //샘플당 비트수. 1초로 샘플을 나누고 각각의 샘플을 몇 개의 비트로 표현할 지 나타냄.

        // data subchunk
        byte[] dataChunkID;             //data가 들어감.
        UInt32 dataChunkSize;           //실제 PCM데이터의 사이즈
                        //(BitsPerSample / 8) * Nu8mChannels * 실제 샘플 수.

        //실제 PCM데이터들.
        public byte[] data8L;              //1채널일때 연속해서 저장되는 8bit sample
                                           //또는 2채널일때 왼쪽 채널에 저장되는 8bit sample
        public byte[] data8R;              //2채널일때 오른쪽 채널에 저장되는 8-bit sample
        
        public Int16[] data16L;           //1채널일때 연속해서 저장되는 16bit sample
                                          //또는 2채널일때 왼쪽 채널에 저장되는 8bit sample
        public Int16[] data16R;           //2채널일때 오른쪽 채널에 저장되는 16bit sample

        public int numSamples;
        /*
        public WaveReader()
        {

        }
        */

        public bool Open(String filename)
        {
            string str;
            m_fInfo = new FileInfo(filename);
            m_fStream = m_fInfo.OpenRead();
            m_binReader = new BinaryReader(m_fStream);

            chunkID = new byte[4];
            format = new byte[4];

            chunkID = m_binReader.ReadBytes(4);
            chunkSize = m_binReader.ReadUInt32();
            format = m_binReader.ReadBytes(4);

            str = System.Text.ASCIIEncoding.ASCII.GetString(chunkID, 0, 4);
            if (str != "RIFF")
                return false;

            str = System.Text.ASCIIEncoding.ASCII.GetString(format, 0, 4);
            if (str != "WAVE")
                return false;

            if (ReadFmt() == false)
                return false;
            if (ReadData() == false)
                return false;

            m_fStream.Close();
            
            return true;
        }

        private bool ReadFmt()
        {
            fmtChunkID = new byte[4];
            fmtChunkID = m_binReader.ReadBytes(4);

            string str = System.Text.ASCIIEncoding.ASCII.GetString(fmtChunkID, 0, 4);
            if (str != "fmt ")
                return false;

            fmtChunkSize = m_binReader.ReadUInt32();
            audioFormat = m_binReader.ReadUInt16();
            numChannels = m_binReader.ReadUInt16();
            sampleRate = m_binReader.ReadUInt32();
            byteRate = m_binReader.ReadUInt32();
            blockAssign = m_binReader.ReadUInt16();
            BitsPerSample = m_binReader.ReadUInt16();

            return true;
        }

        private bool ReadData()
        {
            dataChunkID = new byte[4];
            dataChunkID = m_binReader.ReadBytes(4);
            string str = System.Text.ASCIIEncoding.ASCII.GetString(dataChunkID, 0, 4);
            if (str != "data")
                return false;

            // Read the size of data chunk
            // chunkSize = numSamples * numChannels * BitsPerSample/8
            dataChunkSize = m_binReader.ReadUInt32();
            numSamples = (int)dataChunkSize / (int)(numChannels * BitsPerSample / 8);

            int i;
            // Read sound data
            if (BitsPerSample == 8)
            {
                if (numChannels == 1)
                {
                    data8L = new byte[numSamples];
                    data8L = m_binReader.ReadBytes(numSamples);
                }
                else if (numChannels == 2)
                {
                    data8L = new byte[numSamples];
                    data8R = new byte[numSamples];

                    for (i = 0; i < numSamples; i++)
                    {
                        data8L[i] = m_binReader.ReadByte();
                        data8R[i] = m_binReader.ReadByte();
                    }
                }
            }
            else if (BitsPerSample == 16)
            {
                if (numChannels == 1)
                {
                    data16L = new Int16[numSamples];
                    for (i = 0; i < numSamples; i++)
                        data16L[i] = m_binReader.ReadInt16();
                }
                else if (numChannels == 2)
                {
                    data16L = new Int16[numSamples];
                    data16R = new Int16[numSamples];

                    for (i = 0; i < numSamples; i++)
                    {
                        data16L[i] = m_binReader.ReadInt16();
                        data16R[i] = m_binReader.ReadInt16();
                    }
                }
            }
            return true;
        }


        /// 

        /// Return 16-bit sound data
        /// 
        /// 
        /// 0 : return left channel
        /// 1 : return right channel
        /// 
        /// 
        /// return 16-bit sound data
        /// 
        public Int16[] getData16(int channel)
        {
            if (channel == 0)
                return data16L;
            else
                return data16R;
        }

        /// 

        /// Return 8-bit sound data
        /// 
        /// 
        /// 0 : return left channel
        /// 1 : return right channel
        /// 
        /// 
        /// return 8-bit sound data
        /// 
        public byte[] getData8(int channel)
        {
            if (channel == 0)
                return data8L;
            else
                return data8R;
        }

        public UInt16 getAudioFormat()
        {
            return audioFormat;
        }

        public UInt16 getNumChannels()
        {
            return numChannels;
        }

        public UInt32 getSampleRate()
        {
            return sampleRate;
        }

        public UInt32 getByteRate()
        {
            return byteRate;
        }

        public UInt16 getBitsPerSample()
        {
            return BitsPerSample;
        }

        public int getNumSamples()
        {
            return numSamples;
        }
    }
}
