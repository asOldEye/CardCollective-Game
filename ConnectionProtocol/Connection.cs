using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace ConnectionProtocol
{
    public class Connection : IConnection
    {
        public Connection(IPAddress endpoint)
        {

        }

        public void Send(object obj)
        {
            if (obj == null) throw new ArgumentNullException();
            toSend.Push(obj);
        }

        protected Stack<object> toSend = new Stack<object>();

        public Stack<object> Recieved
        { get; }

        public bool Connected { get; }

        public IPAddress EndPoint { get; }

        public event EventHandler OnDisconnect;
        public event EventHandler OnConnect;

        public event EventHandler OnRecieve;
    }
}