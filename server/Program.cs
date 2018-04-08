using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConnectionProtocol;

namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            int serverPort = 8888;
            TcpListener serverSocket = new TcpListener(IPAddress.Any, serverPort);
            TcpClient clientSocket = default(TcpClient);
            int counter = 0;

            serverSocket.Start();
            Console.WriteLine(" >> " + "Server Started");

            counter = 0;
            while (true)
            {
                counter += 1;
                clientSocket = serverSocket.AcceptTcpClient();
                Console.WriteLine(" >> " + "Client No:" + Convert.ToString(counter) + " started!");
                handleClinet client = new handleClinet();
                client.startClient(clientSocket, Convert.ToString(counter));
            }

            serverSocket.Stop();
            Console.WriteLine(" >> " + "exit");
            Console.ReadLine();
        }


        //Class to handle each client request separatly
        public class handleClinet
        {
            TcpClient clientSocket;
            string clNo;
            public void startClient(TcpClient inClientSocket, string clineNo)
            {
                this.clientSocket = inClientSocket;
                this.clNo = clineNo;
                Thread ctThread = new Thread(doChat);
                ctThread.Start();
            }
            private void doChat()
            {
                int requestCount = 0;
                byte[] bytesFrom = new byte[128];
                string dataFromClient = null;
                Byte[] sendBytes = null;
                string serverResponse = null;
                string rCount = null;
                requestCount = 0;
                NetworkStream networkStream = clientSocket.GetStream();
                bool processFlag = true;

                try
                {
                    while (processFlag)
                    {
                        try
                        {
                            requestCount = requestCount + 1;

                            int readBytes = networkStream.Read(bytesFrom, 0, bytesFrom.Length);

                            if (readBytes != 0)
                            {
                                Console.WriteLine(" >> " + "From client BYTES-" + readBytes + "   " + requestCount);

                                //BinaryFormatter formatter = new BinaryFormatter();

                                //var n = formatter.Deserialize(new MemoryStream(readBytes));

                                //Console.WriteLine("Type is +" + n.GetType());
                            }
                            else
                            {
                                Console.WriteLine(" >> " + "client-" + clNo + " disconnected");
                                processFlag = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            processFlag = false;
                            Console.WriteLine(" >> " + ex.ToString());
                        }
                    }
                }
                finally
                {
                    clientSocket.Close();
                }
            }
        }
    }
}
