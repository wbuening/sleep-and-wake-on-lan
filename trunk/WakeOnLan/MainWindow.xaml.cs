using System.Collections.Generic;
using System.Windows;

namespace WakeOnLan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const string Path = "Settings.ini";

        public MainWindow()
        {
            InitializeComponent();

            List<string> list = Settings.Load(Path);
            if (list.Count == 0)
            {
                list.Add("");
                list.Add("");
                Settings.Save(Path, list);
            }

            textIP.Text = list[0];
            textMAC.Text = list[1];
        }

        private void ButtonWakeClick(object sender, RoutedEventArgs e)
        {
            Logic.Send(Logic.GetMagicPacket(textMAC.Text), textIP.Text);
        }

        private void buttonSleep_Click(object sender, RoutedEventArgs e)
        {
            Logic.Send(Logic.GetAntiMagicPacket(textMAC.Text), textIP.Text);
        }

        private void textIP_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            List<string> list = Settings.Load(Path);
            list[0] = textIP.Text;
            Settings.Save(Path, list);
        }

        private void textMAC_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            List<string> list = Settings.Load(Path);
            list[1] = textMAC.Text;
            Settings.Save(Path, list);
        }
    }
}
