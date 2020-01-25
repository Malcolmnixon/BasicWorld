using System;
using System.Collections;
using System.Collections.Generic;

namespace BasicWorld
{
    public interface IWorld
    {
        ILocalPlayer Player { get; }

        IList<IRemotePlayer> RemotePlayers { get; }

        IList<IMonster> Monsters { get; }
    }
}
