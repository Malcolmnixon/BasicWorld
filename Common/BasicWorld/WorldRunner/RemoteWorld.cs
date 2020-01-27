using System.Collections.Generic;
using BasicWorld.WorldData;

namespace BasicWorld.WorldRunner
{
    public class RemoteWorld : WorldBase
    {
        public RemoteWorld()
        {
            State.Players = new List<Player>();
            State.Monsters = new List<Monster>();
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

            // Update to the new state
            lock (WorldLock)
            {
                State = newState;
            }
        }

        protected override void Tick(float deltaTime)
        {
            // Smooth update remote players
            foreach (var player in State.Players)
                player.Position += player.Velocity * deltaTime;

            // Smooth update monsters
            foreach (var monster in State.Monsters)
                monster.Position += monster.Velocity * deltaTime;
        }
    }
}
