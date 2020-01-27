using Newtonsoft.Json;

namespace BasicWorld.Math
{
    /// <summary>
    /// 2D Vector
    /// </summary>
    public struct Vec2
    {
        /// <summary>
        /// Gets or sets X value
        /// </summary>
        public float X;

        /// <summary>
        /// Gets or sets Y value
        /// </summary>
        public float Y;

        /// <summary>
        /// Initializes a new instance of the Vec2 class
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        public Vec2(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets the magnitude of this vector
        /// </summary>
        [JsonIgnore]
        public float Magnitude => (float)System.Math.Sqrt(X * X + Y * Y);

        /// <summary>
        /// Gets the magnitude squared of this vector
        /// </summary>
        public float Magnitude2 => X * X + Y * Y;

        /// <summary>
        /// Norm
        /// </summary>
        /// <returns></returns>
        public Vec2 Normalize()
        {
            var len = Magnitude;

            if (len < 1e-6)
                return new Vec2();

            return this * (1/len);
        }

        /// <summary>
        /// Add vectors
        /// </summary>
        /// <param name="a">Vector A</param>
        /// <param name="b">Vector B</param>
        /// <returns>Vector A + B</returns>
        public static Vec2 operator +(Vec2 a, Vec2 b) => new Vec2 {X = a.X + b.X, Y = a.Y + b.Y};

        /// <summary>
        /// Subtract vectors
        /// </summary>
        /// <param name="a">Vector A</param>
        /// <param name="b">Vector B</param>
        /// <returns>Vector A - B</returns>
        public static Vec2 operator -(Vec2 a, Vec2 b) => new Vec2 {X = a.X - b.X, Y = a.Y - b.Y};

        /// <summary>
        /// Scale vector
        /// </summary>
        /// <param name="a">Vector A</param>
        /// <param name="s">Scale S</param>
        /// <returns>Vector A*S</returns>
        public static Vec2 operator *(Vec2 a, float s) => new Vec2 {X = a.X * s, Y = a.Y * s};

        /// <summary>
        /// Calculate dot-product of two vectors
        /// </summary>
        /// <param name="a">Vector A</param>
        /// <param name="b">Vector B</param>
        /// <returns>Dot product of vectors</returns>
        public static float Dot(Vec2 a, Vec2 b) => a.X * b.X + a.Y * b.Y;

        /// <summary>
        /// Linear interpolate between A and B
        /// </summary>
        /// <param name="a">Vector A</param>
        /// <param name="b">Vector B</param>
        /// <param name="t">Interpolation value</param>
        /// <returns>Interpolated value</returns>
        public static Vec2 Interpolate(Vec2 a, Vec2 b, float t)
        {
            var ti = 1 - t;
            return new Vec2 {X = a.X * ti + b.X * t, Y = a.Y * ti + b.Y * t};
        }
    }
}
