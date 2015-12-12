using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace Lucas_Ebooks
{
    static class Archive
    {
        private static string ReadJSON(string filename)
        {
            StreamReader reader = new StreamReader(filename);
            string readLine = "";
            string currentLine = "";
            reader.ReadLine(); /* Ignore first line */
            while (currentLine != null)
            {
                currentLine = reader.ReadLine();
                if (currentLine != null)
                {
                    readLine += currentLine + '\n';
                }
            }
            return readLine;
        }
        internal static void ParseFile(string filename)
        {
            List<object> fuckList = new List<object>();
            string file = ReadJSON(filename); //File.ReadAllText(filename);
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;
            setting.MissingMemberHandling = MissingMemberHandling.Ignore;
            JsonSerializer serializer = new JsonSerializer();

            JArray json = JArray.Parse(file);
            var parsedData = JsonConvert.DeserializeObject(file, typeof(ATweet),setting);
        }
    }
}
