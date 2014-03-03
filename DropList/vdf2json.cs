using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SteamKit2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drop
{
	class Vdf2Json
	{


        public static String ConvertVdf2Json(Stream inputFileStream)
		{
				var kv = new KeyValue();
				kv.ReadAsText(inputFileStream);
				return Convert(kv,false);
		}

		static string Convert(KeyValue kv, bool compactJSON)
		{
			var jo = new JObject();
			var rootNode = new JObject();
			jo[kv.Name] = ConvertRecursive(kv);
            StringWriter textWriter = new StringWriter();
            JsonWriter jsonWriter = new JsonTextWriter(textWriter);
			jsonWriter.Formatting = compactJSON ? Formatting.None : Formatting.Indented;
			jo.WriteTo(jsonWriter);
            return textWriter.GetStringBuilder().ToString();
		}

		static JToken ConvertRecursive(KeyValue kv)
		{
			var jo = new JObject();

			if (kv.Children.Count > 0)
			{
				foreach (var child in kv.Children)
				{
					jo[child.Name] = ConvertRecursive(child);
				}
				return jo;
			}
			else
			{
				return (JToken)kv.Value;
			}
		}
	}
}
