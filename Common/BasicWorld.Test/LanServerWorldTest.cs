using System.Linq;
using System.Threading;
using BasicWorld.LanGame;
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
            var player = serverWorld.CreateLocalPlayer();
            serverWorld.Start();
            Assert.AreEqual(0, serverWorld.Monsters.Count);
            Assert.AreEqual(0, serverWorld.RemotePlayers.Count);
        }

        [TestMethod]
        public void TestDiscover()
        {
            using var serverWorld = new LanServerWorld();
            serverWorld.Start();

            using var discovery = new LanServerDiscovery();
            discovery.Start();

            // Wait 5 seconds - server sends discoveries every 3 seconds
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

            // Wait 5 seconds - server sends discoveries every 3 seconds
            Thread.Sleep(5000);

            Assert.IsTrue(discovery.Servers.Count >= 1);

            // Connect to server
            using var clientWorld = new LanClientWorld(discovery.Servers.First().Item1);
            clientWorld.Start();
        }

        [TestMethod]
        public void TestSynchronize()
        {
            using var serverWorld = new LanServerWorld();
            var player = serverWorld.CreateLocalPlayer();
            serverWorld.Start();

            using var discovery = new LanServerDiscovery();
            discovery.Start();

            // Wait 5 seconds - server sends discoveries every 3 seconds
            Thread.Sleep(5000);

            Assert.IsTrue(discovery.Servers.Count >= 1);

            using var clientWorld = new LanClientWorld(discovery.Servers.First().Item1);
            clientWorld.Start();

            // Wait 1 second for game state to synchronize
            Thread.Sleep(1000);

            Assert.AreEqual(1, serverWorld.State.Players.Count);
            Assert.AreEqual(1, clientWorld.State.Players.Count);
        }
    }
}
