using System;
using System.Linq;
using CSVReader.DTOs;
using CSVReader.Framework;
using CSVReader.Wrappers;
using Moq;
using NUnit.Framework;

namespace CSVReaderTests
{
    [TestFixture]
    public class Given_a_file_parser
    {
        private ClientInfo[] _ClientInfos;

        [SetUp]
        public void When_parsing_a_file()
        {
            var streamReader = new Mock<IStreamReader>();
            var file = new Mock<IFile>();

            file
                .Setup(f => f.OpenText("file name"))
                .Returns(streamReader.Object);

            streamReader
                .SetupSequence(sr => sr.ReadLine())
                .Returns("LastName, Address, PhoneNumber,   FirstName")
                .Returns(" Smith, 102 Long Lane, 123123   ,   Jimmy")
                .Returns("Owen, 65 Ambling Way, 654354, Clive"); 

            var fileParser = new FileParser(file.Object);

            _ClientInfos = fileParser.Parse("file name");
        }

        [Test]
        public void It_should_produce_a_list_of_client_info()
        {
            var expectedResults = new[]
            {
                new ClientInfo
                {
                    Address = "102 Long Lane",
                    FirstName = "Jimmy",
                    LastName = "Smith",
                    PhoneNumber = "123123"
                },
                new ClientInfo
                {
                    Address = "65 Ambling Way",
                    FirstName = "Clive",
                    LastName = "Owen",
                    PhoneNumber = "654354"
                }
            };

            Assert.IsTrue(expectedResults.Length == _ClientInfos.Length 
                && expectedResults.All(expected => 
                    _ClientInfos.Any(
                        actual => actual.LastName == expected.LastName && 
                        actual.FirstName == expected.FirstName && 
                        actual.Address == expected.Address && 
                        actual.PhoneNumber == expected.PhoneNumber)));
        }
    }

    [TestFixture]
    public class Given_a_file_parser_with_missing_header_columns
    {
        private FileParser _FileParser;
        private Mock<IFile> _File;
        private Mock<IStreamReader> _StreamReader;

        [SetUp]
        public void Setup()
        {
            _StreamReader = new Mock<IStreamReader>();
            _File = new Mock<IFile>();

            _File
                .Setup(f => f.OpenText("file name"))
                .Returns(_StreamReader.Object);
        }

        [Test]
        public void It_should_throw_an_exception_when_missing_the_first_name()
        {
            _StreamReader
                .SetupSequence(sr => sr.ReadLine())
                .Returns("LastName, Address, PhoneNumber, WrongColumn");

            _FileParser = new FileParser(_File.Object);

            var exception = Assert.Throws<NotSupportedException>(() => _FileParser.Parse("file name"));

            Assert.AreEqual(
                "The file line [LastName, Address, PhoneNumber, WrongColumn] is not supported. Not all columns are present",
                exception.Message);
        }

        [Test]
        public void It_should_throw_an_exception_when_missing_the_last_name()
        {
            _StreamReader
                .SetupSequence(sr => sr.ReadLine())
                .Returns("WrongColumn, Address, PhoneNumber, FirstName");

            _FileParser = new FileParser(_File.Object);

            var exception = Assert.Throws<NotSupportedException>(() => _FileParser.Parse("file name"));

            Assert.AreEqual(
                "The file line [WrongColumn, Address, PhoneNumber, FirstName] is not supported. Not all columns are present",
                exception.Message);
        }

        [Test]
        public void It_should_throw_an_exception_when_missing_the_address()
        {
            _StreamReader
                .SetupSequence(sr => sr.ReadLine())
                .Returns("LastName, WrongColumn, PhoneNumber, FirstName");

            _FileParser = new FileParser(_File.Object);

            var exception = Assert.Throws<NotSupportedException>(() => _FileParser.Parse("file name"));

            Assert.AreEqual(
                "The file line [LastName, WrongColumn, PhoneNumber, FirstName] is not supported. Not all columns are present",
                exception.Message);
        }

        [Test]
        public void It_should_throw_an_exception_when_missing_the_phonenumber()
        {
            _StreamReader
                .SetupSequence(sr => sr.ReadLine())
                .Returns("LastName, Address, WrongColumn, FirstName");

            _FileParser = new FileParser(_File.Object);

            var exception = Assert.Throws<NotSupportedException>(() => _FileParser.Parse("file name"));

            Assert.AreEqual(
                "The file line [LastName, Address, WrongColumn, FirstName] is not supported. Not all columns are present",
                exception.Message);
        }
    }

    [TestFixture]
    public class Given_a_file_parser_with_dupliacte_header_columns
    {
        private FileParser _FileParser;

        [SetUp]
        public void Setup()
        {
            var streamReader = new Mock<IStreamReader>();
            var file = new Mock<IFile>();

            file
                .Setup(f => f.OpenText("file name"))
                .Returns(streamReader.Object);

            streamReader
                .SetupSequence(sr => sr.ReadLine())
                .Returns("LastName, Address, PhoneNumber, LastName");

            _FileParser = new FileParser(file.Object);
        }

        [Test]
        public void It_should_throw_an_exception_when_there_are_duplicate_columns()
        {
            var exception = Assert.Throws<NotSupportedException>(() => _FileParser.Parse("file name"));

            Assert.AreEqual("The file heading [LastName, Address, PhoneNumber, LastName] is not supported", exception.Message);
        }
    }

    [TestFixture]
    public class Given_a_file_parser_with_an_invalid_row
    {
        private FileParser _FileParser;
        private Mock<IFile> _File;
        private Mock<IStreamReader> _StreamReader;

        [SetUp]
        public void Setup()
        {
            _StreamReader = new Mock<IStreamReader>();
            _File = new Mock<IFile>();

            _File
                .Setup(f => f.OpenText("file name"))
                .Returns(_StreamReader.Object);

            _StreamReader
                .SetupSequence(sr => sr.ReadLine())
                .Returns("LastName, Address, PhoneNumber,   FirstName")
                .Returns(" Smith, 102 Long Lane, 123123   ");

            _FileParser = new FileParser(_File.Object);
        }

        [Test]
        public void It_should_throw_an_exception_when_missing_a_column()
        {
            var exception = Assert.Throws<NotSupportedException>(() => _FileParser.Parse("file name"));

            Assert.AreEqual("The file line [ Smith, 102 Long Lane, 123123   ] is not supported", exception.Message);
        }
    }

    [TestFixture]
    public class Given_a_file_parser_with_a_file_name_that_is_too_short
    {
        private Mock<IFile> _File;

        [SetUp]
        public void Setup()
        {
            _File = new Mock<IFile>();
        }

        [Test]
        public void It_should_throw_an_exception()
        {
            var exception = Assert.Throws<ArgumentException>(() => new FileParser(_File.Object).Parse(""));

            Assert.AreEqual(exception.Message, "The file name is too short");
        }
    }
}
