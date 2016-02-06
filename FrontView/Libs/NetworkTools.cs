// ------------------------------------------------------------------------
//    YATSE 2 - A touch screen remote controller for XBMC (.NET 3.5)
//    Copyright (C) 2010  Tolriq (http://yatse.leetzone.org)
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
// ------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace FrontView.Libs
{
    public static class NetworkTools
    {
        private const int PingTimeout = 1000;

        internal static class NativeMethods
        {
            [DllImport("iphlpapi.dll", ExactSpelling = true)]
            public static extern int SendARP(int destIp, int srcIp, byte[] pMacAddr, ref uint phyAddrLen);
        }

        public static bool IsHostAccessible(string hostNameOrAddress)
        {
            if (String.IsNullOrEmpty(hostNameOrAddress))
                return false;
            using (var ping = new Ping())
            {
                PingReply reply;
                try
                {
                    reply = ping.Send(hostNameOrAddress, PingTimeout);
                }
                catch (Exception ex)
                {
                    if (ex is ArgumentNullException ||
                        ex is ArgumentOutOfRangeException ||
                        ex is InvalidOperationException ||
                        ex is SocketException )
                    {

                        return false;
                    }
                    throw;
                }
                if (reply != null) return reply.Status == IPStatus.Success;
            }
            return false;
        }

        public static string GetMacAddressFromArp(string hostNameOrAddress)
        {
            if (!IsHostAccessible(hostNameOrAddress)) return null;

            var hostEntry = Dns.GetHostAddresses(hostNameOrAddress);
            if (hostEntry.Length == 0)
                return null;

            var macAddr = new byte[6];
            var macAddrLen = (uint)macAddr.Length;

            var byteIp = hostEntry[0].GetAddressBytes();

            var ip = (uint)byteIp[3] << 24;
            ip += (uint)byteIp[2] << 16;
            ip += (uint)byteIp[1] << 8;
            ip += byteIp[0];

            if (NativeMethods.SendARP((int)ip, 0, macAddr, ref macAddrLen) != 0)
                return null;

            var macAddressString = new StringBuilder();
            foreach (var t in macAddr)
            {
                if (macAddressString.Length > 0)
                    macAddressString.Append(":");
                macAddressString.AppendFormat("{0:x2}", t);
            }
            return macAddressString.ToString();
        }

        public static void Wakeup(string macAddress)
        {
            if (macAddress == null)
                return;
            using (var client = new WolUdpClient())
            {
                client.Connect(new IPAddress(0xffffffff), 0x2fff);
                if (!client.IsClientInBrodcastMode()) return;

                var byteCount = 0;
                var bytes = new byte[102];
                for (var trailer = 0; trailer < 6; trailer++)
                {
                    bytes[byteCount++] = 0xFF;
                }
                for (var macPackets = 0; macPackets < 16; macPackets++)
                {
                    var i = 0;
                    for (var macBytes = 0; macBytes < 6; macBytes++)
                    {
                        bytes[byteCount++] = byte.Parse(macAddress.Substring(i, 2), NumberStyles.HexNumber,CultureInfo.InvariantCulture);
                        i += 2;
                    }
                }
                client.Send(bytes, byteCount);
            }
        }

        private class WolUdpClient : UdpClient
        {
            public bool IsClientInBrodcastMode()
            {
                var broadcast = false;
                if (Active)
                {
                    try
                    {
                        Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 0);
                        broadcast = true;
                    }
                    catch (Exception ex)
                    {
                        if (ex is ObjectDisposedException ||
                            ex is SocketException)
                        {

                            broadcast = false;
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                return broadcast;
            }
        }
    }
}
