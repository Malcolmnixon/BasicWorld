using System;
using System.Collections.Generic;
using System.Text;

namespace BasicWorld.WorldData
{
    public class Monster : Actor
    {
        public Guid Target { get; set; }

        public float Speed { get; set; }
    }
}
