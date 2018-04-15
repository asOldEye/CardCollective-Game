using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ConnectionProtocol
{
    /// <summary>
    /// Соединение с конечной точкой
    /// </summary>
    public abstract class Connection
    {
        /// <summary>
        /// Потоковая передача данных
        /// </summary>
        public readonly Streaming Streaming;
        /// <summary>
        /// Соединен ли сейчас с конечной точкой
        /// </summary>
        public bool IsConnected
        { get { return connection == null ? false : connection.Connected; } }

        protected TcpClient connection;
        NetworkStream networkStream;

        byte[] sendBuffer, receiveBuffer;
        Task send, receive;

        /// <summary>
        /// Принятые объекты
        /// </summary>
        public Queue<ConnectionContainer> Received { get; } = new Queue<ConnectionContainer>();
        Queue<ConnectionContainer> sendObjects = new Queue<ConnectionContainer>();

        /// <param name="streamingSupported">Поддерживается ли потоковая передача</param>
        public Connection(bool streamingSupported)
        { if (streamingSupported) Streaming = new Streaming(this); }

        /// <summary>
        /// Отправить объект, обладающий аттрибутом Serializable
        /// </summary>
        /// <param name="obj">Объект, обладающий аттрибутом DataContract</param>
        public void Send(object obj)
        {
            if (connection == null || !connection.Connected) throw new WebException();
            try
            {
                sendObjects.Enqueue(new ConnectionContainer(obj));
            }
            catch { throw; }

            StartSendStream();
        }

        internal void StartSendStream()
        {
            if (send == null ||
                send.Status == TaskStatus.RanToCompletion ||
                send.Status == TaskStatus.Canceled ||
                send.Status == TaskStatus.Faulted)

                (send = new Task(new Action(Send))).Start();
        }
        internal void StartReceiveStream()
        {
            if (receive == null ||
                receive.Status == TaskStatus.RanToCompletion ||
                receive.Status == TaskStatus.Canceled ||
                receive.Status == TaskStatus.Faulted)

                (receive = new Task(new Action(Receive))).Start();
        }

        protected void OnConnect(int sendSize, int receiveSize)
        {
            networkStream = connection.GetStream();

            sendBuffer = new byte[sendSize];
            receiveBuffer = new byte[receiveSize];

            (receive = new Task(new Action(Receive))).Start();

            if (OnConnected != null)
                OnConnected.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Прервать соединение
        /// </summary>
        public void Disconnect()
        {
            sendObjects.Clear();
            Received.Clear();

            networkStream = null;
            connection.Close();

            if (OnDisconnected != null)
                OnDisconnected.Invoke(this, null);
        }























        #region Кишки
        void Receive()
        {
            try

            {

                while (connection.Connected)
                {
                    networkStream.Read(receiveBuffer, 0, 9);
                    byte[] head = new byte[9];
                    Array.Copy(receiveBuffer, head, 9);
                    Heading(out bool isStream, out long lenght, head);

                    if (isStream)
                    {
                        //do
                    }
                    else
                    {
                        using (Stream obj = new MemoryStream())
                        {
                            while (lenght > 0)
                            {
                                int readed = networkStream.Read(receiveBuffer, 0, (receiveBuffer.Length > lenght ? (int)lenght : receiveBuffer.Length));

                                obj.Write(receiveBuffer, 0, readed);

                                lenght -= readed;
                            }
                            obj.Position = 0;
                            ConnectionContainer container = ConnectionContainer.Deserialize(obj);

                            Received.Enqueue(container);

                            if (OnReceived != null)
                                OnReceived.Invoke(this, null);
                        }
                    }
                }
            }
            catch (IOException e) { if (e.InnerException is SocketException) Disconnect(); }
        }

        void Send()
        {
            if (Streaming != null)
                while (sendObjects.Count > 0 && Streaming.outcomingStreams.Count > 0)
                {
                    //do
                }
            else
                while (sendObjects.Count > 0)
                    SendObj(sendObjects.Dequeue());
        }

        byte[] Heading(bool isStream, long size)
        {
            var arr = new byte[9];

            var s = BitConverter.GetBytes(size);
            Array.Copy(s, 0, arr, 1, s.Length);

            arr[0] = (isStream ? byte.MaxValue : byte.MinValue);

            return arr;
        }
        void Heading(out bool isStream, out long size, byte[] heading)
        {
            size = BitConverter.ToInt64(heading, 1);
            isStream = (heading[0] == byte.MaxValue);
        }

        void SendObj(ConnectionContainer obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                ConnectionContainer.Serialize(stream, obj);

                var head = Heading(false, stream.Length);
                networkStream.Write(head, 0, head.Length);

                stream.Position = 0;
                int readed = 0;

                do
                {
                    readed = stream.Read(sendBuffer, 0, sendBuffer.Length);
                    networkStream.Write(sendBuffer, 0, readed);
                }
                while (readed > 0);
            }
        }

        void SendStream(Stream stream, StreamInfo streamInfo)
        {
            var head = Heading(true, sendBuffer.Length);

            SendObj(new ConnectionContainer(streamInfo));

            networkStream.Write(head, 0, head.Length);

            //do
        }
        #endregion кишки



















        #region IDisposable realization
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Disconnect();

                    if (receive != null)
                        receive.Dispose();
                    if (send != null)
                        send.Dispose();

                    connection.Dispose();

                    if (networkStream != null)
                        networkStream.Dispose();
                }
                disposed = true;
            }
        }

        ~Connection()
        { Dispose(false); }
        #endregion

        public event EventHandler OnConnected;
        public event EventHandler OnDisconnected;
        public event EventHandler OnReceived;
    }
}