using System;
using System.Collections.Generic;
using BasicWorld.Math;
using BasicWorld.WorldData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicWorld.Test
{
    [TestClass]
    public class WorldStateTest
    {
        [TestMethod]
        public void SerializeEmpty()
        {
            var world = new WorldState();

            var json = world.ToJson();

            var world2 = WorldState.FromJson(json);
            Assert.IsNull(world2.Players);
            Assert.IsNull(world2.Monsters);
        }

        [TestMethod]
        public void SerializePopulated()
        {
            var world = new WorldState
            {
                Players = new List<Player>
                {
                    new Player {Guid = Guid.NewGuid(), Position = new Vec2 {X = 1, Y = 2}},
                    new Player {Guid = Guid.NewGuid(), Position = new Vec2 {X = 3, Y = 4}}
                },
                Monsters = new List<Monster>
                {
                    new Monster {Guid = Guid.NewGuid(), Position = new Vec2 {X = 5, Y = 6}}
                }
            };

            var json = world.ToJson();

            var world2 = WorldState.FromJson(json);
            Assert.IsNotNull(world2.Players);
            Assert.AreEqual(2, world2.Players.Count);
            Assert.IsNotNull(world2.Monsters);
            Assert.AreEqual(1, world2.Monsters.Count);
        }
    }
}
