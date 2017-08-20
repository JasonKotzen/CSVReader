using CSVReader.DTOs;
using CSVReader.Framework;
using Moq;
using NUnit.Framework;

namespace CSVReaderTests
{
    [TestFixture]
    public class Given_a_client_info_file_generator
    {
        private Mock<IFileWriter> _FileWriter;
        private string[] _FrequencyOrderedNameList;
        private string[] _AlphabeticallyOrderedAddressList;

        [SetUp]
        public void Setup()
        { 
            var fileParser = new Mock<IFileParser>();
            var fileAnalyser = new Mock<IFileAnalyser>();
            _FileWriter = new Mock<IFileWriter>();

            var frequencyOrderedNameList = new string[0];
            var alphabeticallyOrderedAddressList = new string[0];
            var clientInfo = new[] {new ClientInfo()};

            fileParser
                .Setup(parser => parser.Parse("file name"))
                .Returns(clientInfo);

            _FrequencyOrderedNameList = frequencyOrderedNameList;
            fileAnalyser
                .Setup(analyser => analyser.GetFrequencyOrderedNamesList(clientInfo))
                .Returns(_FrequencyOrderedNameList);

            _AlphabeticallyOrderedAddressList = alphabeticallyOrderedAddressList;
            fileAnalyser
                .Setup(analyser => analyser.GetAlphabeticallyOrderedAddressList(clientInfo))
                .Returns(_AlphabeticallyOrderedAddressList);

            var clientFileInfoGenerator = new ClientInfoFileGenerator(
                fileParser.Object,
                fileAnalyser.Object,
                _FileWriter.Object);

            clientFileInfoGenerator.ProduceClientAnalysisFiles("file name");

        }

        [Test]
        public void It_should_produce_an_ordered_list_of_first_and_last_names()
        {
            _FileWriter
                .Verify(writer => writer.WriteFile("OrderedListOfNames", "file name", _FrequencyOrderedNameList));                                    
        }

        [Test]
        public void It_should_produce_an_ordered_list_of_addresses_and_street_names()
        {
            _FileWriter
                .Verify(writer => writer.WriteFile("OrderedListOfAddresses", "file name",_AlphabeticallyOrderedAddressList));
        }
    }
}
