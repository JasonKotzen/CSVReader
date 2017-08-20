using System;
using System.Collections.Generic;
using System.Linq;
using CSVReader.DTOs;
using CSVReader.Wrappers;

namespace CSVReader.Framework
{
    public interface IFileParser
    {
        ClientInfo[] Parse(string fileName);
    }

    public class FileParser : IFileParser
    {

        private readonly IFile _File;

        private int? _FirstNameIndex;
        private int? _LastNameIndex;
        private int? _AddressIndex;
        private int? _PhoneNumberIndex;
        private const string Lastname = "LastName";
        private const string Address = "Address";
        private const string Phonenumber = "PhoneNumber";
        private const string Firstname = "FirstName";

        public FileParser() : this(new FileWrapper())
        {
            
        }

        public FileParser(IFile file)
        {
            _File = file;
        }

        public ClientInfo[] Parse(string fileName)
        {
            if(string.IsNullOrEmpty(fileName)) throw new ArgumentException("The file name is too short");

            var firstRow = true;

            using (var sr = _File.OpenText(fileName))
            {
                string input;
                var clientInfos = new List<ClientInfo>();

                while ((input = sr.ReadLine()) != null)
                {
                    if (firstRow)
                    {
                        ParseHeadingLine(input);
                        firstRow = false;
                    }
                    else
                        clientInfos.Add(ParseBodyLine(input));
                }

                return clientInfos.ToArray();
            }
        }

        private void ParseHeadingLine(string input)
        {
            var rowSplitUpByComma = input.Split(',');

            GuardAgainstIncorrectNumberOfHeadingColumns(input, rowSplitUpByComma);

            for(var row =0; row < rowSplitUpByComma.Length; row ++)
            {
                if (string.Compare(rowSplitUpByComma[row].Trim(), Firstname, StringComparison.InvariantCultureIgnoreCase) ==0)
                    _FirstNameIndex = row;
                
                if (string.Compare(rowSplitUpByComma[row].Trim(), Lastname, StringComparison.InvariantCultureIgnoreCase) == 0)
                    _LastNameIndex= row;

                if (string.Compare(rowSplitUpByComma[row].Trim(), Address, StringComparison.InvariantCultureIgnoreCase) == 0)
                    _AddressIndex = row;
                
                if (string.Compare(rowSplitUpByComma[row].Trim(), Phonenumber, StringComparison.InvariantCultureIgnoreCase) == 0)
                    _PhoneNumberIndex = row;
            }

            GuardAgainstMissingHeaderColumns(input);
        }

        private ClientInfo ParseBodyLine(string lineOfText)
        {
            var rowSplitUpByComma = lineOfText.Split(',');

            GuardAgainstBodyLineException(lineOfText, rowSplitUpByComma);

            var address = rowSplitUpByComma[_AddressIndex.Value].Trim();
            var firstName = rowSplitUpByComma[_FirstNameIndex.Value].Trim();
            var lastName = rowSplitUpByComma[_LastNameIndex.Value].Trim();
            var phoneNumber = rowSplitUpByComma[_PhoneNumberIndex.Value].Trim();

            return new ClientInfo
            {
                Address = address,
                PhoneNumber = phoneNumber,
                FirstName = firstName,
                LastName = lastName
            };
        }

        private static void GuardAgainstBodyLineException(string lineOfText, string[] rowSplitUpByComma)
        {
            if (rowSplitUpByComma.Length != 4)
                throw new NotSupportedException($"The file line [{lineOfText}] is not supported");
        }

        private static void GuardAgainstIncorrectNumberOfHeadingColumns(string input, string[] rowSplitUpByComma)
        {
            if (rowSplitUpByComma.GroupBy(x => x.Trim()).Count() != 4)
                throw new NotSupportedException($"The file heading [{input}] is not supported");
        }

        private void GuardAgainstMissingHeaderColumns(string input)
        {
            if (!_FirstNameIndex.HasValue || !_LastNameIndex.HasValue || !_AddressIndex.HasValue || !_PhoneNumberIndex.HasValue)
                throw new NotSupportedException($"The file line [{input}] is not supported. Not all columns are present");
        }
    }
}
