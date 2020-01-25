using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NetComms;

namespace BasicWorld
{
    public class LanClientWorld : IDisposable, IWorld
    {
        /// <summary>
        /// Server address
        /// </summary>
        private readonly IPAddress _server;

        /// <summary>
        /// Communications provider
        /// </summary>
        private readonly NetComms.IProvider _communicationsProvider;

        /// <summary>
        /// Communications connection
        /// </summary>
        private readonly NetComms.IConnection _communicationsConnection;

        /// <summary>
        /// World state
        /// </summary>
        private WorldState _state = new WorldState();

        public LanClientWorld(IPAddress server)
        {
            _server = server;
            _communicationsProvider = new NetComms.Tcp.TcpProvider(49524);
            _communicationsConnection = _communicationsProvider.CreateClient(_server);
            _communicationsConnection.Transaction += CommunicationsConnectionOnTransaction;
            _communicationsConnection.Notification += CommunicationsConnectionOnNotification;
            _communicationsConnection.ConnectionDropped += CommunicationsConnectionOnConnectionDropped;
        }

        public Player Player { get; set; }

        public List<Player> RemotePlayers => _state.Players.Where(p => p != Player).ToList();

        public List<Monster> Monsters => _state.Monsters;

        public void Dispose()
        {
            _communicationsConnection.Dispose();
        }

        public void Start()
        {
            _communicationsConnection.Start();
        }

        private void CommunicationsConnectionOnConnectionDropped(object sender, ConnectionEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void CommunicationsConnectionOnNotification(object sender, NotificationEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void CommunicationsConnectionOnTransaction(object sender, TransactionEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
