using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicWorld.Test
{
    [TestClass]
    public class LanServerWorldTest
    {
        [TestMethod]
        public void TestCreate()
        {
            using var serverWorld = new LanServerWorld();

            serverWorld.Start();
            Assert.AreEqual(5, serverWorld.Monsters.Count);
            Assert.AreEqual(0, serverWorld.RemotePlayers.Count);
        }

        [TestMethod]
        public void TestDiscover()
        {
            using var serverWorld = new LanServerWorld();
            serverWorld.Start();

            using var discovery = new LanServerDiscovery();
            discovery.Start();

            Thread.Sleep(5000);

            Assert.IsTrue(discovery.Servers.Count >= 1);
        }

        [TestMethod]
        public void TestConnect()
        {
            using var serverWorld = new LanServerWorld();
            serverWorld.Start();

            using var discovery = new LanServerDiscovery();
            discovery.Start();

            Thread.Sleep(5000);

            Assert.IsTrue(discovery.Servers.Count >= 1);

            using var clientWorld = new LanClientWorld(discovery.Servers.First().Item1);
            clientWorld.Start();
        }

        [TestMethod]
        public void TestSynchronize()
        {
            using var serverWorld = new LanServerWorld();
            serverWorld.Start();

            using var discovery = new LanServerDiscovery();
            discovery.Start();

            Thread.Sleep(5000);

            Assert.IsTrue(discovery.Servers.Count >= 1);

            using var clientWorld = new LanClientWorld(discovery.Servers.First().Item1);
            clientWorld.Start();

            Thread.Sleep(1000);

            Assert.AreEqual(5, serverWorld.Monsters.Count);
            Assert.AreEqual(1, serverWorld.RemotePlayers.Count);
            Assert.AreEqual(5, clientWorld.Monsters.Count);
            Assert.AreEqual(1, clientWorld.RemotePlayers.Count);
        }
    }
}
