using System;
using System.Collections.Generic;
using System.Text;

namespace BasicWorld
{
    public interface IRemotePlayer : IActor
    {
        Vec2 Position { get; }
    }
}
