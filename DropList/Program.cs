using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Drop
{
    class Program
    {
        static void Main(string[] args)
        {
            DropList list = Newtonsoft.Json.JsonConvert.DeserializeObject<DropList>("{\"drops\":{\"common_drops\":[],\"uncommon_drops\":[],\"rare_drops\":[],\"mythical_drops\":[],\"legendary_drops\":[],\"arcana_drops\":[]},\"craftings\":{\"crafting_common\":[],\"crafting_uncommon\":[],\"crafting_rare\":[],\"crafting_mythical\":[],\"crafting_legendary\":[],\"crafting_arcana\":[],\"crafting_exceptional\":[]}}");
            Drop.Schemazh ss = Drop.Schemazh.FetchSchema();
            Drop.Dota2 dd = Drop.Dota2.FetchSchema(ss.ItemsGameUrl);
            string xx = "普通掉落列表:\r\n";
            StreamWriter wf;
            //xx = "普通掉落列表:\r\n\r\n";
            xx = "";
            DropList.Item item = new DropList.Item();
            foreach (var x in dd.loot_lists.common_drops)
            {
                item = new DropList.Item();
                item.Name =ss.GetZh(x.Key);
                item.Url = ss.GetPic(x.Key) ;
                list.Drops.common_drops.Add(item);
            }
            wf = new StreamWriter("drop_common.txt", false);

            wf.WriteLine(xx);
            wf.Close();
            Console.WriteLine("ok");

        }
    }
}
