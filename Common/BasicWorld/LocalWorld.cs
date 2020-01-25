using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BasicWorld
{
    public class LocalWorld : IDisposable, IWorld
    {
        private readonly LocalPlayer _player = new LocalPlayer();

        private readonly List<Monster> _monsters = new List<Monster>();

        private readonly CancellationTokenSource _cancel = new CancellationTokenSource();

        private readonly Thread _thread;

        public LocalWorld()
        {
            _thread = new Thread(Worker);
            _thread.Start();

            var rand = new Random();
            for (var i = 0; i < 5; ++i)
            {
                var monster = new Monster();
                monster.Position = new Vec2(
                    (float)(rand.NextDouble() * 40 - 20),
                    (float)(rand.NextDouble() * 40 - 20));
                monster.Speed = (float) ((rand.NextDouble() + 1) / 2);
                _monsters.Add(monster);
            }
        }

        public ILocalPlayer Player => _player;

        public IList<IRemotePlayer> RemotePlayers => new List<IRemotePlayer>();

        public IList<IMonster> Monsters => _monsters.OfType<IMonster>().ToList();

        public void Dispose()
        {
            _cancel.Cancel();
            _thread.Join();
        }

        private void Worker()
        {
            while (!_cancel.IsCancellationRequested)
            {
                // Wait 100ms
                _cancel.Token.WaitHandle.WaitOne(100);

                // Update monsters
                foreach (var m in _monsters)
                {
                    var dir = (_player.Position - m.Position).Normalize();
                    m.Position += dir * m.Speed * 0.1f;
                }
            }
        }


        private class LocalPlayer : ILocalPlayer
        {
            public Guid Guid { get; } = Guid.NewGuid();

            public Vec2 Position { get; set; }
        }

        private class Monster : IMonster
        {
            public Guid Guid { get; } = Guid.NewGuid();

            public Vec2 Position { get; set; }

            public float Speed { get; set; } = 1.0f;
        }
    }
}
