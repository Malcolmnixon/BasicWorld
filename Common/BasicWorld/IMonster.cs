using System;
using System.Collections.Generic;
using System.Text;

namespace BasicWorld
{
    public interface IMonster : IActor
    {
        Vec2 Position { get; }
    }
}
