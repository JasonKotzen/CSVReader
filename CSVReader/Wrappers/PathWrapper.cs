using System.IO;

namespace CSVReader.Wrappers
{
    public interface IPath
    {
        string GetDirectoryName(string path);
        string Combine(string path1, string path2);
    }

    public class PathWrapper : IPath
    {
        public string GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        public string Combine(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }
    }
}
