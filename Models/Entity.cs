using Newtonsoft.Json;
using System.Collections.Generic;

namespace WitAi
{
    public class Entity
    {
        public Entity(string id, string doc = null, Dictionary<dynamic, dynamic> values = null, Dictionary<dynamic, dynamic> lookups = null)
        {
            this.Id = id;
            this.Doc = doc;
            this.Values = values;
            this.Lookups = lookups;
        }

        [JsonProperty("id")]
        /// <summary>ID or name of the requested entity</summary>
        public string Id { get; set; }

        [JsonProperty("doc")]
        /// <summary>Short sentence describing this entity</summary>
        public string Doc { get; set; }

        [JsonProperty("values")]
        /// <summary>Possible values for this entity</summary>
        public Dictionary<dynamic, dynamic> Values { get; set; }

        [JsonProperty("lookups")]
        /// <summary>
        /// Currently only supporting “trait” or “keywords” Search Strategy. 
        /// If not provided, it will default to “keywords”.Traits are only available for new Bot Engine apps
        /// </summary>
        public Dictionary<dynamic, dynamic> Lookups { get; set; }
    }
}