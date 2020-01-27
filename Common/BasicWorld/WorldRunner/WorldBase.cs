using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using BasicWorld.WorldData;

namespace BasicWorld.WorldRunner
{
    /// <summary>
    /// Base class for world
    /// </summary>
    public class WorldBase : IDisposable, IWorld
    {
        /// <summary>
        /// Empty players collection
        /// </summary>
        private static readonly ReadOnlyCollection<Player> EmptyPlayers = new List<Player>().AsReadOnly();

        /// <summary>
        /// Cancellation token to stop world
        /// </summary>
        private readonly CancellationTokenSource _cancel = new CancellationTokenSource();

        /// <summary>
        /// Stopwatch for time measurement
        /// </summary>
        private readonly Stopwatch _time = Stopwatch.StartNew();

        /// <summary>
        /// World update thread
        /// </summary>
        private readonly Thread _thread;

        /// <summary>
        /// Last update in milliseconds
        /// </summary>
        private long _lastUpdate;

        /// <summary>
        /// Constructor
        /// </summary>
        protected WorldBase()
        {
            // Create thread
            _thread = new Thread(ThreadProc);
        }

        /// <summary>
        /// Lock for accessing world state
        /// </summary>
        protected object WorldLock { get; } = new object();

        /// <summary>
        /// Gets or sets the world state
        /// </summary>
        public WorldState State { get; set; } = new WorldState();

        /// <summary>
        /// Gets or sets the current player
        /// </summary>
        public Player Player { get; set; }
        
        /// <summary>
        /// Gets the remote players (those players who aren't the local player)
        /// </summary>
        public ReadOnlyCollection<Player> RemotePlayers => State.Players?.Where(p => p != Player).ToList().AsReadOnly() ?? EmptyPlayers;

        /// <summary>
        /// Gets the monsters
        /// </summary>
        public ReadOnlyCollection<Monster> Monsters => State.Monsters.AsReadOnly();

        /// <summary>
        /// Disposes of the world
        /// </summary>
        public virtual void Dispose()
        {
            if (!_thread.IsAlive)
                return;

            _cancel.Cancel();
            _thread.Join();
        }

        /// <summary>
        /// Start the world
        /// </summary>
        public virtual void Start()
        {
            _time.Restart();
            _lastUpdate = 0;
            _thread.Start();
        }

        public virtual void Update()
        {
            // Fire the tick function
            FireTick();
        }

        /// <summary>
        /// Create local player in world
        /// </summary>
        /// <returns>Local player</returns>
        public Player CreateLocalPlayer()
        {
            lock (WorldLock)
            {
                if (Player == null)
                {
                    Player = new Player { Guid = Guid.NewGuid() };
                    State.Players.Add(Player);
                }

                return Player;
            }
        }

        /// <summary>
        /// Handle world ticks
        /// </summary>
        protected virtual void Tick(float deltaTime)
        {
        }

        private void FireTick()
        {
            lock (WorldLock)
            {
                // Track time
                var now = _time.ElapsedMilliseconds;
                var deltaTime = (now - _lastUpdate) * 0.001f;
                _lastUpdate = now;

                // Fire the tick function with the delta-time and the world locked for update
                Tick(deltaTime);
            }
        }

        /// <summary>
        /// World thread procedure
        /// </summary>
        private void ThreadProc()
        {
            // Loop until asked to cancel
            while (!_cancel.IsCancellationRequested)
            {
                // Wait for around 50ms
                _cancel.Token.WaitHandle.WaitOne(50);

                // Fire the tick function
                FireTick();
            }
        }
    }
}
