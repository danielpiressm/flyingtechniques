using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace KinectClient
{
    class FileWriter
    {
        private string _filename;
        private StreamWriter _stream;

        public FileWriter(string filename)
        {
            _filename = filename;
            _stream = new StreamWriter(_filename);
        }

        public void write(string line)
        {
            _stream.WriteLine(line);
            _stream.Flush();
        }

        public void close()
        {
            _stream.Close();
        }
    }
}
