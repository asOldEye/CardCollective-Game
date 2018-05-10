using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ConnectionProtocol;
using AuxiliaryLibrary;
using System.IO;

namespace clnt
{
    class Program
    {
        static ClientConnection connection;// = new ClientConnection();
        static void Main(string[] args)
        {
            connection = new ClientConnection();
            connection.OnDisconnected += OnDisconnect;
            connection.OnObjectReceived += OnRec;

            connection.Connect("127.0.0.1", 8888);

            while (true)
            {
                //Console.WriteLine(connection.Opitions.AverageDisconnectAvait.Seconds);
                connection.Send(BinarySerializer.Serialize(new Ass(Console.ReadLine())));
            }

        }

        static void OnDisconnect(Connection connection)
        {
            Console.WriteLine("DISCONNECTED");
        }
        static void OnRec(Connection connection, object obj)
        {
            Console.WriteLine((BinarySerializer.Deserialize(obj as Stream) as Ass).Mess);
        }
    }

    [Serializable]
    public class Ass
    {
        public string Mess { get; set;}

        public Ass(string mess)
        {
            this.Mess = mess;
        }
    }


}
