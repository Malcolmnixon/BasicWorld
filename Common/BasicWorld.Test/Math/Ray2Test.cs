using BasicWorld.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicWorld.Test.Math
{
    [TestClass]
    public class Ray2Test
    {
        [TestMethod]
        public void Point()
        {
            var ray = new Ray2
            {
                Origin = new Vec2 { X = 10, Y = 10 },
                Direction = new Vec2 { X = 1f, Y= 1f }
            };

            var pos = ray.Point(5);
            Assert.AreEqual(15, pos.X, 1e-4f);
            Assert.AreEqual(15, pos.Y, 1e-4f);
        }
    }
}
