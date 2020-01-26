using System.Collections.Generic;
using Newtonsoft.Json;

namespace BasicWorld.WorldData
{
    /// <summary>
    /// World state
    /// </summary>
    public class WorldState
    {
        /// <summary>
        /// List of players
        /// </summary>
        public List<Player> Players { get; set; }

        /// <summary>
        /// List of monsters
        /// </summary>
        public List<Monster> Monsters { get; set; }

        /// <summary>
        /// Serialize world state to JSON
        /// </summary>
        /// <returns>JSON encoded world state</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Deserialize world state from JSON
        /// </summary>
        /// <param name="json">JSON encoded world state</param>
        /// <returns>New world state</returns>
        public static WorldState FromJson(string json)
        {
            return JsonConvert.DeserializeObject<WorldState>(json);
        }
    }
}
