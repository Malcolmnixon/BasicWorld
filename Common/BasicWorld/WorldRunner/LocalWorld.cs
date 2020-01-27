using System;
using System.Collections.Generic;
using System.Linq;
using BasicWorld.Math;
using BasicWorld.WorldData;

namespace BasicWorld.WorldRunner
{
    /// <summary>
    /// Local world
    /// </summary>
    public class LocalWorld : WorldBase
    {
        /// <summary>
        /// World random data
        /// </summary>
        private readonly Random _random = new Random();

        public LocalWorld()
        {
            State.Players = new List<Player>();
            State.Monsters = new List<Monster>();
        }

        protected override void Tick(float deltaTime)
        {
            lock (WorldLock)
            {
                var monsters = State.Monsters;
                var players = State.Players;

                // Spawn new monsters (up to 5) about once every 10 seconds
                if (monsters.Count < 5 && _random.NextDouble() < deltaTime / 10)
                {
                    var monster = new Monster
                    {
                        Guid = Guid.NewGuid(),
                        Position = new Vec2
                        {
                            X = (float) (_random.NextDouble() * 100 - 50),
                            Y = (float) (_random.NextDouble() * 100 - 50)
                        },
                        Speed = (float) (_random.NextDouble() * 2 + 1)
                    };

                    // Add to monsters
                    monsters.Add(monster);
                }

                // Loop through all monsters
                foreach (var monster in monsters)
                {
                    // Handle switching targets randomly every 20 seconds
                    if (players.Count != 0 && _random.NextDouble() < deltaTime / 20)
                    {
                        monster.Target = players[_random.Next(players.Count)].Guid;
                    }

                    // If monster has target then run towards it
                    var target = State.Players.FirstOrDefault(p => p.Guid == monster.Target);
                    if (target != null)
                    {
                        var direction = (target.Position - monster.Position).Normalize();
                        monster.Position += direction * monster.Speed * deltaTime;
                    }
                }
            }
        }
    }
}
