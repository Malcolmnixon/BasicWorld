using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BasicWorld
{
    public abstract class WorldRunner : IDisposable, IWorld
    {
        protected WorldState State { get; set; }

        private readonly CancellationTokenSource _cancel = new CancellationTokenSource();

        private readonly Thread _thread;

        protected WorldRunner()
        {
            Player = new Player {Guid = Guid.NewGuid()};

            State = new WorldState
            {
                Players = new List<Player> { Player },
                Monsters = new List<Monster>()
            };

            _thread = new Thread(Worker);
        }

        public Player Player { get; set; }

        public List<Player> RemotePlayers => State.Players.Where(p => p != Player).ToList();

        public List<Monster> Monsters => State.Monsters;

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
                    Guid = Guid.NewGuid(),
                    Position = new Vec2(
                        (float) (rand.NextDouble() * 40 - 20),
                        (float) (rand.NextDouble() * 40 - 20)),
                    Speed = (float) (rand.NextDouble()*4 + 1)
                };
                Monsters.Add(monster);
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
                _cancel.Token.WaitHandle.WaitOne(20);

                // Update state
                State.Tick(0.02f);

                // Dispatch update
                OnUpdate();
            }
        }
    }
}
