using System.Collections.Generic;
using AuxiliaryLibrary;
using System;
using SimpleDatabase;

namespace CardCollectiveEnvironment
{
    public sealed class LoginningAPI : API
    {
        Database database;

        protected override Dictionary<string, Action<APICommand>> ApiCommands
        { get; set; }

        public LoginningAPI(Database database)
        {
            if ((this.database = database) == null)
                throw new ArgumentNullException("Null database");

            ApiCommands = new Dictionary<string, Action<APICommand>>
            {
                { "Login", new Action<object>(Login) },
                { "Register", new Action<object>(Register) }
            };
        }

        public override void OnDisconnected() { }

        void Login(object command)
        {
            LoginningForm f = LoginningForm(command);
            if (f != null)
            {
                if (database.Exists(null, f.Login + "+" + f.Password))
                {
                    var obj = database.ReadObject(null, f.Login + "+" + f.Password);
                    if (obj is PlayerInfo)
                    {
                        SendObject.Invoke(new APIAnswer(command as APICommand, obj as PlayerInfo));
                        //TODO создать другой апи
                    }
                }
                var objs = database.Find(null, f.Login + "+");

                if (objs.Length == 0) WrongCommandAnswer(command, "Wrong login");
                else
                {
                    foreach (var obj in objs)
                    {

                    }
                }
                WrongCommandAnswer(command, "Wrong login or password");
            }
        }
        void Register(object command)
        {
            LoginningForm f = LoginningForm(command);
            if (f != null)
            {
                //TODO
            }
        }

        LoginningForm LoginningForm(object command)
        {
            var f = (command as APICommand).Params;
            if (f.Length == 1 && (f[0] is LoginningForm))
                return ((command as APICommand).Params[0] as LoginningForm);
            else WrongCommandAnswer(command as APICommand, "Wrong params count, must be one LoginningForm");
            return null;
        }
    }
}