using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicWorld.WorldData
{
    public class WorldState
    {
        public List<Player> Players { get; set; }

        public List<Monster> Monsters { get; set; }

        public void Tick(float deltaTime)
        {
            // Skip if no players or monsters
            if (Players == null || Players.Count == 0 || Monsters == null || Monsters.Count == 0)
                return;

            var random = new Random();

            // Loop through all monsters
            foreach (var monster in Monsters)
            {
                // Update the target player
                var targetPlayer = Players.FirstOrDefault(p => p.Guid == monster.Target);
                if (targetPlayer == null)
                {
                    targetPlayer = Players[random.Next(Players.Count)];
                    monster.Target = targetPlayer.Guid;
                }

                // Walk to player
                var direction = (targetPlayer.Position - monster.Position).Normalize();
                monster.Position += direction * monster.Speed * deltaTime;
            }
        }
    }
}
