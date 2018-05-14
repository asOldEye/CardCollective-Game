using System.Collections.Generic;
using AuxiliaryLibrary;
using System;
using SimpleDatabase;
using CardCollectiveSharedSide;

namespace CardCollectiveServerSide
{
    public class LoginningAPI : API
    {
        Database database;
        Supervisor supervisor;

        public LoginningAPI(Database database, Supervisor supervisor)
        {
            if ((this.database = database) == null)
                throw new ArgumentNullException(nameof(database));
            if ((this.supervisor = supervisor) == null)
                throw new ArgumentNullException(nameof(supervisor));

            InitializeAPICommands(new Dictionary<string, Action<APICommand>>()
            {
                {"Login", new Action<APICommand>(Login) },
                {"Registration", new Action<APICommand>(Registration) }
            });
        }

        bool ContinueSession(PlayerInfo player)
        {
            var session = supervisor.GetPlayer(player);
            if (session == null) return false;
            ContinueSession(new InGameAPI(session, supervisor));
            return true;
        }

        public override void OnDisconnected() { }

        #region API methods
        [APICommandAttr(new Type[] { typeof(LoginningForm) })]
        void Login(APICommand command)
        {
            var form = (command.Params[0] as LoginningForm);
            object player = null;
            try
            { player = database.ReadObject(form.Login); }
            catch
            {
                SendObject(new APIAnswer(command, null, new ArgumentException("Wrong login")));
                return;
            }
            if ((player is PlayerInfo))
                if ((player as PlayerInfo).Password == form.Password)
                {
                    if (ContinueSession(player as PlayerInfo))
                        SendObject(new APIAnswer(command, (player as PlayerInfo).SharedPlayerInfo));
                    else
                        SendObject(new APIAnswer(command, null, new ArgumentException("Already in game")));
                }
                else
                    SendObject(new APIAnswer(command, null, new ArgumentException("Wrong password")));
            else throw new ArgumentException("Wrong database answer");
        }
        [APICommandAttr(new Type[] { typeof(LoginningForm) })]
        void Registration(APICommand command)
        {
            var form = (command.Params[0] as LoginningForm);

            if (database.Exists(form.Login))
                SendObject(new APIAnswer(command, null, new ArgumentException("Login in use")));
            else
            {
                var player = new PlayerInfo(new CardCollectiveSharedSide.PlayerInfo()
                { Name = form.Login }, form.Password);

                database.WriteObject(form.Login, player);

                ContinueSession(player);
            }
        }
        #endregion
    }
}