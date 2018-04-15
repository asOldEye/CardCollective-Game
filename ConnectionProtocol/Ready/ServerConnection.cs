using System;
using System.Net.Sockets;

namespace ConnectionProtocol
{
    /// <summary>
    /// Принимающее соединение
    /// </summary>
    public class ServerConnection : Connection
    {
        /// <param name="client">Клиентское подключение</param>
        /// <param name="streamingSupported">Поддерживается ли потоковая передача данных</param>
        public ServerConnection(TcpClient client, bool streamingSupported) : base(streamingSupported)
        {
            if ((connection = client) == null) throw new ArgumentNullException("Null client");
            
            OnConnect(connection.SendBufferSize, connection.ReceiveBufferSize);
        }
    }
}
