using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConnectionProtocol;

namespace Server
{
    class Program
    {
        static ConnectionProvider provider;

        static void Main(string[] args)
        {
            provider = new ConnectionProvider(IPAddress.Any, 8888, 10, false);

            provider.OnIncomingConnection += OnIncomingConnection;
            provider.OnMaxConnections += OnMax;


            Console.WriteLine("ServerStarted");

            provider.AllowNewConnections = true;

            Console.WriteLine("Listening");

            Console.ReadLine();
        }


        static void OnIncomingConnection(object sender, EventArgs eventArgs)
        {
            var f = ((eventArgs as ConnectionEventArgs).obj as ServerConnection);
            Console.WriteLine("NEW CONNECTION!");
            f.OnReceived += OnReceived;
            f.OnDisconnected += OnDisconnected;
        }

        static void OnReceived(object sender, EventArgs eventArgs)
        {
            Console.WriteLine("NEW Received!   " + provider.ConnectionsCount);
            var f = (sender as ServerConnection).Received.Dequeue();
            
            //(sender as ServerConnection).Send(f.Content);
        }
        static void OnDisconnected(object s, EventArgs e)
        {
            Console.WriteLine("Disconnected");
            Console.WriteLine("Connections count = " + provider.ConnectionsCount);
        }
        static void OnMax(object s, EventArgs e)
        {
            Console.WriteLine("Max connections");
            Console.WriteLine("Connections count = " + provider.ConnectionsCount);
        }
    }
}
