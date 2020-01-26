using System.Net;
using System.Text;
using BasicWorld.WorldData;
using BasicWorld.WorldRunner;
using Newtonsoft.Json;

namespace BasicWorld.LanGame
{
    public class LanClientWorld : RemoteWorld
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
        /// Age of player state
        /// </summary>
        private float _playerStateAge;

        public LanClientWorld(IPAddress server)
        {
            _server = server;
            _communicationsProvider = new NetComms.Tcp.TcpProvider(49524);
            _communicationsConnection = _communicationsProvider.CreateClient(_server);
            _communicationsConnection.Notification += OnServerNotification;
            _communicationsConnection.ConnectionDropped += OnServerConnectionDropped;
        }

        public override void Dispose()
        {
            _communicationsConnection.Dispose();
            base.Dispose();
        }

        public override void Start()
        {
            base.Start();
            _communicationsConnection.Start();
        }

        private void OnServerConnectionDropped(object sender, NetComms.ConnectionEventArgs e)
        {
            // TODO: Handle connection dropped
        }

        private void OnServerNotification(object sender, NetComms.NotificationEventArgs e)
        {
            // This should be the server reporting the entire world state
            var json = Encoding.ASCII.GetString(e.Notification);
            var state = JsonConvert.DeserializeObject<WorldState>(json);

            // TODO: Resolve differences in local and remote state of our player
            if (Player != null)
            {
                state.Players.RemoveAll(p => p.Guid == Player.Guid);
                state.Players.Add(Player);
            }

            // Update to the new state
            lock (WorldLock)
            {
                State = state;
            }
        }

        protected override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            // Skip if we have no local player to send
            if (Player == null)
                return;

            // Don't send player state if we sent less than 0.3 seconds ago
            _playerStateAge += deltaTime;
            if (_playerStateAge < 0.3f)
                return;

            // Encode local player to JSON
            var json = JsonConvert.SerializeObject(Player);
            var data = Encoding.ASCII.GetBytes(json);

            // Send notification to server
            _communicationsConnection.SendNotification(data);
            _playerStateAge = 0f;
        }
    }
}
