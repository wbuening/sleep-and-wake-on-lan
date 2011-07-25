using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace WakeOnLan
{
    public static class Logic
    {
        /// <summary>
        /// Send some data through the UDP protocol on port 9.
        /// </summary>
        /// <param name="packet">Input data.</param>
        /// <param name="IP">IP to send.</param>
        public static void Send(byte[] packet, string IP)
        {
            var udpClient = new UdpClient();
            udpClient.Send(packet, packet.Length, new IPEndPoint(IPAddress.Parse(IP), 9));
            udpClient.Close();
        }

        /// <summary>
        /// Create magic packet. You can read about it here
        /// http://en.wikipedia.org/wiki/Wake-on-LAN
        /// or here
        /// http://ru.wikipedia.org/wiki/Wake-on-LAN
        /// </summary>
        /// <param name="macAddress">mac address of remote PC.</param>
        /// <returns>Magic packet.</returns>
        public static byte[] GetMagicPacket(string macAddress)
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
                    arr.Add(Convert.ToByte(macs[i], 16));
                }
            }

            return arr.ToArray();
        }

        /// <summary>
        /// "Antimagic" packet here is the magic packet with reversed mac address segments.
        /// </summary>
        public static byte[] GetAntiMagicPacket(string s, byte action)
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

            arr.Add(action);

            return arr.ToArray();
        }
    }
}
