using System;
using System.Collections;
using System.Collections.Generic;
using BasicWorld.WorldData;

namespace BasicWorld
{
    public interface IWorld
    {
        Player Player { get; }

        List<Player> RemotePlayers { get; }

        List<Monster> Monsters { get; }

        /// <summary>
        /// Start the world
        /// </summary>
        void Start();
    }
}
