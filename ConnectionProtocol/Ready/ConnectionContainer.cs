using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ConnectionProtocol
{
    /// <summary>
    /// Контейнер, в который упаковываются передаваемые объекты
    /// </summary>
    [Serializable]
    public struct ConnectionContainer
    {
        internal static readonly BinaryFormatter formatter = new BinaryFormatter();

        /// <summary>
        /// Содержимое контейнера
        /// </summary>
        public object Content { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content">помещаемый в контейнер объект</param>
        public ConnectionContainer(object content)
        {
            if (content == null) throw new ArgumentNullException("Null content");

            Content = content;

            try
            {
                Serialize(new MemoryStream(), this);
            }
            catch { throw new NotSupportedException("Content does'nt have [Serializable] attribute"); }
        }

        internal static void Serialize(Stream stream, ConnectionContainer obj)
        {
            try
            {
                formatter.Serialize(stream, obj);
            }
            catch { throw; }
        }
        internal static ConnectionContainer Deserialize(Stream stream)
        {
            try
            {
                return (ConnectionContainer)formatter.Deserialize(stream);
            }
            catch { throw; }
        }
    }
}