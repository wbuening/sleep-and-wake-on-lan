using System.Runtime.InteropServices;

namespace SleepOnLan
{
    public static class SleepClass
    {
        [DllImport("Powrprof.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);

        [DllImport("user32.dll")]
        public static extern void LockWorkStation();

        public static void Sleep()
        {
            // Standby
            SetSuspendState(false, true, true);
        }

        public static void Hibernate()
        {
            // Hibernate
            SetSuspendState(true, true, true);
        }

        public static void Reboot()
        {
            System.Diagnostics.Process.Start("shutdown", "/g /f");
        }

        public static void Shutdown()
        {
            System.Diagnostics.Process.Start("shutdown", "/s /f");
        }

        public static void Logoff()
        {
            System.Diagnostics.Process.Start("shutdown", "/l");
        }

        public static void Lock()
        {
            LockWorkStation();
        }
    }
}
