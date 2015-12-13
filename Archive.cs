using System;
using System.Collections.Generic;
using ClutteredMarkov;
using System.IO;
using System.Text.RegularExpressions;

namespace ClTwitter_Ebooks
{
    static class Archive
    {
        private static List<string> ATweetContent = new List<string>();
        private static char[] JsonSplitter = { ':' };

        /// <summary>
        /// Parses only the "text" value of the given file
        /// </summary>
        /// <param name="filename">Filename of file to read</param>
        private static void ReadJSON(string filename)
        {
            foreach (string line in File.ReadAllLines(filename))
            {
                string[] parts = line.Split(JsonSplitter, 2);
                if (parts[0].Trim(' ') == "\"text\"")
                {
                    string textStr = Regex.Unescape(parts[1].Trim(' '));
                    textStr = textStr.Remove(0, 1); /* Removes the initial " */
                    textStr = textStr.Remove(textStr.Length - 2, 2);
                    ATweetContent.Add(textStr);
                }
            }
        }

        public static void ReadArchive()
        {
            try
            {
                foreach (string file in Directory.EnumerateFiles(Properties.Settings.Default.ArchivePath, "*.js"))
                {
                    ReadJSON(file);
                }
            } 
            catch (Exception excInfo)
            {
                Console.WriteLine(excInfo.Message);
                Console.ReadKey(true);
                System.Environment.Exit(0);
            }
        }

        public static void FeedAll()
        {
            foreach (string line in ATweetContent)
            {
                Markov.Feed(line);
            }
            Markov.Save();
        }

    }
}
