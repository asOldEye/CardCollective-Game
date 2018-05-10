using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace AuxiliaryLibrary
{
    public abstract class API
    {
        protected abstract Dictionary<string, Action<APICommand>> ApiCommands { get; set; }

        public Action Disconnect { get; set; }
        public Action<object> SendObject { get; set; }
        public Action<Stream, StreamInfo> SendStream { get; set; }
        public Action<API> ReplaceAPI { get; set; }

        protected void WrongCommandAnswer(object command, string message)
        {
            var comm = (command as APICommand);
            SendObject.Invoke(new APIAnswer(comm, null, new ArgumentException(message)));
        }

        public void Perform(APICommand command)
        {
            if (command == null) throw new ArgumentNullException("Null command");

            if (ApiCommands.TryGetValue(command.Command, out Action<APICommand> method))
            {
                Task t = Task.Run(() => method(command));
            }
            else WrongCommandAnswer(command, "Wrong API command name");
        
        }

        public abstract void OnDisconnected();
        public virtual void OnSessionEnded()
        { Disconnect.Invoke(); }

        public virtual void OnReceived(object obj)
        {
            if (obj is APICommand)
            {
                Perform(obj as APICommand);
                return;
            }
        }
        public virtual void OnIncomingStream(StreamInfo info)
        {
            WrongCommandAnswer(new APICommand("SendStream", null), "Send stream are'nt supported by this API");
            Disconnect.Invoke();
        }

        public virtual void OnSessionContinued(API other)
        {
            other.Disconnect = Disconnect;
            other.SendObject = SendObject;
            other.SendStream = SendStream;
            other.ReplaceAPI = ReplaceAPI;
        }
    }
}