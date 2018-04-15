using System;
using System.Collections.Generic;
using System.IO;

namespace ConnectionProtocol
{
    /// <summary>
    /// Потоковая передача данных
    /// </summary>
    public class Streaming
    {
        private Connection owner;
        internal Queue<KeyValuePair<Stream, StreamInfo>> outcomingStreams = new Queue<KeyValuePair<Stream, StreamInfo>>();
        Stream receivingStream;
        internal long receivedSize = 0;
        internal List<byte> receivedBuffer = new List<byte>();
        internal long sendedSize = 0;
        
        internal Streaming(Connection owner)
        { this.owner = owner; }

        /// <summary>
        /// Отправить поток
        /// </summary>
        /// <param name="stream">Поток</param>
        /// <param name="streamInfo">Информация о потоке</param>
        public void Send(Stream stream, StreamInfo streamInfo)
        {
            if (stream == null) throw new ArgumentNullException();
            if (streamInfo == null) throw new ArgumentNullException();
            if (!stream.CanRead) throw new NotSupportedException();

            outcomingStreams.Enqueue(new KeyValuePair<Stream, StreamInfo>(stream, streamInfo));

            owner.StartSendStream();
        }

        /// <summary>
        /// Поток, в который записываются потоковые данные
        /// </summary>
        public Stream ReceivingStream
        {
            get { return receivingStream; }
            set
            {
                if (value == null) throw new ArgumentNullException();
                if (!value.CanWrite) throw new NotSupportedException();

                receivingStream = value;
            }
        }

        /// <summary>
        /// Информация о входящем потоке
        /// </summary>
        public StreamInfo ReceivingStreamInfo
        { get; internal set; }

        /// <summary>
        /// Прогресс получения
        /// </summary>
        /// <returns>Процентов получено, -1 если процесс не происходит</returns>
        public float ReceivingProgress
        {
            get
            {
                if (ReceivingStreamInfo != null)
                    return receivedSize / ReceivingStreamInfo.FullSize;
                return -1;
            }
        }
        /// <summary>
        /// Прогресс отсылки
        /// </summary>
        /// <returns>Процентов отослано, -1 если процесс не происходит</returns>
        public float SendingProgress
        {
            get
            {
                if (outcomingStreams.Count > 0)
                    return sendedSize / outcomingStreams.Peek().Value.FullSize;
                return -1;
            }
        }
        
        internal void OnStreamRecievedInvoke()
        {
            if (OnStreamReceived != null)
                OnStreamReceived.Invoke(this, null);
        }
        internal void OnStreamIncomingInvoke()
        {
            if (OnStreamIncoming != null)
                OnStreamIncoming.Invoke(this, null);
        }

        public event EventHandler OnStreamIncoming;
        public event EventHandler OnStreamReceived;
    }
}