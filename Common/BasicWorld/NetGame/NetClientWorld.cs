using System.Net;
using System.Text;
using BasicWorld.WorldData;
using BasicWorld.WorldRunner;
using Newtonsoft.Json;

namespace BasicWorld.NetGame
{
    /// <summary>
    /// Network client world
    /// </summary>
    public class NetClientWorld : RemoteWorld
    {
        /// <summary>
        /// Communications connection
        /// </summary>
        private readonly NetComms.IConnection _communicationsConnection;

        /// <summary>
        /// Age of player state
        /// </summary>
        private float _playerStateAge;

        public NetClientWorld(IPAddress server)
        {
            //var provider = new NetComms.Tcp.TcpProvider(49524);
            var provider = new NetComms.Udp.UdpProvider(49524);
            _communicationsConnection = provider.CreateClient(server);
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

            UpdateWorld(state);
        }

        protected override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            // Skip if we have no local player to send
            if (Player == null)
                return;

            // Don't send player state if we sent less than 0.1 seconds ago
            _playerStateAge += deltaTime;
            if (_playerStateAge < 0.1f)
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
