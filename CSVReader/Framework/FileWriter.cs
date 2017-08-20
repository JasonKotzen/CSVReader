using CSVReader.Wrappers;

namespace CSVReader.Framework
{
    public interface IFileWriter
    {
        void WriteFile(string fileName, string location, string[] fileLines);
    }

    public class FileWriter : IFileWriter
    {
        private readonly IPath _Path;
        private readonly IFile _File;

        public FileWriter() : this(new PathWrapper(), new FileWrapper())
        { }

        public FileWriter(IPath path, IFile file)
        {
            _Path = path;
            _File = file;
        }

        public void WriteFile(string fileName, string location, string[] fileLines)
        {
            var directory = _Path.GetDirectoryName(location);

            var newPath = _Path.Combine(directory, fileName);

            _File.WriteAllLines($"{newPath}.txt", fileLines);
        }
    }
}
