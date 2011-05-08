using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace WakeOnLan
{
    public static class Logic
    {
        public static void Send(byte[] packet, string text)
        {
            var udpClient = new UdpClient();
            udpClient.Send(packet, packet.Length, new IPEndPoint(IPAddress.Parse(text), 9));
            udpClient.Close();
        }

        public static byte[] GetMagicPacket(string s)
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
                    arr.Add(Convert.ToByte(macs[i], 16));
                }
            }

            return arr.ToArray();
        }

        // "Antimagic" packet here is the magic packet with reversed mac address bytes.
        public static byte[] GetAntiMagicPacket(string s)
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
