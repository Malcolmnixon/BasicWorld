using System;
using System.Collections.Generic;

namespace BasicWorld.Math
{
    public class ConvexPoly2
    {
        /// <summary>
        /// Array of lines
        /// </summary>
        private readonly Line2[] _lines;

        /// <summary>
        /// Construct a convex polygon from the given points
        /// </summary>
        /// <param name="points">List of points</param>
        public ConvexPoly2(IList<Vec2> points)
        {
            // Allocate the array of lines
            _lines = new Line2[points.Count];

            // Populate lines
            for (var i = 0; i < points.Count; ++i)
            {
                var p1 = points[i];
                var p2 = points[(i + 1) % points.Count];

                var location = (p1 + p2) * 0.5f;
                var normal = new Vec2 {X = p2.Y - p1.Y, Y = -p2.X + p1.X}.Normalize();

                _lines[i] = new Line2 {Location = location, Normal = normal};
            }

            // Sanity check normals
            var inside = 0;
            var outside = 0;
            for (var i = 0; i < _lines.Length; ++i)
            {
                var l1 = _lines[i];
                var l2 = _lines[(i + 1) % _lines.Length];

                if (l1.Inside(l2.Location))
                    inside++;
                else
                    outside++;
            }

            // If inside and outside are mixed then the polygon is not convex
            if (inside != 0 && outside != 0)
                throw new Exception("Points do not make convex polygon");

            // If all points are outside then we have the wrong winding, flip all normals
            if (outside != 0)
                for (var i = 0; i < _lines.Length; ++i)
                    _lines[i].Normal *= -1;
        }

        public bool Inside(Vec2 pos)
        {
            for (var i = 0; i < _lines.Length; ++i)
            {
                if (!_lines[i].Inside(pos))
                    return false;
            }

            return true;
        }

        public bool Intercept(Ray2 ray, out float dist)
        {
            return Intercept(ray, out _, out _, out dist);
        }

        private bool Intercept(Ray2 ray, out Line2 line, out Vec2 point, out float dist)
        {
            // Loop over all lines
            var minLine = new Line2();
            var minPoint = new Vec2();
            var minDist = float.MaxValue;
            for (var i = 0; i < _lines.Length; ++i)
            {
                // Skip lines we don't intercept with
                if (!_lines[i].Intercept(ray, out var lineDist))
                    continue;

                // Get the hit point
                var pos = ray.Point(lineDist);

                // Skip points that aren't inside
                if (!Inside(pos))
                    continue;

                // Skip if this isn't the closest intercept
                if (lineDist > minDist)
                    continue;

                // Save the closest thus far
                minDist = lineDist;
                minLine = _lines[i];
                minPoint = pos;
            }

            // Save line and point
            line = minLine;
            point = minPoint;

            // Detect hit
            if (minDist < float.MaxValue)
            {
                dist = minDist;
                return true;
            }

            // No intercept
            dist = float.PositiveInfinity;
            return false;
        }
    }
}
