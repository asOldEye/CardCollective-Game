using System;
using System.Collections.Generic;
using System.Net;

namespace ConnectionProtocol
{
    public interface IConnection
    {
        Stack<object> Recieved
        { get; }

        void Send(object obj);

        event EventHandler OnDisconnect;
        event EventHandler OnConnect;

        bool Connected
        { get; }

        IPAddress EndPoint
        { get; }
    }
}