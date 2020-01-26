using BasicWorld.WorldRunner;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicWorld.Test
{
    [TestClass]
    public class LocalWorldTest
    {
        [TestMethod]
        public void TestCreate()
        {
            using var world = new LocalWorld();
            var player = world.CreateLocalPlayer();

            Assert.IsNotNull(world.Player);
            Assert.AreEqual(0, world.RemotePlayers.Count);
            Assert.AreEqual(0, world.Monsters.Count);
        }
    }
}
