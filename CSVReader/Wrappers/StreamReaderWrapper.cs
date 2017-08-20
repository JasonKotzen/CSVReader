using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReader.Wrappers
{
    public interface IStreamReader : IDisposable
    {
        string ReadLine();
    }

    public class StreamReaderWrapper : IStreamReader
    {
        private readonly StreamReader _StreamReader;

        public StreamReaderWrapper(StreamReader streamReader)
        {
            _StreamReader = streamReader;
        }

        public string ReadLine()
        {
            return _StreamReader.ReadLine();
        }

        public void Dispose()
        {
            _StreamReader?.Dispose();
        }
    }
}
