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
using System.ComponentModel;

namespace ConnectionProtocol
{
    public enum StreamPriority
    { low, auto, hight }

    [Serializable]
    public class ClientConnection<T> : IConnection<T> where T : ISerializable
    {
        public bool IsConnected
        { get { return connection == null ? false : connection.Connected; } }
        public IPAddress EndPoint
        { get; protected set; }

        static BinaryFormatter formatter = new BinaryFormatter();

        TcpClient connection;
        NetworkStream networkStream;

        byte[] sendBuffer, receiveBuffer;

        CancellationTokenSource cts;
        Task send, receive;

        public Stack<T> Received { get; } = new Stack<T>();
        Stack<ConnectionContainer<T>> sendObjects = new Stack<ConnectionContainer<T>>();

        Stack<KeyValuePair<Stream, StreamPriority>> sendStreams;

        Stream receivingStream;
        public Stream ReceivingStream
        {
            get { return receivingStream; }
            set
            {
                if (value == null) throw new ArgumentNullException();
                if (!value.CanWrite) throw new NotSupportedException();

                receivingStream = value;
            }
        }

        public bool StreamingSupported
        { get; }
        public bool StreamingReady
        { get; set; } = false;

        public ClientConnection(bool streamingSupported)
        {
            connection = new TcpClient();
            cts = new CancellationTokenSource();

            if (StreamingSupported = streamingSupported)
            { sendStreams = new Stack<KeyValuePair<Stream, StreamPriority>>(); }
        }

        public void Send(T obj)
        {
            if (connection == null || !connection.Connected) throw new Exception();
            if (obj == null) throw new ArgumentNullException();

            sendObjects.Push(new ConnectionContainer<T>(obj));

            if (send == null ||
                send.Status == TaskStatus.RanToCompletion ||
                send.Status == TaskStatus.Canceled ||
                send.Status == TaskStatus.Faulted)

                (send = new Task(new Action(Send))).Start();
        }
        public void Send(Stream stream, StreamPriority priority)
        {
            if (stream == null) throw new ArgumentNullException();
            if (!StreamingSupported) throw new NotSupportedException();
            if (!stream.CanRead) throw new NotSupportedException();

            sendStreams.Push(new KeyValuePair<Stream, StreamPriority>(stream, priority));

            if (send == null ||
                send.Status == TaskStatus.RanToCompletion ||
                send.Status == TaskStatus.Canceled ||
                send.Status == TaskStatus.Faulted)

                (send = new Task(new Action(Send))).Start();
        }

        public async Task Connect(IPAddress endPoint, int recieverPort)
        {
            if (connection.Connected) throw new Exception("Already connected");

            await connection.ConnectAsync(endPoint, recieverPort);

            if (connection.Connected)
            {
                EndPoint = endPoint;

                networkStream = connection.GetStream();

                sendBuffer = new byte[connection.SendBufferSize];
                receiveBuffer = new byte[connection.ReceiveBufferSize];

                (receive = new Task(new Action(Receive))).Start();

                if (OnConnect != null)
                    OnConnect.Invoke(this, new EventArgs());
            }
        }

        public void Disconnect()
        {
            if (!connection.Connected) throw new Exception("Not Connected");

            cts.Cancel();

            EndPoint = null;

            sendObjects.Clear();
            Received.Clear();

            networkStream = null;
            connection.Close();

            if (OnDisconnect != null)
                OnDisconnect.Invoke(this, new EventArgs());
        }

        #region кишки

        void Receive()
        {
            int lenght = -1;
            try
            {
                while (connection.Connected && !cts.IsCancellationRequested)
                {
                    networkStream.Read(receiveBuffer, 0, 10);


                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(IOException))
                {
                    Console.WriteLine("Disconnected");
                }
            }
            finally
            {

            }
        }

        void Send()
        {
            if (connection == null || !connection.Connected) return;

            if (StreamingSupported)
            {
                while (sendStreams.Count > 0 || sendObjects.Count > 0)
                {
                    if (sendStreams.Count > 0)
                    {
                        switch (sendStreams.Peek().Value)
                        {
                            case StreamPriority.hight:

                                break;
                            case StreamPriority.auto:

                                break;
                            default:

                                break;
                        }
                    }
                    else
                    {
                        if (connection == null || !connection.Connected || cts.IsCancellationRequested)
                            return;
                        if (SendObj(sendObjects.Peek(), false))
                            sendObjects.Pop();
                    }
                }
            }
            else
            {
                while (sendObjects.Count > 0)
                {
                    if (connection == null || !connection.Connected || cts.IsCancellationRequested)
                        return;
                    if (SendObj(sendObjects.Peek(), false))
                        sendObjects.Pop();
                }
            }
        }

        bool SendObj(ConnectionContainer<T> obj, bool isService)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj as ISerializable);

                return SendStream(stream, isService, false);
            }
        }
        bool SendStream(Stream s, bool isService, bool isStream)
        {
            var head = Heading(isService, isStream, s.Length);

            if (!connection.Connected || cts.IsCancellationRequested) return false;

            networkStream.Write(head, 0, head.Length);

            int readed = 0;
            s.Position = 0;

            do
            {
                if (!connection.Connected || cts.IsCancellationRequested) return false;

                readed = s.Read(sendBuffer, 0, sendBuffer.Length);
                networkStream.Write(sendBuffer, 0, readed);
            }
            while (readed > 0);

            return true;
        }

        public byte[] Heading(bool isService, bool isStream, long size)
        {
            var arr = new byte[10];

            var s = BitConverter.GetBytes(size);
            Array.Copy(s, 0, arr, 2, s.Length);

            arr[0] = (isService ? byte.MaxValue : byte.MinValue);
            arr[1] = (isStream ? byte.MaxValue : byte.MinValue);

            return arr;
        }
        public void Heading(out bool isService, out bool isStream, out long size, byte[] heading)
        {
            size = BitConverter.ToInt64(heading, 2);

            isStream = (heading[1] == byte.MaxValue);
            isService = (heading[0] == byte.MaxValue);
        }

        #endregion кишки

        #region IProgress
        //do
        public void Report(float value)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IDisposable realization
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        bool disposed = false;
        //do
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (connection.Connected)
                        Disconnect();
                    cts.Dispose();

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

        ~ClientConnection()
        { Dispose(false); }
        #endregion

        #region Events
        public event EventHandler OnConnect;
        public event EventHandler OnDisconnect;

        public event EventHandler OnReceived;
        public event EventHandler OnStreamReceived;
        public event EventHandler OnStreamSended;
        #endregion
    }
}

