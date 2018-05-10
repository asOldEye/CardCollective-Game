using System;
using ConnectionProtocol;
using AuxiliaryLibrary;

namespace Server
{
    class Seance<API> where API : AuxiliaryLibrary.API
    {
        API api;
        public Seance(Connection connection, API api)
        {
            connection.OnObjectReceived += OnReceived;
            connection.OnIncomingStream += OnIncomingStream;
            connection.OnDisconnected += OnDisconnected;

            api.Disconnect = connection.Disconnect;
            api.SendObject = connection.Send;
            api.SendStream = connection.Send;
            api.ReplaceAPI = ReplaceAPI;
        }

        void OnDisconnected(Connection sender)
        { if (api != null) api.OnDisconnected(); }
        void OnReceived(Connection sender, object obj)
        { api.OnReceived(obj); }
        void OnIncomingStream(Connection sender, StreamInfo info)
        { api.OnIncomingStream(info); }

        public void ReplaceAPI(AuxiliaryLibrary.API newAPI)
        {
            if (newAPI == null) throw new ArgumentNullException("Null new API");
            api.OnSessionContinued(newAPI);
            api = (API)newAPI;
        }
    }
}