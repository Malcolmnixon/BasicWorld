using System.Collections.Generic;
using System.Collections.ObjectModel;
using BasicWorld.WorldData;

namespace BasicWorld
{
    public interface IWorld
    {
        /// <summary>
        /// Get the current player
        /// </summary>
        Player Player { get; }

        /// <summary>
        /// Get all remote players
        /// </summary>
        ReadOnlyCollection<Player> RemotePlayers { get; }

        /// <summary>
        /// Get all monsters
        /// </summary>
        ReadOnlyCollection<Monster> Monsters { get; }

        /// <summary>
        /// Start the world
        /// </summary>
        void Start();

        /// <summary>
        /// Update the world
        /// </summary>
        void Update();

        /// <summary>
        /// Create local player
        /// </summary>
        /// <returns></returns>
        Player CreateLocalPlayer();
    }
}
