using Newtonsoft.Json;
using System.Collections.Generic;

namespace WitAi
{
    public class EntityResponse
    {
        [JsonProperty("id")]
        /// <summary>ID or name of the requested entity</summary>
        public string Id { get; set; }

        /// <summary>Short sentence describing this entity</summary>
        [JsonProperty("doc")]
        public string Doc { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("builtin")]
        public bool Builtin { get; set; }

        /// <summary>
        /// Currently only supporting “trait” or “keywords” Search Strategy. 
        /// If not provided, it will default to “keywords”.Traits are only available for new Bot Engine apps
        /// </summary>
        [JsonProperty("lookups")]
        public Dictionary<string, dynamic> Lookups { get; set; }
    }
}