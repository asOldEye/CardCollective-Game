using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ConnectionProtocol;
namespace testClient
{
    class Program
    {
        static int r = 0;
        static void Main(string[] args)
        {
            for (int i = 0; i < 12; i++)
                F(i);
        }

        static void F(int i)
        {
            Console.WriteLine("Started");
            ClientConnection connection = new ClientConnection(false);

            connection.OnConnected += OnConnected;
            connection.OnReceived += OnReceived;
            connection.OnDisconnected += OnDisconnected;
            connection.Connect(IPAddress.Parse("127.0.0.1"), 8888);
            Console.WriteLine("Connection");

            connection.Send(new Stringgg());
            Console.WriteLine("Sending");

            Console.ReadLine();
        }

        static void OnReceived(object sender, EventArgs eventArgs)
        {
            Console.WriteLine("NEW Received!   " + r++);
            var f = (sender as ClientConnection).Received.Dequeue();
            
            //(sender as ClientConnection).Send(f.Content);
        }
        static void OnConnected(object s, EventArgs e)
        {
            Console.WriteLine("Connected");
        }

        static void OnDisconnected(object s, EventArgs e)
        {
            Console.WriteLine("Disconnected");
        }

    }
    [Serializable]
    public class Stringgg
    {
        public string ass = "asss";
    }
    

}
