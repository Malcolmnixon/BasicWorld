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
        /// Dictionary of connections to the player GUID
        /// </summary>
        private readonly Dictionary<NetComms.IConnection, Guid> _playersByConnection = new Dictionary<NetComms.IConnection, Guid>();

        /// <summary>
        /// Age of the world state
        /// </summary>
        private float _worldStateAge;

        public NetServerWorld()
        {
            var provider = new NetComms.Tcp.TcpProvider(49524);
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

            // Only send world state every 0.3 seconds
            _worldStateAge += deltaTime;
            if (_worldStateAge < 0.3f)
                return;

            // Send world state to all clients
            var json = State.ToJson();
            var body = Encoding.ASCII.GetBytes(json);
            _communicationsServer.SendNotification(body);
        }

        private void OnClientNotification(object sender, NetComms.NotificationEventArgs e)
        {
            // This should be a network player giving their information
            var json = Encoding.ASCII.GetString(e.Notification);
            var player = JsonConvert.DeserializeObject<Player>(json);

            // Lock while integrating player
            lock (WorldLock)
            {
                // Process connection information
                if (!_playersByConnection.TryGetValue(e.Connection, out var lastGuid))
                {
                    // Connection has no associated player. Add this player
                    _playersByConnection[e.Connection] = player.Guid;

                    // Add the player to the game
                    State.Players.Add(player);
                }
                else if (lastGuid != player.Guid)
                {
                    // Player has changed. Update player associated with connection
                    _playersByConnection[e.Connection] = player.Guid;

                    // Remove the old player
                    State.Players.RemoveAll(p => p.Guid == lastGuid);
                    State.Players.Add(player);
                }
                else
                {
                    // Save updated player information
                    var idx = State.Players.FindIndex(p => p.Guid == player.Guid);
                    if (idx < 0)
                        throw new Exception("Player collection inconsistent");

                    // Update player
                    State.Players[idx] = player;
                }
            }
        }

        private void OnClientConnectionDropped(object sender, NetComms.ConnectionEventArgs e)
        {
            // Lock while updating game state
            lock (WorldLock)
            {
                // If the connection had no associated player then we're done
                if (!_playersByConnection.TryGetValue(e.Connection, out var guid))
                    return;

                // Remove the player from the dictionary
                _playersByConnection.Remove(e.Connection);

                // Remove the player from the game state
                State.Players.RemoveAll(p => p.Guid == guid);
            }
        }
    }
}
