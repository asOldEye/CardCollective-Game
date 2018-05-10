using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using AuxiliaryLibrary;

namespace ConnectionProtocol
{
    /// <summary>
    /// Соединение с конечной точкой
    /// </summary>
    public abstract class Connection
    {
        /// <summary>
        /// Соединен ли сейчас с конечной точкой
        /// </summary>
        public bool Connected
        {
            get
            {
                try
                { return connection.Connected; }
                catch { return false; }
            }
        }

        Queue<object> receivedObjects;
        public Queue<object> ReceivedObjects
        {
            get
            {
                if (Opitions.EventOriented)
                    throw new NotSupportedException("You can't use ReceivedObjects in event oriented connection");
                else return receivedObjects;
            }
        }

        protected TcpClient connection;
        NetworkStream networkStream;

        Task send, receive;

        Queue<Pair<Stream, StatusByte>> toSend = new Queue<Pair<Stream, StatusByte>>();
        Stream receiver;

        public Connection(ConnectionOpitions opitions)
        {
            if ((Opitions = opitions) == null) throw new ArgumentNullException("Null opitions");
            if (!Opitions.EventOriented)
                receivedObjects = new Queue<object>();
        }

        public ConnectionOpitions Opitions { get; }

        public void Send(Stream stream, StreamInfo info)
        {
            if (stream == null) throw new ArgumentNullException("Null stream");
            if (!stream.CanRead) throw new ArgumentException("Can't read stream");
            if (info == null) throw new ArgumentNullException("Null stream info");
            lock (toSend)
            {
                toSend.Enqueue(new Pair<Stream, StatusByte>(BinarySerializer.Serialize(info), StatusByte.streamInfo));
                toSend.Enqueue(new Pair<Stream, StatusByte>(stream, StatusByte.data));
            }
            StartSend();
        }
        public void Send(object obj)
        {
            if (obj == null) throw new ArgumentNullException("Null stream");

            Stream s;
            try
            {
                s = BinarySerializer.Serialize(obj);
            }
            catch (NotSupportedException) { throw; }

            toSend.Enqueue(new Pair<Stream, StatusByte>(s, obj is ConnectionOpitions ? StatusByte.connectionOpitions : (obj is StreamInfo ? StatusByte.streamInfo : StatusByte.data)));

            StartSend();
        }

        public void Receive(Stream destination)
        {
            if (destination == null) throw new ArgumentNullException("Null destionation stream");
            if (!destination.CanWrite) throw new ArgumentException("Can't write in destionation stream");
            receiver = destination;
        }

        protected void OnConnect()
        {
            networkStream = connection.GetStream();

            StartReceive();

            if (Opitions.EventOriented && OnConnected != null)
                OnConnected.Invoke(this);
        }
        public void Disconnect()
        {
            connection.Close();

            if (Opitions.EventOriented && OnDisconnected != null)
                OnDisconnected.Invoke(this);
        }

        #region Кишки
        void StartSend()
        {
            if (send == null || send.Status == TaskStatus.RanToCompletion)
                (send = new Task(new Action(Send))).Start();
        }
        void StartReceive()
        {
            if (receive == null ||
                receive.Status == TaskStatus.RanToCompletion ||
                receive.Status == TaskStatus.Canceled ||
                receive.Status == TaskStatus.Faulted)

                (receive = new Task(new Action(Receive))).Start();
        }

        void Receive()
        {
            byte[] receiveBuffer = new byte[connection.ReceiveBufferSize = Opitions.BufferSize],
                head = new byte[9];

            int readed;
            Stream destination = null;
            bool stream = false;

            try
            {
                while (connection.Connected)
                {
                    networkStream.Read(head, 0, head.Length);
                    Heading(out long size, out StatusByte statusByte, head);

                    if (stream)
                    {
                        if ((destination = receiver) == null)
                            while (size > 0)
                            {
                                readed = networkStream.Read(receiveBuffer, 0, receiveBuffer.Length);
                                size -= readed;
                            }
                    }
                    else destination = new MemoryStream();

                    while (size > 0)
                    {
                        readed = networkStream.Read(receiveBuffer, 0, receiveBuffer.Length);
                        destination.Write(receiveBuffer, 0, readed);
                        size -= readed;
                    }

                    if (stream) stream = false;
                    else
                    {
                        object obj = BinarySerializer.Deserialize(destination);
                        switch (statusByte)
                        {
                            case StatusByte.connectionOpitions:
                                if (OnConnectionOpitionsReceived != null)
                                    OnConnectionOpitionsReceived.BeginInvoke(this, obj as ConnectionOpitions, null, null);
                                break;
                            case StatusByte.streamInfo:
                                if (!Opitions.EventOriented)
                                    receivedObjects.Enqueue(obj);
                                else if (OnIncomingStream != null)
                                    OnIncomingStream.Invoke(this, obj as StreamInfo);
                                stream = true;
                                break;
                            case StatusByte.data:
                                if (!Opitions.EventOriented)
                                    receivedObjects.Enqueue(obj);
                                else if (OnObjectReceived != null)
                                    OnObjectReceived.BeginInvoke(this, obj, null, null);
                                break;
                        }
                    }
                }
            }
            catch (IOException e) { if (e.InnerException is SocketException) Disconnect(); }
            catch (System.Runtime.Serialization.SerializationException)
            {
                if (destination.Length == 0) Disconnect();
            }
        }

        void Send()
        {
            byte[] sendBuffer = new byte[connection.SendBufferSize = Opitions.BufferSize];
            try
            {
                while (toSend.Count > 0)
                {
                    var source = toSend.Peek();
                    var head = Heading(source.Obj1.Length, source.Obj2);

                    networkStream.Write(head, 0, head.Length);
                    source.Obj1.Position = 0;

                    while (source.Obj1.Position <= source.Obj1.Length - 1)
                    {
                        int readed = source.Obj1.Read(sendBuffer, 0, sendBuffer.Length);
                        networkStream.Write(sendBuffer, 0, readed);
                    }
                    toSend.Dequeue();
                }
            }
            catch (IOException e) { if (e.InnerException is SocketException) Disconnect(); }
        }

        byte[] Heading(long size, StatusByte statusByte)
        {
            var arr = new byte[9];
            var s = BitConverter.GetBytes(size);
            Array.Copy(s, arr, s.Length);
            arr[8] = (byte)statusByte;
            return arr;
        }
        void Heading(out long size, out StatusByte statusByte, byte[] heading)
        {
            size = BitConverter.ToInt64(heading, 0);
            statusByte = (StatusByte)heading[8];
        }

        enum StatusByte
        {
            streamInfo,
            data,
            connectionOpitions
        }
        #endregion

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

                    if (receive != null) receive.Dispose();
                    if (send != null) send.Dispose();

                    if (networkStream != null)
                        networkStream.Dispose();
                }
                disposed = true;
            }
        }

        ~Connection()
        { Dispose(false); }
        #endregion

        public event NonParametrizedEventHandler<Connection> OnConnected;
        public event NonParametrizedEventHandler<Connection> OnDisconnected;

        public event ParametrizedEventHandler<Connection, object> OnObjectReceived;
        protected event ParametrizedEventHandler<Connection, ConnectionOpitions> OnConnectionOpitionsReceived;

        public event ParametrizedEventHandler<Connection, StreamInfo> OnIncomingStream;
    }
}