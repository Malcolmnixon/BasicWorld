using BasicWorld.NetGame;

namespace BasicWorld.LanGame
{
    public class LanServerWorld : NetServerWorld
    {
        /// <summary>
        /// Discovery provider
        /// </summary>
        private readonly NetDiscovery.IProvider _discoveryProvider;

        /// <summary>
        /// Discovery server
        /// </summary>
        private readonly NetDiscovery.IServer _discoveryServer;

        public LanServerWorld()
        {
            _discoveryProvider = new NetDiscovery.Udp.UdpProvider(49523);
            _discoveryServer = _discoveryProvider.CreateServer();
            _discoveryServer.Identity = "Basic World";
        }

        public override void Dispose()
        {
            // Dispose of the world
            base.Dispose();

            // Stop network communications
            _discoveryProvider.Dispose();
            _discoveryServer.Dispose();
        }

        public override void Start()
        {
            // Start the world
            base.Start();

            // Start networking
            _discoveryServer.Start();
            _discoveryProvider.Start();
        }
    }
}
