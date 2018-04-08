using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ConnectionProtocol
{
    public interface IConnection<T> : IDisposable, IProgress<float>
    {
        bool IsConnected { get; }

        IPAddress EndPoint { get; }

        Stack<T> Received { get; }
        
        Stream ReceivingStream { get; set; }

        bool StreamingSupported { get; }

        bool StreamingReady { get; set; }

        void Send(T obj);

        void Send(Stream stream, StreamPriority priority);

        void Disconnect();

        #region Events
        event EventHandler OnConnect;
        event EventHandler OnDisconnect;

        event EventHandler OnReceived;
        event EventHandler OnStreamReceived;
        event EventHandler OnStreamSended;
        #endregion
    }
}