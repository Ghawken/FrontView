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

//   This file is from XBMC EventServer samples.

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Remote.XBMC.Matrix.Api
{
    public enum IconType
    {
        IconNone = 0x00,
        IconJpeg = 0x01,
        IconPng = 0x02,
        IconGif = 0x03
    }

    [Flags]
    public enum ButtonTypes
    {
        BtnUseName = 0x01,
        BtnDown = 0x02,
        BtnUp = 0x04,
        BtnUseAmount = 0x08,
        BtnQueue = 0x10,
        BtnNoRepeat = 0x20,
        BtnVkey = 0x40,
        BtnAxis = 0x80
    }

    public enum MouseType
    {
        MSNone = 0x00,
        MSAbsolute = 0x01
    }

    public enum LogType
    {
        Logdebug = 0,
        Loginfo = 1,
        Lognotice = 2,
        Logwarning = 3,
        Logerror = 4,
        Logsevere = 5,
        Logfatal = 6,
        Lognone = 7
    }

    public enum ActionType
    {
        ActionNone = 0x00,
        ActionExecbuiltin = 0x01,
        ActionButton = 0x02
    }

    public class XbmcEventClient : IDisposable
    {
        /************************************************************************/
        /* Written by Peter Tribe aka EqUiNox (TeamBlackbolt)                   */
        /* Based upon XBMC's xbmcclient.cpp class                               */
        /************************************************************************/
       

        private enum PacketType
        {
            PtHelo = 0x01,
            PtBye = 0x02,
            PtButton = 0x03,
            PtMouse = 0x04,
            PtPing = 0x05,
            //PtBroadcast = 0x06,  //Currently not implemented
            PtNotification = 0x07,
            PtBlob = 0x08,
            PtLog = 0x09,
            PtAction = 0x0A
            //,PtDebug = 0xFF //Currently not implemented
        }

        private const int StdPort = 9777;
        private const int MaxPacketSize = 1024;
        private const int HeaderSize = 32;
        private const int MaxPayloadSize = MaxPacketSize - HeaderSize;
        private const byte MajorVersion = 2;
        private const byte MinorVersion = 0;
        private uint _uniqueToken;
        private Socket _socket;

        public bool Connect(string address)
        {
            return Connect(address, StdPort, (uint)DateTime.Now.TimeOfDay.Milliseconds);
        }

        public bool Connect(string address, int port)
        {
            return Connect(address, port, (uint)DateTime.Now.TimeOfDay.Milliseconds);
        }

        public bool Connect(string address, int port, uint uniqueToken)
        {
            
            try
            {
                if (_socket != null) Disconnect();
                _uniqueToken = uniqueToken;
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                var ipHostEntry = Dns.GetHostEntry(address);
                foreach (var ipAddress in ipHostEntry.AddressList)
                {
                    if (ipAddress.AddressFamily != AddressFamily.InterNetwork) continue;
                    _socket.Connect(ipAddress.ToString(), port);
                    
                    return true;
                }
            }
            catch (SocketException) {}
            return false;
        }

        public bool Connected
        {
            get
            {
                return _socket != null && _socket.Connected;
            }
        }

        public void Disconnect()
        {
            try
            {
                if (_socket != null)
                {
                    _socket.Shutdown(SocketShutdown.Both);
                    _socket.Close();
                    _socket = null;
                }
            }
            catch (SocketException) { }
        }

        private byte[] Header(PacketType packetType, int numberOfPackets, int currentPacket, int payloadSize)
        {
            var header = new byte[HeaderSize];
            header[0] = (byte)'X';
            header[1] = (byte)'B';
            header[2] = (byte)'M';
            header[3] = (byte)'C';
            header[4] = MajorVersion;
            header[5] = MinorVersion;
            if (currentPacket == 1)
            {
                header[6] = (byte)(((ushort)packetType & 0xff00) >> 8);
                header[7] = (byte)((ushort)packetType & 0x00ff);
            }
            else
            {
                header[6] = ((ushort)PacketType.PtBlob & 0xff00) >> 8;
                header[7] = (ushort)PacketType.PtBlob & 0x00ff;
            }
            header[8] = (byte)((currentPacket & 0xff000000) >> 24);
            header[9] = (byte)((currentPacket & 0x00ff0000) >> 16);
            header[10] = (byte)((currentPacket & 0x0000ff00) >> 8);
            header[11] = (byte)(currentPacket & 0x000000ff);
            header[12] = (byte)((numberOfPackets & 0xff000000) >> 24);
            header[13] = (byte)((numberOfPackets & 0x00ff0000) >> 16);
            header[14] = (byte)((numberOfPackets & 0x0000ff00) >> 8);
            header[15] = (byte)(numberOfPackets & 0x000000ff);
            header[16] = (byte)((payloadSize & 0xff00) >> 8);
            header[17] = (byte)(payloadSize & 0x00ff);
            header[18] = (byte)((_uniqueToken & 0xff000000) >> 24);
            header[19] = (byte)((_uniqueToken & 0x00ff0000) >> 16);
            header[20] = (byte)((_uniqueToken & 0x0000ff00) >> 8);
            header[21] = (byte)(_uniqueToken & 0x000000ff);
            return header;
        }

        private bool Send(PacketType packetType, byte[] payload)
        {
            if (!Connected)
                return false;
            try
            {
                var successfull = true;
                var packetCount = (payload.Length / MaxPayloadSize) + 1;
                var bytesSent = 0;
                var bytesLeft = payload.Length;
                for (var package = 1; package <= packetCount; package++)
                {
                    int bytesToSend;
                    if (bytesLeft > MaxPayloadSize)
                    {
                        bytesToSend = MaxPayloadSize;
                        bytesLeft -= bytesToSend;
                    }
                    else
                    {
                        bytesToSend = bytesLeft;
                        bytesLeft = 0;
                    }
                    var header = Header(packetType, packetCount, package, bytesToSend);
                    var packet = new byte[MaxPacketSize];
                    Array.Copy(header, 0, packet, 0, header.Length);
                    Array.Copy(payload, bytesSent, packet, header.Length, bytesToSend);
                    var sendSize = _socket.Send(packet, header.Length + bytesToSend, SocketFlags.None);
                    if (sendSize != (header.Length + bytesToSend))
                    {
                        successfull = false;
                        break;
                    }
                    bytesSent += bytesToSend;
                }
                return successfull;
            }
            catch (SocketException)
            {
                return false;
            }
        }

        /************************************************************************/
        /* SendHelo - Payload format                                            */
        /* %s -  device name (max 128 chars)                                    */
        /* %c -  icontype ( 0=>NOICON, 1=>JPEG , 2=>PNG , 3=>GIF )              */
        /* %s -  my port ( 0=>not listening )                                   */
        /* %d -  reserved1 ( 0 )                                                */
        /* %d -  reserved2 ( 0 )                                                */
        /* XX -  imagedata ( can span multiple packets )                        */
        /************************************************************************/
        public bool SendHelo(string devName, IconType iconType, string iconFile)
        {
            if (!Connected)
                return false;
            if (devName == null)
                return false;
            var icon = new byte[0];
            if (iconType != IconType.IconNone)
            {
                if (File.Exists(iconFile))
                    icon = File.ReadAllBytes(iconFile);
            }
            var payload = new byte[devName.Length + 12 + icon.Length];
            var offset = 0;
            foreach (var t in devName)
                payload[offset++] = (byte)t;
            payload[offset++] = (byte)'\0';
            payload[offset++] = (byte)iconType;
            payload[offset++] = 0;
            payload[offset++] = (byte)'\0';
            for (var i = 0; i < 8; i++)
                payload[offset++] = 0;
            Array.Copy(icon, 0, payload, devName.Length + 12, icon.Length);

            return Send(PacketType.PtHelo, payload);
        }

        public bool SendHelo(string devName)
        {
            return SendHelo(devName, IconType.IconNone, "");
        }

        /************************************************************************/
        /* SendNotification - Payload format                                    */
        /* %s - caption                                                         */
        /* %s - message                                                         */
        /* %c - icontype ( 0=>NOICON, 1=>JPEG , 2=>PNG , 3=>GIF )               */
        /* %d - reserved ( 0 )                                                  */
        /* XX - imagedata ( can span multiple packets )                         */
        /************************************************************************/
        public bool SendNotification(string caption, string message, IconType iconType, string iconFile)
        {
            if (!Connected)
                return false;
            if (message == null || caption == null)
                return false;
            var icon = new byte[0];
            if (iconType != IconType.IconNone)
                icon = File.ReadAllBytes(iconFile);
            var payload = new byte[caption.Length + message.Length + 7 + icon.Length];
            var offset = 0;
            foreach (var t in caption)
                payload[offset++] = (byte)t;
            payload[offset++] = (byte)'\0';
            foreach (var t in message)
                payload[offset++] = (byte)t;
            payload[offset++] = (byte)'\0';
            payload[offset++] = (byte)iconType;
            for (var i = 0; i < 4; i++)
                payload[offset++] = 0;
            Array.Copy(icon, 0, payload, caption.Length + message.Length + 7, icon.Length);

            return Send(PacketType.PtNotification, payload);
        }

        public bool SendNotification(string caption, string message)
        {
            return SendNotification(caption, message, IconType.IconNone, "");
        }

        /************************************************************************/
        /* SendButton - Payload format                                          */
        /* %i - button code                                                     */
        /* %i - flags 0x01 => use button map/name instead of code               */
        /*            0x02 => btn down                                          */
        /*            0x04 => btn up                                            */
        /*            0x08 => use amount                                        */
        /*            0x10 => queue event                                       */
        /*            0x20 => do not repeat                                     */
        /*            0x40 => virtual key                                       */
        /*            0x80 => axis key                                          */
        /* %i - amount ( 0 => 65k maps to -1 => 1 )                             */
        /* %s - device map (case sensitive and required if flags & 0x01)        */
        /*      "KB" - Standard keyboard map                                    */
        /*      "XG" - Xbox Gamepad                                             */
        /*      "R1" - Xbox Remote                                              */
        /*      "R2" - Xbox Universal Remote                                    */
        /*      "LI:devicename" -  valid LIRC device map where 'devicename'     */
        /*                         is the actual name of the LIRC device        */
        /*      "JS<num>:joyname" -  valid Joystick device map where            */
        /*                           'joyname'  is the name specified in        */
        /*                           the keymap. JS only supports button code   */
        /*                           and not button name currently (!0x01).     */
        /* %s - button name (required if flags & 0x01)                          */
        /************************************************************************/
        private bool SendButton(string button, ushort buttonCode, string deviceMap, ButtonTypes flags, short amount)
        {
            if (!Connected)
                return false;
            if (button.Length != 0)
            {
                if ((flags & ButtonTypes.BtnUseName) == 0)
                    flags |= ButtonTypes.BtnUseName;
                buttonCode = 0;
            }
            else
                button = "";
            if (amount > 0)
            {
                if ((flags & ButtonTypes.BtnUseAmount) == 0)
                    flags |= ButtonTypes.BtnUseAmount;
            }
            if ((flags & ButtonTypes.BtnDown) == 0 || (flags & ButtonTypes.BtnUp) == 0)
                flags |= ButtonTypes.BtnDown;
            var payload = new byte[button.Length + deviceMap.Length + 8];
            var offset = 0;
            payload[offset++] = (byte)((buttonCode & 0xff00) >> 8);
            payload[offset++] = (byte)(buttonCode & 0x00ff);
            payload[offset++] = (byte)(((ushort)flags & 0xff00) >> 8);
            payload[offset++] = (byte)((ushort)flags & 0x00ff);
            payload[offset++] = (byte)((amount & 0xff00) >> 8);
            payload[offset++] = (byte)(amount & 0x00ff);
            foreach (var t in deviceMap)
                payload[offset++] = (byte)t;
            payload[offset++] = (byte)'\0';
            foreach (var t in button)
                payload[offset++] = (byte)t;
            payload[offset] = (byte)'\0';

            return Send(PacketType.PtButton, payload);
        }

        public bool SendButton(string button, string deviceMap, ButtonTypes bType, short amount)
        {
            return SendButton(button, 0, deviceMap, bType, amount);
        }

        public bool SendButton(string button, string deviceMap, ButtonTypes bType)
        {
            return SendButton(button, 0, deviceMap, bType, 0);
        }

        public bool SendButton(ushort buttonCode, string deviceMap, ButtonTypes bType, short amount)
        {
            return SendButton("", buttonCode, deviceMap, bType, amount);
        }

        public bool SendButton(ushort buttonCode, string deviceMap, ButtonTypes bType)
        {
            return SendButton("", buttonCode, deviceMap, bType, 0);
        }

        public bool SendButton(ushort buttonCode, ButtonTypes bType, short amount)
        {
            return SendButton("", buttonCode, "", bType, amount);
        }

        public bool SendButton(ushort buttonCode, ButtonTypes bType)
        {
            return SendButton("", buttonCode, "", bType, 0);
        }

        public bool SendButton()
        {
            return SendButton("", 0, "", ButtonTypes.BtnUp, 0);
        }

        /************************************************************************/
        /* SendPing - No payload                                                */
        /************************************************************************/
        public bool SendPing()
        {
            if (!Connected)
                return false;
            var payload = new byte[0];
            return Send(PacketType.PtPing, payload);
        }

        /************************************************************************/
        /* SendBye - No payload                                                 */
        /************************************************************************/
        public bool SendBye()
        {
            if (!Connected)
                return false;
            var payload = new byte[0];
            return Send(PacketType.PtBye, payload);
        }

        /************************************************************************/
        /* SendMouse - Payload format                                           */
        /* %c - flags                                                           */
        /*    - 0x01 absolute position                                          */
        /* %i - mousex (0-65535 => maps to screen width)                        */
        /* %i - mousey (0-65535 => maps to screen height)                       */
        /************************************************************************/
        public bool SendMouse(ushort x, ushort y, MouseType mType)
        {
            if (!Connected)
                return false;
            var payload = new byte[9];
            var offset = 0;
            payload[offset++] = (byte)mType;
            payload[offset++] = (byte)((x & 0xff00) >> 8);
            payload[offset++] = (byte)(x & 0x00ff);
            payload[offset++] = (byte)((y & 0xff00) >> 8);
            payload[offset] = (byte)(y & 0x00ff);

            return Send(PacketType.PtMouse, payload);
        }

        public bool SendMouse(ushort x, ushort y)
        {
            return SendMouse(x, y, MouseType.MSAbsolute);
        }

        /************************************************************************/
        /* SendLog - Payload format                                             */
        /* %c - log type                                                        */
        /* %s - message                                                         */
        /************************************************************************/
        public bool SendLog(LogType logLevel, string message)
        {
            if (!Connected)
                return false;
            if (message == null)
                return false;
            var payload = new byte[message.Length + 2];
            var offset = 0;
            payload[offset++] = (byte)logLevel;
            foreach (var t in message)
                payload[offset++] = (byte)t;
            payload[offset] = (byte)'\0';

            return Send(PacketType.PtLog, payload);
        }

        /************************************************************************/
        /* SendAction - Payload format                                          */
        /* %c - action type                                                     */
        /* %s - action message                                                  */
        /************************************************************************/
        public bool SendAction(ActionType action, string message)
        {
            if (!Connected)
                return false;
            if (message == null)
                return false;
            var payload = new byte[message.Length + 2];
            var offset = 0;
            payload[offset++] = (byte)action;
            foreach (var t in message)
                payload[offset++] = (byte)t;
            payload[offset] = (byte)'\0';

            return Send(PacketType.PtAction, payload);
        }

        public bool SendAction(string message)
        {
            return message != null && SendAction(ActionType.ActionExecbuiltin, message);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Disconnect();
            }
        }
    }
}


