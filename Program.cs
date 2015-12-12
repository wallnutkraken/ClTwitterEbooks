using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lucas_Ebooks
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (string file in Directory.EnumerateFiles("./twitter"))
            {
                Archive.ReadJSON(file);
            }
        }
    }
}
