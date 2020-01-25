using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BasicWorld
{
    public class LocalWorld : WorldRunner
    {
        public override IList<IRemotePlayer> RemotePlayers { get; } = new List<IRemotePlayer>();
    }
}
