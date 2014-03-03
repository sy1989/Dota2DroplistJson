using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Threading;

namespace Drop
{

    public class DropList
    {


        [JsonProperty("drops")]
        public Drop Drops { get; set; }

        [JsonProperty("craftings")]
        public Crafting Craftings { get; set; }

        public class Item
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }
        }
        public class Drop
        {

            [JsonProperty("uncommon_drops")]
            public List<Item> uncommon_drops { get; set; }

            [JsonProperty("common_drops")]
            public List<Item> common_drops { get; set; }


            [JsonProperty("rare_drops")]
            public List<Item> rare_drops { get; set; }

            [JsonProperty("mythical_drops")]
            public List<Item> mythical_drops { get; set; }

            [JsonProperty("legendary_drops")]
            public List<Item> legendary_drops { get; set; }

            [JsonProperty("arcana_drops")]
            public List<Item> arcana_drops { get; set; }




        }
        public class Crafting
        {
            [JsonProperty("crafting_arcana")]
            public List<Item> crafting_arcana { get; set; }

            [JsonProperty("crafting_common")]
            public List<Item> crafting_common { get; set; }

            [JsonProperty("crafting_uncommon")]
            public List<Item> crafting_uncommon { get; set; }

            [JsonProperty("crafting_exceptional")]
            public List<Item> crafting_exceptional { get; set; }

            [JsonProperty("crafting_legendary")]
            public List<Item> crafting_legendary { get; set; }

            [JsonProperty("crafting_mythical")]
            public List<Item> crafting_mythical { get; set; }

            [JsonProperty("crafting_rare")]
            public List<Item> crafting_rare { get; set; }
        }


    }
}

