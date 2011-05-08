using System;
using System.Windows;
using System.Windows.Forms;
using System.Threading;

namespace SleepOnLan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WorkClass _doWork = new WorkClass();
        private const string SettingsPath = "Settings.ini";
        private Thread workingThread;

        public MainWindow()
        {
            InitializeComponent();

            // This thread will be waiting "antimagic" packet.
            workingThread = new Thread(_doWork.DoWork);
            workingThread.Start();

            // Load the id of action which we will do.
            comboBox1.SelectedIndex = Settings.Load(SettingsPath);
        }

        private void comboBox1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _doWork.State = comboBox1.SelectedIndex;
            Settings.Save(SettingsPath, comboBox1.SelectedIndex.ToString());
        }

        private NotifyIcon TrayIcon = new NotifyIcon();

        private void CreateTrayIcon()
        {
            TrayIcon.Icon = System.Drawing.Icon.FromHandle(Properties.Resources.Icon.GetHicon()); // Set picture of icon.
            TrayIcon.Text = "Click to Show/Hide"; // Tooltip text on tray icon.

            TrayIcon.Click += delegate(object sender, EventArgs e)
            {
                if (((MouseEventArgs)e).Button == MouseButtons.Left)
                {
                    ShowHideMainWindow(sender, null);
                }
            };

            TrayIcon.Visible = true;
        }

        private void ShowHideMainWindow(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                Hide();
            }
            else
            {
                Show();
                WindowState = WindowState.Normal;
                Activate(); // Set focus to program window.
            }
        }
        
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            CreateTrayIcon(); // Create our tray icon.
            WindowState = WindowState.Minimized; // Minimize app on start.
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);

            if (WindowState == WindowState.Minimized)
            {
                Hide();
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _doWork.CloseConnection(); // Close UDP client.
            workingThread.Abort(); // Close child thread.
            TrayIcon.Dispose(); // Dispose tray icon.
            base.OnClosing(e); // Do original closing event.
        }
    }
}
