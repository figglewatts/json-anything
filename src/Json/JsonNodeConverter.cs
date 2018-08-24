using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonAnything.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonAnything.Json
{
    public class JsonNodeConverter : JsonConverter<JsonNode>
    {
        public override void WriteJson(JsonWriter writer, JsonNode value, JsonSerializer serializer) { throw new NotImplementedException(); }

        public override JsonNode ReadJson(JsonReader reader,
            Type objectType,
            JsonNode existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            JToken token = JToken.ReadFrom(reader);
            return Convert(token);
        }

        public JsonNode Convert(JToken j)
        {
            switch (j.Type)
            {
                case JTokenType.String:
                {
                    return j.Value<string>();
                }
                case JTokenType.Boolean:
                {
                    return j.Value<bool>();
                }
                case JTokenType.Float:
                {
                    return j.Value<float>();
                }
                case JTokenType.Integer:
                {
                    return j.Value<int>();
                }
                case JTokenType.Null:
                {
                    return new JsonNode(null, NodeType.Null);
                }
                case JTokenType.Array:
                {
                    List<JsonNode> arr = new List<JsonNode>();

                    foreach (JToken t in j)
                    {
                        arr.Add(Convert(t));
                    }

                    return arr;
                }
                case JTokenType.Object:
                {
                    Dictionary<string, JsonNode> obj = new Dictionary<string, JsonNode>();

                    foreach (JProperty p in j.Children<JProperty>())
                    {
                        obj[p.Name] = Convert(p.Value);
                    }

                    return obj;
                }
                default:
                {
                    Logger.Log()(LogLevel.WARN, "Unknown Token type {0}", j.Type.ToString());
                    return null;
                }
            }
        }
    }
}
