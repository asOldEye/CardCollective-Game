using System;
using System.Runtime.Serialization;

namespace ConnectionProtocol
{
    [Serializable]
    public struct ConnectionContainer<T> : ISerializable
    {
        public Type Type { get; }
        public T Content { get; }

        public ConnectionContainer(T content)
        {
            if (content == null) throw new ArgumentNullException();

            Type = content.GetType();
            Content = content;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Content", Content, typeof(T));
            info.AddValue("Type", Type, typeof(Type));
        }
    }
}
