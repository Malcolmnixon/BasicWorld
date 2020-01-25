using System;
using System.Collections;
using System.Collections.Generic;

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
