using System.Collections.Generic;
using System.IO;


namespace Engine
{
    /// <summary>
    /// This class will read a text file
    /// This class is just for pre release purposes and will eventually be replaced with a procedurally generated map
    /// </summary>
    public static class ContentReader
    {

        /// <summary>
        /// Returns each line as a seperate element in a list
        /// </summary>
        /// <param name="filePath">The file that is to be loaded</param>
        /// <returns>String list of each line in the file</returns>
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
