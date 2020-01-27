namespace BasicWorld.Math
{
    /// <summary>
    /// 2D ray structure
    /// </summary>
    public struct Ray2
    {
        /// <summary>
        /// Ray origin
        /// </summary>
        public Vec2 Origin;

        /// <summary>
        /// Ray direction
        /// </summary>
        public Vec2 Direction;

        /// <summary>
        /// Get point at distance down ray
        /// </summary>
        /// <param name="distance">Distance</param>
        /// <returns>Point on ray</returns>
        public Vec2 Point(float distance) => Origin + Direction * distance;
    }
}
