﻿using System;
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
    public class Schemazh
    {
        private const string SchemaMutexName = "steam_bot_cache_file_mutex";
        private const string SchemaApiUrlBase = "http://api.steampowered.com/IEconItems_570/GetSchema/v0001/?language=zh&key=52A32619A2A2E702F8A2E8E9B6D3F2D4";
        private const string cachefile = "dota2_schema_zh.cache";

        /// <summary>
        /// Fetches the Tf2 Item schema.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <returns>A  deserialized instance of the Item Schema.</returns>
        /// <remarks>
        /// The schema will be cached for future use if it is updated.
        /// </remarks>
        public static Schemazh FetchSchema()
        {
            var url = SchemaApiUrlBase;

            // just let one thread/proc do the initial check/possible update.
            bool wasCreated;
            var mre = new EventWaitHandle(false,
                EventResetMode.ManualReset, SchemaMutexName, out wasCreated);

            // the thread that create the wait handle will be the one to 
            // write the cache file. The others will wait patiently.
            if (!wasCreated)
            {
                bool signaled = mre.WaitOne(10000);

                if (!signaled)
                {
                    return null;
                }
            }

            HttpWebResponse response = Drop.SteamWeb.Request(url, "GET");

            DateTime schemaLastModified = DateTime.Parse(response.Headers["Last-Modified"]);

            string result = GetSchemaString(response, schemaLastModified);

            response.Close();

            // were done here. let others read.
            mre.Set();

            SchemaResult schemaResult = JsonConvert.DeserializeObject<SchemaResult>(result);
            return schemaResult.result ?? null;
        }

        // Gets the schema from the web or from the cached file.
        private static string GetSchemaString(HttpWebResponse response, DateTime schemaLastModified)
        {
            string result;
            bool mustUpdateCache = !File.Exists(cachefile) || schemaLastModified > File.GetCreationTime(cachefile);

            if (mustUpdateCache)
            {
                var reader = new StreamReader(response.GetResponseStream());
                result = reader.ReadToEnd();

                File.WriteAllText(cachefile, result);
                File.SetCreationTime(cachefile, schemaLastModified);
            }
            else
            {
                // read previously cached file.
                TextReader reader = new StreamReader(cachefile);
                result = reader.ReadToEnd();
                reader.Close();
            }

            return result;
        }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("items_game_url")]
        public string ItemsGameUrl { get; set; }

        [JsonProperty("items")]
        public Item[] Items { get; set; }

        [JsonProperty("originNames")]
        public ItemOrigin[] OriginNames { get; set; }

        [JsonProperty("item_sets")]
        public Item_set[] Item_sets { get; set; }


        /// <summary>
        /// Find an SchemaItem by it's defindex.
        /// </summary>
        public Item GetItem(int defindex)
        {
            foreach (Item item in Items)
            {
                if (item.Defindex == defindex)
                    return item;
            }
            return null;
        }


        public Item GetItemByZhname(string zhname)
        {
            foreach (Item item in Items)
            {
                if (item.ItemName == zhname)
                    return item;
            }
            return null;
        }

        public string  GetZh(string name)
        {
            foreach (Item item in Items)
            {
                if ( item.Name  == name)
                    return item.ItemName;
            }
            return null;
        }

        public string GetPic(string name)
        {
            foreach (Item item in Items)
            {
                if (item.Name == name)
                    return item.Image_url;
            }
            return null;
        }
        /// <summary>
        /// Returns all Items of the given crafting material.
        /// </summary>
        /// <param name="material">Item's craft_material_type JSON property.</param>
        /// <seealso cref="Item"/>
        public List<Item> GetItemsByCraftingMaterial(string material)
        {
            return Items.Where(item => item.CraftMaterialType == material).ToList();
        }

        public List<Item> GetItems()
        {
            return Items.ToList();
        }

        public class ItemOrigin
        {
            [JsonProperty("origin")]
            public int Origin { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public class Item
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("defindex")]
            public ushort Defindex { get; set; }

            [JsonProperty("item_class")]
            public string ItemClass { get; set; }

            [JsonProperty("item_type_name")]
            public string ItemTypeName { get; set; }

            [JsonProperty("item_name")]
            public string ItemName { get; set; }


            [JsonProperty("image_url")]
            public string Image_url { get; set; }

            [JsonProperty("craft_material_type")]
            public string CraftMaterialType { get; set; }

            [JsonProperty("used_by_classes")]
            public string[] UsableByClasses { get; set; }

            [JsonProperty("item_slot")]
            public string ItemSlot { get; set; }

            [JsonProperty("craft_class")]
            public string CraftClass { get; set; }

            [JsonProperty("item_quality")]
            public int ItemQuality { get; set; }
        }



        public class Item_set
        {

            [JsonProperty("item_set")]
            public string Item_setname { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("items")]
            public List<string> Setsinclude { get; set; }

        }

        public Item_set GetItemBySet(string setname)
        {
            foreach (Item_set set in Item_sets)
            {
                if (set.Name == setname)
                    return set;
            }
            return null;
        }


        protected class SchemaResult
        {
            public Schemazh result { get; set; }
        }

    }
}

