using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Update world with new state
        /// </summary>
        /// <param name="state"></param>
        protected void UpdateWorld(WorldState state)
        {
            // TODO: Resolve differences in local and remote state of our player
            if (Player != null)
            {
                state.Players.RemoveAll(p => p.Guid == Player.Guid);
                state.Players.Add(Player);
            }

            // Reset smooth update positions for remote players
            foreach (var player in RemotePlayers)
                player.SmoothPosition = player.Position;

            // Reset smooth update positions for monsters
            foreach (var monster in State.Monsters)
                monster.SmoothPosition = monster.Position;

            // Update to the new state
            lock (WorldLock)
            {
                State = state;
                _updateAge = 0;
            }
        }

        protected override void Tick(float deltaTime)
        {
            // TODO: Implement smooth motion kinematics
            lock (WorldLock)
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
}
