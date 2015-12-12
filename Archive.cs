using System;
using System.Collections.Generic;
using System.Linq;
using TweetSharp;
using System.IO;
using System.Text.RegularExpressions;

namespace Lucas_Ebooks
{
    static class Archive
    {
        private static List<string> _ATweetContent = new List<string>();
        private static char[] JsonSplitter = { ':' };
        public static List<string> ATweetContent
        {
            get
            {
                return _ATweetContent;
            }
        }
        /// <summary>
        /// Parses only the "text" value of the given file
        /// </summary>
        /// <param name="filename">Filename of file to read</param>
        public static void ReadJSON(string filename)
        {
            foreach (string line in File.ReadAllLines(filename))
            {
                string[] parts = line.Split(JsonSplitter, 2);
                if (parts[0].Trim(' ') == "\"text\"")
                {
                    string textStr = Regex.Unescape(parts[1].Trim(' '));
                    textStr = textStr.Remove(0, 1); /* Removes the initial " */
                    textStr = textStr.Remove(textStr.Length - 2, 2);
                    _ATweetContent.Add(textStr);
                }
            }
        }

    }
}
