using CSVReader.Framework;
using CSVReader.Wrappers;
using Moq;
using NUnit.Framework;

namespace CSVReaderTests
{
    [TestFixture]
    public   class Given_a_file_writer_when_writing_a_file
    {
        private Mock<IFile> _File;
        private string[] _FileLines;

        [SetUp]
        public void Setup()
        {
            _FileLines = new[] {""};

            var path = new Mock<IPath>();
            path
                .Setup(p => p.GetDirectoryName("location"))
                .Returns("directoryname");

            path
                .Setup(p => p.Combine("directoryname", "filename"))
                .Returns("newpath");

            _File = new Mock<IFile>();

            new FileWriter(path.Object, _File.Object)
                .WriteFile("filename", "location", _FileLines);
        }

        [Test]
        public void It_should_write_all_lines_to_the_correct_location()
        {
            _File.Verify(f => f.WriteAllLines("newpath.txt", _FileLines));
        }
    }
}
