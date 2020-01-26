using System;
using System.Collections.Generic;
using System.Text;
using BasicWorld.WorldData;

namespace BasicWorld.WorldRunner
{
    public class RemoteWorld : WorldBase
    {
        public RemoteWorld()
        {
            State.Players = new List<Player>();
            State.Monsters = new List<Monster>();
        }

        protected override void Tick(float deltaTime)
        {
            // TODO: Implement smooth motion kinematics
        }
    }
}
