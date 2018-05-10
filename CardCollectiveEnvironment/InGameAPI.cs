using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuxiliaryLibrary;

namespace CardCollectiveEnvironment
{
    public class InGameAPI : API
    {
        Player player;

        protected override Dictionary<string, Action<APICommand>> ApiCommands { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        protected InGameAPI(PlayerInfo player)
        {
            if (player == null) throw new ArgumentNullException("Null player");
        }

        public override void OnDisconnected()
        {
            throw new NotImplementedException();
        }
        
        void Unlogin(object obj)
        {
            OnSessionEnded();
        }

        void StartSession(object obj)
        {

        }
        void MakeMove(object obj)
        {

        }
        void LeaveSession(object obj)
        {

        }
        void LoadSessions(object obj)
        {

        }

        void LoadPlayerInfo(object obj)
        {

        }

        void StartChat(object obj)
        {

        }
        void MakeMessageInChat(object obj)
        {

        }
        void LeaveChat(object obj)
        {

        }
    }
}