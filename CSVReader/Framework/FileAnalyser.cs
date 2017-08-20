using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CSVReader.DTOs;

namespace CSVReader.Framework
{
    public interface IFileAnalyser
    {
        string[] GetFrequencyOrderedNamesList(ClientInfo[] clientInfos);

        string[] GetAlphabeticallyOrderedAddressList(ClientInfo[] clientInfos);
    }

    public class FileAnalyser : IFileAnalyser
    {
        public string[] GetFrequencyOrderedNamesList(ClientInfo[] clientInfos)
        {
            var allNames = clientInfos.Select(c => c.FirstName)
                .Concat(clientInfos.Select(c => c.LastName));

            var grouped = allNames
                .GroupBy(grouping => grouping)
                .OrderByDescending(grouping => grouping.Count())
                .ThenBy(grouping => grouping.Key);

            return grouped.Select(g => $"{g.Key}, {g.Count().ToString()}").ToArray();
        }

        public string[] GetAlphabeticallyOrderedAddressList(ClientInfo[] clientInfos)
        {
            var addressesList = new List<Tuple<string, string>>();
            
            foreach (var clientInfo in clientInfos)
            {
                var addressArray = clientInfo.Address.Split(' ');

                if(addressArray.Length <= 1)
                    throw new NotSupportedException($"The following address is not supported [{clientInfo.Address}]");
                
                if(!IsNumeric(addressArray[0]))
                    throw new NotSupportedException($"The following address is not supported [{clientInfo.Address}] because a street number is required");

                addressesList.Add(new Tuple<string, string>(addressArray[1], clientInfo.Address));
            }

            return addressesList
                .OrderBy(addressTuple => addressTuple.Item1)
                .Select(addressTuple => addressTuple.Item2).ToArray();
        }

        private static bool IsNumeric(string input)
        {
            return Regex.IsMatch(input, @"^\d+$");
        }
    }
}
