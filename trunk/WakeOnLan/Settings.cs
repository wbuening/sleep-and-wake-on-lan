using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace WakeOnLan
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

            try
            {
                if (File.Exists(path))
                {
                    var stream = new StreamReader(path);
                    while (!stream.EndOfStream)
                    {
                        list.Add(stream.ReadLine());
                    }
                    
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return list;
        }

        public static void Save(string path, List<string> list)
        {
            var stream = new StreamWriter(path);
            foreach(var item in list)
            {
                stream.WriteLine(item);
            }
            stream.Close();
        }
    }
}
