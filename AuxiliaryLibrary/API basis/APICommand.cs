using System;

namespace AuxiliaryLibrary
{
    [Serializable]
    public class APICommand
    {
        public string Command { get; }
        public object[] Params { get; set; }

        public APICommand(string command, object[] param)
        {
            if ((Command = command) == null)
                throw new ArgumentNullException("Null command");
            if ((Params = param) == null)
                throw new ArgumentNullException("Null param");
        }
    }
}