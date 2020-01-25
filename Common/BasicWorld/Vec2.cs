using System;
using System.Collections.Generic;
using System.Text;

namespace BasicWorld
{
    public struct Vec2
    {
        public Vec2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }

        public float Length() => (float)Math.Sqrt(X * X + Y * Y);

        public Vec2 Normalize()
        {
            var len = Length();

            if (len < 1e-6)
                return new Vec2(0, 0);

            return this / len;
        }

        public static Vec2 operator +(Vec2 a, Vec2 b) => new Vec2(a.X + b.X, a.Y + b.Y);
        public static Vec2 operator -(Vec2 a, Vec2 b) => new Vec2(a.X - b.X, a.Y - b.Y);
        public static Vec2 operator *(Vec2 a, float s) => new Vec2(a.X * s, a.Y * s);
        public static Vec2 operator /(Vec2 a, float s) => a * (1 / s);
    }
}
