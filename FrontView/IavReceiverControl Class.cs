using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FrontView.Classes;
using FrontView.Libs;
using Setup;

namespace FrontView
{
    public interface IAvReceiverControl
    {
        //  event EventHandler<string> SetVolumeEvent;
        //    event EventHandler<bool> SetOnOffEvent;

        //  IPAddress DiscoverAvReceiver();
        int VolumeUp();
        int VolumeDown();
        int QueryVolume();
        bool MuteUnmute();
        void Connect(IPAddress ip);
        void Disconnect();

        bool SocketConnected();
        void QueryOnOff();
        void TurnOn();
        void TurnOff();

        void QueryMute();

        int WhatisVolume();

        bool WhatisMute();
    }



    class StateObject
    {
        // Client socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 256;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }
    public class VSX1123 : IAvReceiverControl
    {

        //   public event EventHandler<string> SetVolumeEvent;
        //   public event EventHandler<bool> SetOnOffEvent;
        //private readonly FrontView.IAvReceiverControl _parent;
        public int RecVolume = 0;
        public bool Mute = true;
        private readonly ManualResetEvent connectDone = new ManualResetEvent(false);
        private readonly ManualResetEvent sendDone = new ManualResetEvent(false);
        private readonly ManualResetEvent receiveDone = new ManualResetEvent(false);
        private Socket _sck;
        private int _port = 23;

        //  private Utilities _utilities;

        public void Connect(IPAddress ip)
        {
            
            _sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(ip, _port);
            Connect(endPoint, _sck);
            
            Receive();
            
        }

        private void Connect(EndPoint remoteEP, Socket client)
        {


                client.BeginConnect(remoteEP,new AsyncCallback(ConnectCallback), client);

                connectDone.WaitOne();
                

        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.
                client.EndConnect(ar);


                Logger.Instance().LogDump("- IAVRECEIVER-", "Socket connected to {0}" + client.RemoteEndPoint.ToString());


                // Signal that the connection has been made.
                connectDone.Set();
            }
            catch (SocketException ex)
            {
                Logger.Instance().LogDump("- IAVRECEIVER-", "Socket Exception: " + ex);
               
                Disconnect();

            }
        }

        public void Disconnect()
        {
            if (_sck.Connected)
            {
                _sck.Disconnect(true);
            }
        }

        public bool SocketConnected()
        {
            bool part1 = _sck.Poll(1000, SelectMode.SelectRead);
            bool part2 = (_sck.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }

        private void Receive()
        {
            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = _sck;

            // Begin receiving the data from the remote device.
            _sck.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReceiveCallback), state);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            // Retrieve the state object and the client socket 
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.workSocket;
            // Read data from the remote device.
            int bytesRead = client.EndReceive(ar);
            if (bytesRead > 0)
            {
                string response = Encoding.ASCII.GetString(state.buffer, 0, bytesRead);
                Logger.Instance().LogDump("- IAVRECEIVER-", "Response: " + response,true);
                if (response.Contains("VOL"))
                {
                    string tobesearched = "VOL";
                    string code = response.Substring(response.IndexOf(tobesearched) + tobesearched.Length);

                    code = code.Substring(0, 3);

                    Logger.Instance().LogDump("- IAVRECEIVER-", "REC-Volume Set:" + code,true);

                    RecVolume = Convert.ToInt16(code);
                    
                    //SetVolumeEvent(this, response);
                }
                if (response.StartsWith("MUT"))
                {
                    if (response.StartsWith("MUT0"))
                    {
                        Mute = true;
                    }
                    if (response.StartsWith("MUT1"))
                    {
                        Mute = false;
                    }
                }

                // There might be more data, so store the data received so far.
                state.sb.Append(response);
                //  Get the rest of the data.
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            else
            {
                // All the data has arrived; put it in response.
                if (state.sb.Length > 1)
                {
                    string response = state.sb.ToString();
                    //   SetVolumeEvent(this, response);
                }
                // Signal that all bytes have been received.
                receiveDone.Set();
            }
        }

        private void SendMessage(string message)
        {
            byte[] buffer = Encoding.Default.GetBytes(message + "\r\n");

            _sck.BeginSend(buffer, 0, buffer.Length, SocketFlags.None,
        new AsyncCallback(SendCallback), _sck);
            Receive();
        }

        private void SendCallback(IAsyncResult ar)
        {
            // Retrieve the socket from the state object.
            Socket client = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.
            int bytesSent = client.EndSend(ar);
            Logger.Instance().LogDump("- IAVRECEIVER-", "Sent {0} bytes to server."+ bytesSent,true);

            // Signal that all bytes have been sent.
            sendDone.Set();
        }


        public int VolumeUp()
        {
            SendMessage("VU");
            return RecVolume;
        }

        public int VolumeDown()
        {
            SendMessage("VD");
            return RecVolume;
        }

        public int QueryVolume()
        {
            SendMessage("?V");
            return RecVolume;
        }

        public bool MuteUnmute()
        {
            SendMessage("MZ");
            return true;
        }

        public void QueryMute()
        {
            SendMessage("?M");
        }


        public void QueryOnOff()
        {
            SendMessage("?P");

        }

        public void TurnOn()
        {
            SendMessage("PO");
        }

        public void TurnOff()
        {
            SendMessage("PF");
        }

        public int WhatisVolume()
        {
            return RecVolume;
        }

        public bool WhatisMute()
        {
            return Mute;
        }
    }
}
