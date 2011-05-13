using System.Collections.Generic;
using System.IO;

namespace SleepOnLan
{
    public static class Settings
    {
        /// <summary>
        /// Load settings from file.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <returns>List of settings. Each list item corresponds to each string in file.</returns>
        public static List<string> Load(string path)
        {
            var list = new List<string>();

            if (File.Exists(path))
            {
                // If we use "using" keyword Dispose() method (same as Close() in our case) will be called automatically.
                using (var stream = new StreamReader(path))
                {
                    while (!stream.EndOfStream)
                    {
                        list.Add(stream.ReadLine());
                    }
                }
            }

            return list;
        }

        public static void Save(string path, List<string> list)
        {
            using (var stream = new StreamWriter(path))
            {
                foreach (var item in list)
                {
                    stream.WriteLine(item);
                }
            }
        }
    }
}
