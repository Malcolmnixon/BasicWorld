using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BasicWorld
{
    public abstract class WorldRunner : IDisposable, IWorld
    {
        protected readonly LocalPlayer PlayerInstance = new LocalPlayer();

        protected readonly List<Monster> MonsterList = new List<Monster>();

        private readonly CancellationTokenSource _cancel = new CancellationTokenSource();

        private readonly Thread _thread;

        protected WorldRunner()
        {
            _thread = new Thread(Worker);
        }

        public ILocalPlayer Player => PlayerInstance;

        public abstract IList<IRemotePlayer> RemotePlayers { get; }

        public IList<IMonster> Monsters => MonsterList.OfType<IMonster>().ToList();

        public virtual void Dispose()
        {
            _cancel.Cancel();
            _thread.Join();
        }

        public virtual void Start()
        {
            var rand = new Random();
            for (var i = 0; i < 5; ++i)
            {
                var monster = new Monster
                {
                    Position = new Vec2(
                        (float) (rand.NextDouble() * 40 - 20),
                        (float) (rand.NextDouble() * 40 - 20)),
                    Speed = (float) ((rand.NextDouble() + 1) / 2)
                };
                MonsterList.Add(monster);
            }

            _thread.Start();
        }

        protected virtual void OnUpdate()
        {
        }

        private void Worker()
        {
            while (!_cancel.IsCancellationRequested)
            {
                // Wait 100ms
                _cancel.Token.WaitHandle.WaitOne(100);

                // Update monsters
                foreach (var m in MonsterList)
                {
                    var dir = (Player.Position - m.Position).Normalize();
                    m.Position += dir * m.Speed * 0.1f;
                }

                // Dispatch update
                OnUpdate();
            }
        }

        protected class LocalPlayer : ILocalPlayer
        {
            public Guid Guid { get; } = Guid.NewGuid();

            public Vec2 Position { get; set; }
        }

        protected class Monster : IMonster
        {
            public Guid Guid { get; } = Guid.NewGuid();

            public Vec2 Position { get; set; }

            public float Speed { get; set; } = 1.0f;
        }
    }
}
