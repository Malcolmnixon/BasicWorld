using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using BasicWorld.WorldData;

namespace BasicWorld.WorldRunner
{
    public class RemoteWorld : WorldBase
    {
        private float _updateAge;

        public RemoteWorld()
        {
            State.Players = new List<Player>();
            State.Monsters = new List<Monster>();
        }

        public override void Start()
        {
            // Perform base start
            base.Start();
        }

        /// <summary>
        /// Update world with new state
        /// </summary>
        /// <param name="newState"></param>
        protected void UpdateWorld(WorldState newState)
        {
            // TODO: Resolve differences in local and remote state of our player
            if (Player != null)
            {
                newState.Players.RemoveAll(p => p.Guid == Player.Guid);
                newState.Players.Add(Player);
            }

            // Reset smooth update positions for remote players
            foreach (var player in newState.Players)
                player.SmoothPosition = player.Position;

            // Reset smooth update positions for monsters
            foreach (var monster in newState.Monsters)
                monster.SmoothPosition = monster.Position;

            // Update to the new state
            lock (WorldLock)
            {
                State = newState;
                _updateAge = 0;
            }
        }

        protected override void Tick(float deltaTime)
        {
            _updateAge += deltaTime;

            // Smooth update remote players
            foreach (var player in RemotePlayers)
                player.SmoothPosition = player.Position + player.Velocity * _updateAge;

            // Smooth update monsters
            foreach (var monster in State.Monsters)
                monster.SmoothPosition = monster.Position + monster.Velocity * _updateAge;
        }
    }
}
