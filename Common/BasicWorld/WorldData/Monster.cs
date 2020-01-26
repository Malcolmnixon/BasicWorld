using System;

namespace BasicWorld.WorldData
{
    public class Monster : Actor
    {
        public Guid Target { get; set; }

        public float Speed { get; set; }
    }
}
