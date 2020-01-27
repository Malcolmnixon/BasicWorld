using System;
using System.Collections.Generic;
using System.Text;

namespace BasicWorld.Math
{
    /// <summary>
    /// 2D line
    /// </summary>
    public struct Line2
    {
        /// <summary>
        /// Point on line
        /// </summary>
        public Vec2 Location;

        /// <summary>
        /// Line normal
        /// </summary>
        public Vec2 Normal;

        /// <summary>
        /// Calculate the intercept of a ray with this line
        /// </summary>
        /// <param name="ray">Ray</param>
        /// <param name="distance">Intercept distance along ray</param>
        /// <returns>True if ray intercepts line</returns>
        public bool Intercept(Ray2 ray, out float distance)
        {
            // Get the dot-product of the distance from the line to the ray and the normal
            var distanceDotNormal = Vec2.Dot(Location - ray.Origin, Normal);

            // Get the dot product of the ray and the line normal
            var rayDotNormal = Vec2.Dot(ray.Direction, Normal);
            if (rayDotNormal >= -1e-5f && rayDotNormal <= 1e-5f)
            {
                if (distanceDotNormal >= -1e-5 && distanceDotNormal < 1e-5)
                {
                    // Line is on plane
                    distance = 0f;
                    return true;
                }

                // No intercept
                distance = float.PositiveInfinity;
                return false;
            }

            // Calculate distance and return true if non-negative
            distance = distanceDotNormal / rayDotNormal;
            return distance >= 0f;
        }

        /// <summary>
        /// Test if a point is "inside" the line
        /// </summary>
        /// <param name="point">Point to test</param>
        /// <returns>True if point is inside line</returns>
        public bool Inside(Vec2 point)
        {
            // Calculate depth inside line
            var depth = Vec2.Dot(Location - point, Normal);

            // Return true if inside (with a fudge margin)
            return depth >= -1e-4;
        }
    }
}
