using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace FrontViewConsoleChange
{
    class Program
    {
        private static byte[] data;

        static void Main(string[] args)
        {

            String[] arguments = Environment.GetCommandLineArgs();
            Console.WriteLine("Arguments Given : " + arguments.Length);
            if (arguments.Length <= 1)
            {
                Console.WriteLine("Needs arguments: ");
                Environment.Exit(0);
            }
            string port = arguments[1];
            string action = arguments[2];
            string path = "";
            if (arguments.Length >= 4)
            {
                  path = arguments[3];
            }

            Send(Convert.ToInt32(port), action, path);

//Format Arguments
// FrontViewChangeConsole IPPort-FrontViewServer ON/OFF {path}
// eg. FrontViewChangeConsole 5000 ON c:\thepath\to\the\file
        }

        static void Send(int Port, string action, string path)
        {
            IPEndPoint RemoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port);
            Socket server = new Socket(AddressFamily.InterNetwork,
                                       SocketType.Dgram, ProtocolType.Udp);
            string welcome = "FrontViewConsoleCommand " + action.ToUpper() + "," + path;
            data = Encoding.ASCII.GetBytes(welcome);

            int count = 12;
            for (int i = 0; i < count; i++)
            {
                server.SendTo(data, data.Length, SocketFlags.None, RemoteEndPoint);
                System.Threading.Thread.Sleep(200);
            }
        }

    }
}
