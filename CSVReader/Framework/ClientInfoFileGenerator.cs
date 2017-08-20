namespace CSVReader.Framework
{
    public interface IClientInfoFileGenerator
    {
        void ProduceClientAnalysisFiles(string clientFileLocation);
    }

    public class ClientInfoFileGenerator : IClientInfoFileGenerator
    {
        private readonly IFileParser _FileParser;
        private readonly IFileAnalyser _FileAnalyser;
        private readonly IFileWriter _FileWriter;

        public ClientInfoFileGenerator()
            :this(new FileParser(), new FileAnalyser(), new FileWriter())
        {

        }

        public ClientInfoFileGenerator(
            IFileParser fileParser,
            IFileAnalyser fileAnalyser,
            IFileWriter fileWriter)
        {
            _FileParser = fileParser;
            _FileAnalyser = fileAnalyser;
            _FileWriter = fileWriter;
        }

        public void ProduceClientAnalysisFiles(string clientFileLocation)
        {
            var clientInfos = _FileParser.Parse(clientFileLocation);

            var frequencyOrderedNamesList = _FileAnalyser.GetFrequencyOrderedNamesList(clientInfos);

            var alphabeticallyOrderedAddressList = _FileAnalyser.GetAlphabeticallyOrderedAddressList(clientInfos);

            _FileWriter.WriteFile("OrderedListOfNames", clientFileLocation, frequencyOrderedNamesList);

            _FileWriter.WriteFile("OrderedListOfAddresses", clientFileLocation, alphabeticallyOrderedAddressList);
        }
    }
}
