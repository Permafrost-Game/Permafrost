using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    /// <summary>
    /// This class will read a text file
    /// This class is just for pre release purposes and will eventually be replaced with content pipeline
    /// </summary>
    public static class ContentReader
    {

        public static List<string> LoadText(string filePath)
        {
            List<string> lines = new List<string>();

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }
    }
}
