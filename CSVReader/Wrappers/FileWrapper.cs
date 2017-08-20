using System.IO;

namespace CSVReader.Wrappers
{
    public interface IFile
    {
        IStreamReader OpenText(string path);
        void WriteAllLines(string path, string[] contents);
    }
    public class FileWrapper : IFile
    {
        public IStreamReader OpenText(string path)
        {
            return new StreamReaderWrapper(File.OpenText(path));
        }

        public void WriteAllLines(string path, string[] contents)
        {
            File.WriteAllLines(path, contents);
        }
    }
}
