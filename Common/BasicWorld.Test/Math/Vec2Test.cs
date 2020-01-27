using BasicWorld.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicWorld.Test.Math
{
    [TestClass]
    public class Vec2Test
    {
        [TestMethod]
        public void Magnitude()
        {
            var vecZero = new Vec2 {X = 0, Y = 0};
            Assert.AreEqual(0, vecZero.Magnitude, 1e-4f);

            var vecOne = new Vec2 {X = 1, Y = 0};
            Assert.AreEqual(1, vecOne.Magnitude, 1e-4f);

            var vecRoot2 = new Vec2 {X = 1, Y = 1};
            Assert.AreEqual(1.414213f, vecRoot2.Magnitude, 1e-4f);
        }

        [TestMethod]
        public void Magnitude2()
        {
            var vecZero = new Vec2 { X = 0, Y = 0 };
            Assert.AreEqual(0, vecZero.Magnitude2, 1e-4f);

            var vecOne = new Vec2 { X = 1, Y = 0 };
            Assert.AreEqual(1, vecOne.Magnitude2, 1e-4f);

            var vecRoot2 = new Vec2 { X = 1, Y = 1 };
            Assert.AreEqual(2, vecRoot2.Magnitude2, 1e-4f);
        }

        [TestMethod]
        public void Normalize()
        {
            var vecNorm = new Vec2 {X = 1, Y = 1}.Normalize();
            Assert.AreEqual(0.7071067f, vecNorm.X, 1e-4f);
            Assert.AreEqual(0.7071067f, vecNorm.Y, 1e-4f);
        }

        [TestMethod]
        public void Add()
        {
            var vecAdd = new Vec2 {X = 1, Y = 2} + new Vec2 {X = 3, Y = 4};
            Assert.AreEqual(4, vecAdd.X, 1e-4f);
            Assert.AreEqual(6, vecAdd.Y, 1e-4f);
        }

        [TestMethod]
        public void Subtract()
        {
            var vecSub = new Vec2 { X = 4, Y = 6 } - new Vec2 { X = 3, Y = 4 };
            Assert.AreEqual(1, vecSub.X, 1e-4f);
            Assert.AreEqual(2, vecSub.Y, 1e-4f);
        }

        [TestMethod]
        public void Multiply()
        {
            var vecMul = new Vec2 {X = 1, Y = 3} * 8;
            Assert.AreEqual(8, vecMul.X, 1e-4f);
            Assert.AreEqual(24, vecMul.Y, 1e-4f);
        }

        [TestMethod]
        public void Dot()
        {
            var dot = Vec2.Dot(new Vec2 {X = 8, Y = 8}, new Vec2 {X = 2, Y = -1});
            Assert.AreEqual(8, dot, 1e-4);
        }

        [TestMethod]
        public void Interpolate()
        {
            var vecInt = Vec2.Interpolate(new Vec2 {X = 10, Y = 10}, new Vec2 {X = 20, Y = 30}, 0.2f);
            Assert.AreEqual(12, vecInt.X, 1e-4);
            Assert.AreEqual(14, vecInt.Y, 1e-4);
        }
    }
}
