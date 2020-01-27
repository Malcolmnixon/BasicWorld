using BasicWorld.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicWorld.Test.Math
{
    [TestClass]
    public class ConvexPoly2Test
    {
        [TestMethod]
        public void Inside()
        {
            var poly = new ConvexPoly2(
                new[]
                {
                    new Vec2 {X = 10, Y = 10},
                    new Vec2 {X = 10, Y = 20},
                    new Vec2 {X = 20, Y = 20},
                    new Vec2 {X = 20, Y = 10}
                });

            var inside = poly.Inside(new Vec2 {X = 15, Y = 15});
            Assert.IsTrue(inside);

            var outside = poly.Inside(new Vec2 {X = 5, Y = 10});
            Assert.IsFalse(outside);
        }

        [TestMethod]
        public void Intercept()
        {
            var poly = new ConvexPoly2(
                new[]
                {
                    new Vec2 {X = 10, Y = 10},
                    new Vec2 {X = 10, Y = 20},
                    new Vec2 {X = 20, Y = 20},
                    new Vec2 {X = 20, Y = 10}
                });

            var ray1 = new Ray2
            {
                Origin = new Vec2 { X = 0, Y = 0 },
                Direction = new Vec2 { X=10, Y=10}.Normalize()
            };
            var hit1 = poly.Intercept(ray1, out var dist1);
            Assert.IsTrue(hit1);
            Assert.AreEqual(14.14213f, dist1, 1e-4);

            var ray2 = new Ray2
            {
                Origin = new Vec2 {X = 0, Y = 0},
                Direction = new Vec2 {X = 10, Y = 15}.Normalize()
            };
            var hit2 = poly.Intercept(ray2, out var dist2);
            Assert.IsTrue(hit2);
            Assert.AreEqual(18.02775f, dist2, 1e-4);

            var ray3 = new Ray2
            {
                Origin = new Vec2 { X = 0, Y = 0 },
                Direction = new Vec2 { X = 15, Y = 10 }.Normalize()
            };
            var hit3 = poly.Intercept(ray3, out var dist3);
            Assert.IsTrue(hit3);
            Assert.AreEqual(18.02775f, dist3, 1e-4);

            var ray4 = new Ray2
            {
                Origin = new Vec2 { X = 0, Y = 0 },
                Direction = new Vec2 { X = 10, Y = 25 }.Normalize()
            };
            var hit4 = poly.Intercept(ray4, out _);
            Assert.IsFalse(hit4);
        }
    }
}
