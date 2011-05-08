using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;

namespace SleepOnLan
{
    class WorkClass
    {
        public int State { get; set; }
        private int Port = 9;
        private UdpClient udpClient;

        public void DoWork(object obj)
        {
            List<string> addresses = MacAddress();
            List<byte[]> antiPackets = addresses.Select(GetAntiMagicPacket).ToList();

            udpClient = new UdpClient(Port);
            var ep = new IPEndPoint(IPAddress.Any, Port);

            while (true)
            {
                byte[] data = udpClient.Receive(ref ep);

                // If any "antimagic" packet contains our mac address
                if (antiPackets.Any(packet => packet.SequenceEqual(data) == true))
                {
                    // do action.
                    DoSleep();
                }
            }
        }

        public void CloseConnection()
        {
            udpClient.Close();
        }

        private void DoSleep()
        {
            if (State == 0)
            {
                SleepClass.Sleep();
            }
            else if (State == 1)
            {
                SleepClass.Hibernate();
            }
            else if (State == 2)
            {
                SleepClass.Reboot();
            }
            else if (State == 3)
            {
                SleepClass.Shutdown();
            }
            else if (State == 4)
            {
                SleepClass.Logoff();
            }
            else if (State == 5)
            {
                SleepClass.Lock();
            }
        }

        // Get list of all network adapters mac addresses.
        private List<string> MacAddress()
        {
            var addresses = new List<string>();

            var oMClass = new ManagementClass("Win32_NetworkAdapterConfiguration");

            ManagementObjectCollection colMObj = oMClass.GetInstances();

            foreach (ManagementObject objMO in colMObj)
            {
                try
                {
                    addresses.Add(objMO["MacAddress"].ToString());
                }
                catch (NullReferenceException e)
                {
                    //MessageBox.Show(e.Message);
                }
            }

            return addresses;
        }

        // "Antimagic" packet here is the magic packet with reversed mac address bytes.
        static byte[] GetAntiMagicPacket(string s)
        {
            var arr = new List<byte>(102);

            string[] macs = s.Split(' ', ':', '-');

            for (int i = 0; i < 6; i++)
            {
                arr.Add(0xff);
            }

            for (int j = 0; j < 16; j++)
            {
                for (int i = 0; i < 6; i++)
                {
                    arr.Add(Convert.ToByte(macs[5 - i], 16));
                }
            }

            return arr.ToArray();
        }
    }
}
