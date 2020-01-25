using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace BasicWorld
{
    /// <summary>
    /// LAN server discovery
    /// </summary>
    public class LanServerDiscovery : IDisposable
    {
        /// <summary>
        /// Discovery provider
        /// </summary>
        private readonly NetDiscovery.IProvider _discoveryProvider;

        /// <summary>
        /// Discovery client
        /// </summary>
        private readonly NetDiscovery.IClient _discoveryClient;

        /// <summary>
        /// Dictionary of servers
        /// </summary>
        private readonly Dictionary<IPAddress, string> _servers = new Dictionary<IPAddress, string>();

        public LanServerDiscovery()
        {
            _discoveryProvider = new NetDiscovery.Udp.UdpProvider(49523);
            _discoveryClient = _discoveryProvider.CreateClient();
            _discoveryClient.Discovery += DiscoveryClientOnDiscovery;
        }

        public List<Tuple<IPAddress, string>> Servers
        {
            get
            {
                lock (_servers)
                {
                    return _servers.Select(e => new Tuple<IPAddress, string>(e.Key, e.Value)).ToList();
                }
            }
        }

        public void Start()
        {
            _discoveryClient.Start();
            _discoveryProvider.Start();
        }

        public void Dispose()
        {
            _discoveryClient.Dispose();
            _discoveryProvider.Dispose();
        }

        private void DiscoveryClientOnDiscovery(object sender, NetDiscovery.DiscoveryEventArgs e)
        {
            lock (_servers)
            {
                _servers[e.Address] = e.Identity;
            }
        }
    }
}
