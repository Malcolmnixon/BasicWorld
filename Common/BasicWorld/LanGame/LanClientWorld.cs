using System.Net;
using BasicWorld.NetGame;

namespace BasicWorld.LanGame
{
    public class LanClientWorld : NetClientWorld
    {
        public LanClientWorld(IPAddress server) : base(server)
        {
        }
    }
}
