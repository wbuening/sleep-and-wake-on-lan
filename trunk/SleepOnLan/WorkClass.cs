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
        public bool ComboBoxIsChecked { get; set; }

        public void DoWork(object obj)
        {
            // Get list of this PS's mac addresses.
            List<string> addresses = MacAddress();
            // Get antimagic packets for all mac addresses.
            List<byte[]> antiPackets = addresses.Select(GetAntiMagicPacket).ToList();

            udpClient = new UdpClient(Port);
            var ep = new IPEndPoint(IPAddress.Any, Port);

            while (true)
            {
                // Wait incoming data.
                byte[] dataFull = udpClient.Receive(ref ep);

                var data = new byte[dataFull.Length - 1];

                for (int i = 0; i < dataFull.Length - 2; i++ )
                {
                    data[i] = dataFull[i];
                }

                var action = dataFull[dataFull.Length - 1];

                // Compare all values of 2 arrays. If any "antimagic" packet contains our mac address
                if (antiPackets.Any(packet => packet.SequenceEqual(data)))
                {
                    // do action.
                    DoAction(action);
                }
            }
        }

        /// <summary>
        /// If we close our program receive function will be still active. It will prevent program from closing so we should close udpclient first.
        /// </summary>
        public void CloseConnection()
        {
            udpClient.Close();
        }

        // Do some actions on the assumption of comboBox selected index.
        private void DoAction(byte action)
        {
            if (ComboBoxIsChecked)
            {
                State = action;
            }

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

        /// <summary>
        /// Get list of all PS's mac addresses.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// "Antimagic" packet here is the magic packet with reversed mac address bytes.
        /// </summary>
        /// <param name="macAddress"></param>
        /// <returns>Magic packet</returns>
        static byte[] GetAntiMagicPacket(string macAddress)
        {
            var arr = new List<byte>(102);

            string[] macs = macAddress.Split(' ', ':', '-');

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
