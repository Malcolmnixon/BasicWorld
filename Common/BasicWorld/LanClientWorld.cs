using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
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

        public LanClientWorld(IPAddress server)
        {
            _server = server;
            _communicationsProvider = new NetComms.Tcp.TcpProvider(49524);
            _communicationsConnection = _communicationsProvider.CreateClient(_server);
            _communicationsConnection.Transaction += CommunicationsConnectionOnTransaction;
            _communicationsConnection.Notification += CommunicationsConnectionOnNotification;
            _communicationsConnection.ConnectionDropped += CommunicationsConnectionOnConnectionDropped;
        }

        public ILocalPlayer Player { get; }
        public IList<IRemotePlayer> RemotePlayers { get; }
        public IList<IMonster> Monsters { get; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            _communicationsConnection.Start();
        }

        private void CommunicationsConnectionOnConnectionDropped(object sender, ConnectionEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CommunicationsConnectionOnNotification(object sender, NotificationEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CommunicationsConnectionOnTransaction(object sender, TransactionEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
