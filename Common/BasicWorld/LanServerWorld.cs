namespace BasicWorld
{
    public class LanServerWorld : WorldRunner
    {
        /// <summary>
        /// Discovery provider
        /// </summary>
        private readonly NetDiscovery.IProvider _discoveryProvider;

        /// <summary>
        /// Discovery server
        /// </summary>
        private readonly NetDiscovery.IServer _discoveryServer;

        /// <summary>
        /// Communications provider
        /// </summary>
        private readonly NetComms.IProvider _communicationsProvider;

        /// <summary>
        /// Communications server
        /// </summary>
        private readonly NetComms.IServer _communicationsServer;

        public LanServerWorld()
        {
            _discoveryProvider = new NetDiscovery.Udp.UdpProvider(49523);
            _discoveryServer = _discoveryProvider.CreateServer();
            _discoveryServer.Identity = "Basic World";

            _communicationsProvider = new NetComms.Tcp.TcpProvider(49524);
            _communicationsServer = _communicationsProvider.CreateServer();
            _communicationsServer.NewConnection += OnNewClientConnection;
            _communicationsServer.ConnectionDropped += OnClientConnectionDropped;
            _communicationsServer.Notification += OnClientNotification;
            _communicationsServer.Transaction += OnClientTransaction;
        }

        public override void Dispose()
        {
            base.Dispose();

            _discoveryProvider.Dispose();
            _discoveryServer.Dispose();
            _communicationsServer.Dispose();
        }

        public override void Start()
        {
            // Start networking
            _discoveryServer.Start();
            _discoveryProvider.Start();
            _communicationsServer.Start();

            base.Start();
        }

        private void OnClientTransaction(object sender, NetComms.TransactionEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void OnClientNotification(object sender, NetComms.NotificationEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void OnClientConnectionDropped(object sender, NetComms.ConnectionEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void OnNewClientConnection(object sender, NetComms.ConnectionEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
