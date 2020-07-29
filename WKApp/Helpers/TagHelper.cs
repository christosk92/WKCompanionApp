using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WKApp.Helpers
{
    public static class TagHelper
    {
        private static List<int> AllIndexesOf(this string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }
        private static string[] types =
        {
            "<vocabulary>",
            "<ja>",
            "<kanji>",
            "<a",
            "<jp>",
            "<kan>",
            "<voc>",
            "<radical>"
        };
        public static bool ContainsTag(this string input) => types.Any(c => input.Contains(c));

        public static List<String> Tags(this string input)
        {
            List<string> returnList = new List<string>();
            List<int> indicesFirst = input.AllIndexesOf("<");
            List<int> indicesLast = input.AllIndexesOf(">");
            for (int i = 0; i < indicesFirst.Count; i++)
            {
                returnList.Add(input.Substring(indicesFirst[i], indicesLast[i] - indicesFirst[i] + 1));
            }
            return returnList;
        }
        public static string[] SplitAt(this string source, params int[] index)
        {
            index = index.Distinct().OrderBy(x => x).ToArray();
            string[] output = new string[index.Length + 1];
            int pos = 0;

            for (int i = 0; i < index.Length; pos = index[i++])
                output[i] = source.Substring(pos, index[i] - pos);

            output[index.Length] = source.Substring(pos);
            return output;
        }
        public static List<KeyValuePair<int, int>> TagsIndices(this string input)
        {
            List<KeyValuePair<int, int>> returnList = new List<KeyValuePair<int, int>>();
            List<int> indicesFirst = input.AllIndexesOf("<");
            indicesFirst = indicesFirst.Where((x, i) => i % 2 == 0).ToList();
            List<int> indicesLast = input.AllIndexesOf(">");
            indicesLast = indicesLast.Where((x, i) => i % 2 != 0).ToList();

            for (int i = 0; i < indicesFirst.Count; i++)
            {
                int first = indicesFirst[i];
                int last = indicesLast[i] - indicesFirst[i] + 1;
                returnList.Add(new KeyValuePair<int, int>(first, last));
            }
            return returnList;
        }
    }
}
