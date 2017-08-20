using System;
using System.Linq;
using CSVReader.DTOs;
using CSVReader.Framework;
using NUnit.Framework;

namespace CSVReaderTests
{
    [TestFixture]
    public class Given_a_file_analyser
    {
        [Test]
        public void It_should_be_able_to_produce_a_frequency_ordered_names_list()
        {
            var clientInfos = new[]
            {
                new ClientInfo {FirstName = "Matt", LastName = "Brown"},
                new ClientInfo {FirstName = "Heinrich", LastName = "Jones"},
                new ClientInfo {FirstName = "Johnson", LastName = "Smith"},
                new ClientInfo {FirstName = "Tim", LastName = "Johnson"}
            };

            var namesList = new FileAnalyser().GetFrequencyOrderedNamesList(clientInfos).ToArray();

            Assert.AreEqual(namesList[0], "Johnson, 2");
            Assert.AreEqual(namesList[1], "Brown, 1");
            Assert.AreEqual(namesList[2], "Heinrich, 1");
            Assert.AreEqual(namesList[3], "Jones, 1");
            Assert.AreEqual(namesList[4], "Matt, 1");
            Assert.AreEqual(namesList[5], "Smith, 1");
            Assert.AreEqual(namesList[6], "Tim, 1");
        }
    }

    [TestFixture]
    public class Given_a_file_analyser_when_producing_an_alphabetically_ordered_address_list
    {
        [Test]
        public void It_should_be_able_to_produce_a_frequency_ordered_names_list()
        {
            var clientInfos = new[]
            {
                new ClientInfo {Address = "33 Clifton street"},
                new ClientInfo {Address = "22 Jones Road"},
                new ClientInfo {Address = "12 Action St"}
            };

            var namesList = new FileAnalyser().GetAlphabeticallyOrderedAddressList(clientInfos).ToArray();

            Assert.AreEqual(namesList[0], "12 Action St");
            Assert.AreEqual(namesList[1], "33 Clifton street");
            Assert.AreEqual(namesList[2], "22 Jones Road");
        }

        [Test]
        public void It_should_throw_a_not_supported_exception_when_an_address_does_not_have_at_least_2_parts_to_it()
        {
            var clientInfos = new[]
            {
                new ClientInfo {Address = ""},
            };

            var exception = Assert.Throws<NotSupportedException>(() => new FileAnalyser()
                .GetAlphabeticallyOrderedAddressList(clientInfos).ToArray());

            Assert.AreEqual("The following address is not supported []", exception.Message);


        }

        [Test]
        public void It_should_throw_a_not_supported_exception_when_an_address_does_not_have_a_number()
        {
            var clientInfos = new[]
            {
                new ClientInfo {Address = "Clifton street"},
            };

            var exception = Assert.Throws<NotSupportedException>(() => new FileAnalyser()
                .GetAlphabeticallyOrderedAddressList(clientInfos).ToArray());

            Assert.AreEqual("The following address is not supported [Clifton street] because a street number is required", exception.Message);
        }
    }
}
