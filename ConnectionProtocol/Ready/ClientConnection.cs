using System.Threading.Tasks;
using System.Net;

namespace ConnectionProtocol
{
    /// <summary>
    /// Клиент - соединение
    /// </summary>
    public class ClientConnection : Connection
    {
        /// <param name="streamingSupported">Поддерживает ли потоковую передачу данных</param>
        public ClientConnection(bool streamingSupported) : base(streamingSupported)
        {
            connection = new System.Net.Sockets.TcpClient();
        }

        /// <summary>
        /// Установить соединение с конечной точкой
        /// </summary>
        /// <param name="address">Адрес конечной точки</param>
        /// <param name="port">Порт конечной точки</param>
        /// <returns></returns>
        public void Connect(IPAddress address, int port)
        {
            try
            {
                connection.Connect(address, port);
            }
            catch { throw; }
            OnConnect(connection.SendBufferSize, connection.ReceiveBufferSize);
        }
    }
}