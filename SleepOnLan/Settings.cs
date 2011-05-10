using System;
using System.IO;
using System.Windows;

namespace SleepOnLan
{
    public static class Settings
    {
        /// <summary>
        /// Load the action id from file.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <returns>Id of action which we will do.</returns>
        public static int Load(string path)
        {
            int ret = 0;

            try
            {
                if (File.Exists(path))
                {
                    var stream = new StreamReader(path);
                    ret = Int32.Parse(stream.ReadToEnd());
                    stream.Close();
                }
                else
                {
                    Save(path, "0");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return ret;
        }

        public static void Save(string path, string value)
        {
            var stream = new StreamWriter(path);
            stream.WriteLine(value);
            stream.Close();
        }
    }
}
