using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ConnectionProtocol
{
    /// <summary>
    /// Считывает новые входящие соединения
    /// </summary>
    public class ConnectionProvider : IDisposable
    {
        List<ServerConnection> connections;
        TcpListener listener;
        bool allowNewConnections = false;
        Task listening;

        /// <summary>
        /// Разрешена ли функция стриминга для соединений
        /// </summary>
        public bool StreamingAvialable
        { get; set; }
        /// <summary>
        /// Разрешается ли создавать новые соединения
        /// </summary>
        public bool AllowNewConnections
        {
            get { return allowNewConnections; }
            set
            {
                if (allowNewConnections = value)
                    StartListening();
            }
        }

        /// <summary>
        /// Порт, на котором ведется прослушивание
        /// </summary>
        public int Port
        { get; }
        /// <summary>
        /// Локальный адрес прослушивания
        /// </summary>
        public IPAddress LocalAddr
        { get; }

        /// <summary>
        /// Максимальное количество соединений
        /// </summary>
        public int MaxConnections
        { get; protected set; }

        /// <summary>
        /// Количество соединений
        /// </summary>
        public int ConnectionsCount
        { get { return connections.Count; } }

        /// <param name="localaddr">Локальный адрес прослушивания</param>
        /// <param name="port">Порт, на котором ведется прослушивание</param>
        /// <param name="maxConnections">Максимальное количество соединений</param>
        /// <param name="connReceiveBufferSize">Размер буфера приема для соединений</param>
        /// <param name="connSendBufferSize">Размер буфера отправки для соединений</param>
        /// <param name="streamingAvialable">Разрешена ли функция стриминга для соединений</param>
        public ConnectionProvider(IPAddress localaddr, int port, int maxConnections, bool streamingAvialable)
        {
            try
            { listener = new TcpListener(IPAddress.Any, port); }
            catch { throw; }

            if ((MaxConnections = maxConnections) < 1) throw new ArgumentException("Wrong max connections count");

            StreamingAvialable = streamingAvialable;

            connections = new List<ServerConnection>();
        }

        void Listen()
        {
            listener.Start();
            while (allowNewConnections)
            {
                if (connections.Count == MaxConnections)
                {
                    if(OnMaxConnections != null)
                        OnMaxConnections.Invoke(this, null);
                    listener.Stop();
                    return;
                }
                else
                {
                    var connection = new ServerConnection(listener.AcceptTcpClient(), StreamingAvialable);
                    connection.OnDisconnected += OnDisconnected;
                    connections.Add(connection);

                    if (OnIncomingConnection != null)
                        OnIncomingConnection.Invoke(this, new ConnectionEventArgs(connection));
                }
            }
        }
        protected void StartListening()
        {
            if (listening == null ||
                listening.Status == TaskStatus.RanToCompletion ||
                listening.Status == TaskStatus.Canceled ||
                listening.Status == TaskStatus.Faulted)

                (listening = new Task(new Action(Listen))).Start();
        }

        void OnDisconnected(object connection, EventArgs args)
        {
            connections.Remove(connection as ServerConnection);
            if ((connections.Count == MaxConnections - 1))
                StartListening();
        }

        public event EventHandler OnIncomingConnection;
        public event EventHandler OnMaxConnections;

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
                    foreach (var f in connections)
                    { f.Dispose(); }
                }
                disposed = true;
            }
        }

        ~ConnectionProvider()
        { Dispose(false); }
        #endregion
    }
}