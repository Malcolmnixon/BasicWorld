using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using BasicWorld.NetGame;

namespace BasicWorld.WebGame
{
    /// <summary>
    /// Web client world
    /// </summary>
    public class WebClientWorld : NetClientWorld
    {
        public WebClientWorld() : base(IPAddress.Parse("204.48.22.8"))
        {
        }
    }
}
