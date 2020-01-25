using System;
using System.Collections.Generic;
using System.Text;

namespace BasicWorld
{
    public interface ILocalPlayer : IActor
    {
        Vec2 Position { get; set; }
    }
}
