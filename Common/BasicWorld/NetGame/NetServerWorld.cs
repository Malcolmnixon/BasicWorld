using System;
using System.Collections.Generic;
using System.Text;
using BasicWorld.WorldData;
using BasicWorld.WorldRunner;
using Newtonsoft.Json;

namespace BasicWorld.NetGame
{
    /// <summary>
    /// Network server world
    /// </summary>
    public class NetServerWorld : LocalWorld
    {
        /// <summary>
        /// Communications server
        /// </summary>
        private readonly NetComms.IServer _communicationsServer;

        /// <summary>
        /// Age of the world state
        /// </summary>
        private float _worldStateAge;

        public NetServerWorld()
        {
            //var provider = new NetComms.Tcp.TcpProvider(49524);
            var provider = new NetComms.Udp.UdpProvider(49524);
            _communicationsServer = provider.CreateServer();
            _communicationsServer.ConnectionDropped += OnClientConnectionDropped;
            _communicationsServer.Notification += OnClientNotification;
            _communicationsServer.NewConnection += (s, e) =>
            {
                System.Diagnostics.Trace.WriteLine("New connection");
            };
        }

        public override void Dispose()
        {
            // Dispose of the world
            base.Dispose();

            // Stop network communications
            _communicationsServer.Dispose();
        }

        public override void Start()
        {
            // Start the world
            base.Start();

            // Start networking
            _communicationsServer.Start();
        }

        protected override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            // Only send world state every 0.1 seconds
            _worldStateAge += deltaTime;
            if (_worldStateAge < 0.1f)
                return;

            // Send world state to all clients
            var json = State.ToJson();
            var body = Encoding.ASCII.GetBytes(json);
            _communicationsServer.SendNotification(body);
        }

        private void OnClientNotification(object sender, NetComms.NotificationEventArgs e)
        {
            try
            {
                // This should be a network player giving their information
                var json = Encoding.ASCII.GetString(e.Notification);
                var player = JsonConvert.DeserializeObject<Player>(json);

                // Get the connection player-Guid
                var oldGuid = (Guid?) e.Connection.AssociatedData ?? Guid.Empty;

                // Lock while integrating player
                lock (WorldLock)
                {
                    // Remove the player with the previous Guid
                    State.Players.RemoveAll(p => p.Guid == oldGuid);

                    // Add the new player
                    State.Players.Add(player);

                    // Save the GUID if the most recently populated player
                    e.Connection.AssociatedData = player.Guid;
                }
            }
            catch
            {
                // Ignore all errors as we don't want the server to fail when given junk
            }
        }

        private void OnClientConnectionDropped(object sender, NetComms.ConnectionEventArgs e)
        {
            // Get the connection player-Guid
            var oldGuid = (Guid?)e.Connection.AssociatedData ?? Guid.Empty;

            // Lock while updating game state
            lock (WorldLock)
            {
                // Remove the player from the game state
                State.Players.RemoveAll(p => p.Guid == oldGuid);
            }
        }
    }
}
