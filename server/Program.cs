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
        static Database database = new Database("CardCollectiveUsersDatabase", Directory.GetCurrentDirectory());
        static Supervisor supervisor = new Supervisor(new TimeSpan(0, 5, 0));
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
            while (Console.ReadLine() != "stop") ;
        }

        static void OnIncomingConnection(ConnectionProvider provider, ServerConnection connection)
        {
            if (connection != null)
                new Seance(connection, new LoginningAPI(database, supervisor));
        }
        static void OnMaxConnections(ConnectionProvider provider)
        {
            var mem = GC.GetTotalMemory(false);
            if (mem < 2000000000)
                provider.MaxConnections = provider.MaxConnections * 125 / 100;
        }
        static void ReDrawUI(ConnectionProvider provider, int count)
        {
            Console.WriteLine("Connections count: [" + count + "]/[" + provider.MaxConnections + "]");
        }
    }
}