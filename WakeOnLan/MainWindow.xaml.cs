using System.Collections.Generic;
using System.Windows;

namespace WakeOnLan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const string SettingsPath = "Settings.ini";

        public MainWindow()
        {
            InitializeComponent();

            // Load settings from Settings.ini. If file does not exist it will be created.
            List<string> list = Settings.Load(SettingsPath);

            // If file is empty we will add 2 strings into it.
            if (list.Count == 0)
            {
                list.Add("");
                list.Add("");
                Settings.Save(SettingsPath, list);
            }

            // Fill text fields in GUI.
            textIP.Text = list[0];
            textMAC.Text = list[1];
        }

        private void ButtonWakeClick(object sender, RoutedEventArgs e)
        {
            // Send magic packet with "textMAC" mac address to "textIP" IP. 
            Logic.Send(Logic.GetMagicPacket(textMAC.Text), textIP.Text);
        }

        private void buttonSleep_Click(object sender, RoutedEventArgs e)
        {
            Logic.Send(Logic.GetAntiMagicPacket(textMAC.Text), textIP.Text);
        }

        /// <summary>
        /// If we change value of textBox "textIP" this new value will be immediately written into Settings.ini
        /// </summary>
        private void textIP_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // Load existing values.
            List<string> list = Settings.Load(SettingsPath);
            // Replace IP value with new one.
            list[0] = textIP.Text;
            // Write new settings into file.
            Settings.Save(SettingsPath, list);
        }

        /// <summary>
        /// If we change value of textBox "textMAC" this new value will be immediately written into Settings.ini
        /// </summary>
        private void textMAC_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            List<string> list = Settings.Load(SettingsPath);
            list[1] = textMAC.Text;
            Settings.Save(SettingsPath, list);
        }
    }
}
