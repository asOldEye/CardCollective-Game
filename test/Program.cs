using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConnectionProtocol;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.InteropServices;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientConnection<ConnectionContainer<string>> connection = new ClientConnection<ConnectionContainer<string>>(false);
            

            connection.OnConnect += OnConnect;
            connection.OnDisconnect += OnDisconnect;

            connection.Connect(IPAddress.Loopback, 8888);

            connection.Send(new ConnectionContainer<string>("a"));
            Console.WriteLine("End");
            Console.ReadLine();
            connection.Disconnect();

            Console.Write("end");

            Console.ReadLine();
        }

        static void OnConnect(object o, EventArgs e)
        {
            Console.WriteLine("Connected");
        }
        static void OnDisconnect(object o, EventArgs e)
        {
            Console.WriteLine("Disconnected");
        }
    }
}
