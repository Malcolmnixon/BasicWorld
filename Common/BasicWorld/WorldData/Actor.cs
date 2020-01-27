using System;
using BasicWorld.Math;
using Newtonsoft.Json;

namespace BasicWorld.WorldData
{
    public class Actor
    {
        /// <summary>
        /// Actor unique-ID
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Actor position
        /// </summary>
        public Vec2 Position { get; set; }

        /// <summary>
        /// Actor velocity
        /// </summary>
        public Vec2 Velocity { get; set; }

        /// <summary>
        /// Actor smooth-position (interpolated locally)
        /// </summary>
        [JsonIgnore]
        public Vec2 SmoothPosition { get; set; }
    }
}
