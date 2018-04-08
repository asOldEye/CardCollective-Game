namespace ConnectionProtocol
{
    //диктуется сервером клиенту
    class ConnectionInfo
    {
        public int HeartRate { get; }
        public int HeartRateTimeout { get; }

        public ConnectionInfo(int heartrate, int heartrateTimeout)
        {
            HeartRate = heartrate;
            HeartRateTimeout = heartrateTimeout;
        }
    }
}
