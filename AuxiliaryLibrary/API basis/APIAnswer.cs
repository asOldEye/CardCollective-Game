using System;

namespace AuxiliaryLibrary
{
    [Serializable]
    public class APIAnswer
    {
        public APIAnswer(APICommand command, object answer, Exception exception = null)
        {
            if ((Command = command) == null)
                throw new ArgumentNullException("Null command");
            if ((Answer = answer) == null)
                throw new ArgumentNullException("Null answer");
            Exception = exception;
        }

        public APICommand Command { get; }
        public object Answer { get; }

        public Exception Exception { get; }
    }
}