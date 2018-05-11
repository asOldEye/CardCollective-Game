using System.Collections.Generic;
using AuxiliaryLibrary;
using System;
using SimpleDatabase;
using System.IO;

namespace CardCollectiveServerSide
{
    public sealed class LoginningAPI : API
    {
        Database database;

        public LoginningAPI(Database database)
        {
            if ((this.database = database) == null)
                throw new ArgumentNullException("Null database");

            InitializeAPICommands(new Dictionary<string, Action<APICommand>>()
            {
                {"Login", new Action<APICommand>(Login) },
                {"Register", new Action<APICommand>(Register) }
            });
        }

        protected void ContinueSession()
        {
            //TODO
            //base.ContinueSession();
        }

        public override void OnDisconnected() { }

        [APICommandAttr(new Type[] { typeof(LoginningForm) })]
        void Login(APICommand command)
        {
            var form = (command.Params[0] as LoginningForm);

            //database bla-bla-bla
        }
        [APICommandAttr(new Type[] { typeof(LoginningForm) })]
        void Register(APICommand command)
        {
            var form = (command.Params[0] as LoginningForm);

            //database bla-bla-bla
        }
    }
}