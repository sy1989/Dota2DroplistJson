using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Threading;

namespace Drop
{
    /// <summary>
    /// This class represents the TF2 Item schema as deserialized from its
    /// JSON representation.
    /// </summary>
    public class Dota2
    {


        public static Dota2 FetchSchema(string url)
        {
            HttpWebResponse response = Drop.SteamWeb.Request(url, "GET");
            string result = Vdf2Json.ConvertVdf2Json(response.GetResponseStream());
            response.Close();
            SchemaResult schemaResult = JsonConvert.DeserializeObject<SchemaResult>(result);
            return schemaResult.items_game ?? null;

        }
        private static string GetSchemaString(HttpWebResponse response)
        {
            string result;
            var reader = new StreamReader(response.GetResponseStream());
            result = reader.ReadToEnd();
            return result;
        }

        [JsonProperty("loot_lists")]
        public Loot_lists loot_lists { get; set; }

        public class Loot_lists
        {

            [JsonProperty("uncommon_drops")]
            public Dictionary<string, string> uncommon_drops { get; set; }

            [JsonProperty("common_drops")]
            public Dictionary<string, string> common_drops { get; set; }


            [JsonProperty("rare_drops")]
            public Dictionary<string, string> rare_drops { get; set; }

            [JsonProperty("mythical_drops")]
            public Dictionary<string, string> mythical_drops { get; set; }

            [JsonProperty("legendary_drops")]
            public Dictionary<string, string> legendary_drops { get; set; }

            [JsonProperty("arcana_drops")]
            public Dictionary<string, string> arcana_drops { get; set; }

            [JsonProperty("crafting_arcana")]
            public Dictionary<string, string> crafting_arcana { get; set; }

            [JsonProperty("crafting_common")]
            public Dictionary<string, string> crafting_common { get; set; }

            [JsonProperty("crafting_uncommon")]
            public Dictionary<string, string> crafting_uncommon { get; set; }

            [JsonProperty("crafting_exceptional")]
            public Dictionary<string, string> crafting_exceptional { get; set; }

            [JsonProperty("crafting_legendary")]
            public Dictionary<string, string> crafting_legendary { get; set; }

            [JsonProperty("crafting_mythical")]
            public Dictionary<string, string> crafting_mythical { get; set; }

            [JsonProperty("crafting_rare")]
            public Dictionary<string, string> crafting_rare { get; set; }

            
             [JsonProperty("wraithnight_premium_fortune_items")]
            public Dictionary<string, string> wraithnight_premium_fortune_items { get; set; } //冥魂之夜骨瓮

             [JsonProperty("wraithnight_premium_fortune_items_2")]
             public Dictionary<string, string> wraithnight_premium_fortune_items_2 { get; set; } //冥魂之夜shen瓮


            [JsonProperty("frostivus_2013_wraith_king_items")]
             public Dictionary<string, string> frostivus_2013_wraith_king_items { get; set; } //图纸

            [JsonProperty("wraithnight_small_offering_list")]
            public Dictionary<string, string> wraithnight_small_offering_list { get; set; } //祭品

            [JsonProperty("wraithnight_large_tribute_list")]
            public Dictionary<string, string> wraithnight_large_tribute_list { get; set; } //贡物 
        }

        protected class SchemaResult
        {
            public Dota2 items_game { get; set; }
        }

    }
}

