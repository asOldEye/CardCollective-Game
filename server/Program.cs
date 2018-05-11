using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConnectionProtocol;
using AuxiliaryLibrary;
using System.IO;
using CardCollectiveServerSide;
using SimpleDatabase;

namespace Server
{
    class Program
    {
        static Database database = new Database(Directory.GetCurrentDirectory());
        static ConnectionProvider provider;
        static void Main(string[] args)
        {
            Console.WriteLine("Server starting...");
            var connectionOpitions = new ConnectionOpitions(true, new TimeSpan(0, 0, 10), new TimeSpan(0, 0, 15));
            provider = new ConnectionProvider(IPAddress.Any, 8888, 10, connectionOpitions);
            provider.OnMaxConnections += OnMaxConnections;
            provider.OnConnectionsCountChanged += ReDrawUI;
            provider.OnIncomingConnection += OnIncomingConnection;
            provider.AllowNewConnections = true;

            Console.WriteLine("Server in work...");

            //string msg = "";
            //do
            //{
            //    Console.WriteLine("stop - Shut down all server's services [IN WORK]");
            //    Console.WriteLine("1 - Switch allow new incoming connections [" + provider.AllowNewConnections + "]");
            //    Console.WriteLine("2 - Switch allow new incoming connections [" + provider.ConnectionsCount + "/" + provider.MaxConnections + "]");

            //    Console.WriteLine(">>");
            //    msg = Console.ReadLine();

            //    string other;

            //    switch (msg)
            //    {
            //        case "1":
            //            Console.WriteLine(">>");
            //            other = Console.ReadLine();
            //            if (other == "true" && !provider.AllowNewConnections)
            //                provider.AllowNewConnections = true;
            //            else if (other == "false" && provider.AllowNewConnections)
            //                provider.AllowNewConnections = false;
            //            break;

            //        case "2":
            //            Console.WriteLine(">>");
            //            other = Console.ReadLine();

            //            try
            //            {

            //            }
            //            catch (Exception e)
            //            { Console.WriteLine(e.Message); }

            //            break;
            //    }

            //    msg = Console.ReadLine();
            //}
            //while (msg != "stop");

            //provider.AllowNewConnections = false;

            //Console.WriteLine("Server stopped");
            Console.ReadLine();
        }

        static void OnIncomingConnection(ConnectionProvider provider, ServerConnection connection)
        {
            if (connection != null)
                new Seance(connection, new LoginningAPI(database));
        }
        static void OnMaxConnections(ConnectionProvider provider)
        {
            var mem = GC.GetTotalMemory(false);
            if (mem < 2000000000)
                provider.MaxConnections = provider.MaxConnections / 100 * 125;
            if (mem < 3000000000)
                provider.MaxConnections = provider.MaxConnections / 100 * 110;
        }

        static void ReDrawUI(ConnectionProvider provider, int count)
        {
            Console.WriteLine("Connections count: [" + count + "]/[" + provider.MaxConnections + "]");
        }
    }
}