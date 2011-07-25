using System;
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

        // Load settings from Settings.ini. If file does not exist it will be created.
        private List<string> _list = Settings.Load(SettingsPath);

        public MainWindow()
        {
            InitializeComponent();

            // If file is empty we will add 2 strings into it.
            if (_list.Count == 0)
            {
                _list.Add("");
                _list.Add("");
                _list.Add("0");
                Settings.Save(SettingsPath, _list);
            }

            // Fill text fields in GUI.
            textIP.Text = _list[0];
            textMAC.Text = _list[1];
            comboBox1.SelectedIndex = Int32.Parse(_list[2]);
        }

        private void ButtonWakeClick(object sender, RoutedEventArgs e)
        {
            // Send magic packet with "textMAC" mac address to "textIP" IP. 
            Logic.Send(Logic.GetMagicPacket(textMAC.Text), textIP.Text);
        }

        private void buttonDoAction_Click(object sender, RoutedEventArgs e)
        {
            Logic.Send(Logic.GetAntiMagicPacket(textMAC.Text, (byte)comboBox1.SelectedIndex), textIP.Text);
        }

        /// <summary>
        /// If we change value of textBox "textIP" this new value will be immediately written into Settings.ini
        /// </summary>
        private void textIP_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // Replace IP value with new one.
            _list[0] = textIP.Text;
            // Write new settings into file.
            Settings.Save(SettingsPath, _list);
        }

        /// <summary>
        /// If we change value of textBox "textMAC" this new value will be immediately written into Settings.ini
        /// </summary>
        private void textMAC_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            _list[1] = textMAC.Text;
            Settings.Save(SettingsPath, _list);
        }

        private void comboBox1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _list[2] = comboBox1.SelectedIndex.ToString();
            Settings.Save(SettingsPath, _list);
        }
    }
}
