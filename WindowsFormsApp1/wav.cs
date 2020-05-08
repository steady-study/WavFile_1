/*using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WindowsFormsApp1
{
    class WaveReader
    {
        FileInfo m_fInfo;
        FileStream m_fStream;
        BinaryReader m_binReader;

        // RIFF chunk
        byte[] chunkID;
        UInt32 chunkSize;
        byte[] format;

        // fmt subchunk
        byte[] fmtChunkID;
        UInt32 fmtChunkSize;
        UInt16 audioFormat;
        UInt16 numChannels;
        UInt32 sampleRate;
        UInt32 byteRate;
        UInt16 blockAssign;
        UInt16 BitsPerSample;

        // data subchunk
        byte[] dataChunkID;
        UInt32 dataChunkSize;
        byte[] data8L;              // 8-bit left channel
        byte[] data8R;              // 8-bit right channel
        Int16[] data16L;           // 16-bit left channel
        Int16[] data16R;           // 16-bit right channel
        int numSamples;

        public WaveReader()
        {

        }

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
}*/