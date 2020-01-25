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
            
            world.Start();
            Assert.AreEqual(5, world.Monsters.Count);
            Assert.AreEqual(0, world.RemotePlayers.Count);
        }
    }
}
