using BasicWorld.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicWorld.Test.Math
{
    [TestClass]
    public class Line2Test
    {
        [TestMethod]
        public void Intercept()
        {
            var line = new Line2
            {
                Location = new Vec2 { X = 1, Y = 1 },
                Normal = new Vec2 { X = 0, Y = -1 }
            };

            var ray = new Ray2
            {
                Origin = new Vec2 {X = 10, Y = -5},
                Direction = new Vec2 {X = 0.7071067f, Y = 0.7071067f}
            };

            var hits = line.Intercept(ray, out var dist);
            Assert.IsTrue(hits);
            Assert.AreEqual(8.485281f, dist, 1e-4f);

            var pos = ray.Point(dist);
            Assert.AreEqual(16, pos.X, 1e-4f);
            Assert.AreEqual(1, pos.Y, 1e-4f);
        }

        [TestMethod]
        public void Inside()
        {
            var line = new Line2
            {
                Location = new Vec2 { X = 1, Y = 1 },
                Normal = new Vec2 { X = 0, Y = -1 }
            };

            var inside = line.Inside(new Vec2 {X = -35, Y = 3});
            Assert.IsTrue(inside);

            var outside = line.Inside(new Vec2 {X = 66, Y = -4});
            Assert.IsFalse(outside);
        }
    }
}
