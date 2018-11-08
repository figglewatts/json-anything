using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using JsonAnything.Json;
using JsonAnything.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace JsonAnything.GUI.GUIComponents
{
    public class JsonTree : IImGuiComponent
    {
        private string text = "";
        private Dictionary<string, JsonNode> _json;
        private JsonSerializer _jsonSerializer = new JsonSerializer();
        private bool _rootTreeOpen = true;
        private string _jsonFileName = "";

        private JSchema _schema;

        public JsonTree()
        {
            _jsonSerializer.Formatting = Formatting.Indented;
            _jsonSerializer.Converters.Add(new JsonNodeConverter());
        }

        public void LoadJson(string filePath)
        {
            // TODO: exception checking
            
            _jsonFileName = filePath;
            using (StreamReader sr = new StreamReader(filePath))
            using (JsonReader jr = new JsonTextReader(sr))
            {
                _json = _jsonSerializer.Deserialize<Dictionary<string, JsonNode>>(jr);
            }
        }

        public void SaveJson(string filePath)
        {
            // TODO: exception checking
            
            using (StreamWriter sw = new StreamWriter(filePath))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                _jsonSerializer.Serialize(jw, _json);
            }
        }
        
        public void Render()
        {
            if (_json == null) return;
            
            ImGui.Columns(2, "columns", true);
            ImGui.Separator();
            ImGui.PushID(_jsonFileName);

            ImGui.AlignTextToFramePadding();
            ImGui.PushItemWidth(-1);
            ImGui.SetNextTreeNodeOpen(_rootTreeOpen);
            if (ImGui.TreeNode("test.json"))
            {
                ImGui.NextColumn();
                ImGui.NextColumn();
                foreach (var key in _json.Keys)
                {
                    renderSwitch(key, _json[key]);
                }

                ImGui.TreePop();
            }

            ImGui.PopID();
            ImGui.PopItemWidth();
            ImGui.Columns(1, "columns", false);
            ImGui.Separator();
        }

        public void LoadSchema(string schemaFilePath)
        {
            // TODO: exception checking
            // TODO: logic here for loading a schema with a JSON file already loaded
            
            using (StreamReader sr = new StreamReader(schemaFilePath))
            using (JsonReader jr = new JsonTextReader(sr))
            {
                _schema = JSchema.Load(jr);
            }
        }

        private void renderSwitch(string key, JsonNode node)
        {
            switch (node.Type)
            {
                case NodeType.String:
                {
                    renderString(key, node);
                    break;
                }
                case NodeType.Number:
                {
                    renderFloat(key, node);
                    break;
                }
                case NodeType.Integer:
                {
                    renderInt(key, node);
                    break;
                }
                case NodeType.Boolean:
                {
                    renderBool(key, node);
                    break;
                }
                case NodeType.Null:
                {
                    renderNull(key, node);
                    break;
                }
                case NodeType.Array:
                {
                    renderArray(key, node);
                    break;
                }
                case NodeType.Object:
                {
                    renderObject(key, node);
                    break;
                }
                default:
                {
                    ImGui.Text(key);
                    break;
                }
            }
        }

        private void renderString(string key, JsonNode node)
        {
            ImGui.AlignTextToFramePadding();
            string s = node.AsString;
            ImGui.Text(key);
            ImGui.NextColumn();
            ImGui.PushItemWidth(-1);
            ImGuiNETExtensions.InputText($"##{key}", ref s);
            ImGui.PopItemWidth();
            ImGui.NextColumn();
            node.AsString = s;
        }

        private void renderFloat(string key, JsonNode node)
        {
            ImGui.AlignTextToFramePadding();
            float f = node.AsFloat;
            ImGui.Text(key);
            ImGui.NextColumn();
            ImGui.PushItemWidth(-1);
            ImGui.DragFloat($"##{key}", ref f, float.MinValue, float.MaxValue, 0.01f, "%.4f");
            ImGui.PopItemWidth();
            ImGui.NextColumn();
            node.AsFloat = f;
        }

        private void renderInt(string key, JsonNode node)
        {
            ImGui.AlignTextToFramePadding();
            int i = node.AsInt;
            ImGui.Text(key);
            ImGui.NextColumn();
            ImGui.PushItemWidth(-1);
            ImGui.DragInt($"##{key}", ref i, 1.0f, int.MinValue, int.MaxValue, i.ToString());
            ImGui.PopItemWidth();
            ImGui.NextColumn();
            node.AsInt = i;
        }

        private void renderBool(string key, JsonNode node)
        {
            ImGui.AlignTextToFramePadding();
            bool b = node.AsBool;
            ImGui.Text(key);
            ImGui.NextColumn();
            ImGui.PushItemWidth(-1);
            ImGui.Checkbox($"##{key}", ref b);
            ImGui.PopItemWidth();
            ImGui.NextColumn();
            node.AsBool = b;
        }

        private void renderNull(string key, JsonNode node)
        {
            ImGui.AlignTextToFramePadding();
            string s = "null";
            ImGui.Text(key);
            ImGui.NextColumn();
            ImGui.PushItemWidth(-1);
            ImGuiNETExtensions.InputText($"##{key}", ref s, 1024U, InputTextFlags.ReadOnly);
            ImGui.PopItemWidth();
            ImGui.NextColumn();
        }

        private void renderArray(string key, JsonNode node)
        {
            ImGui.AlignTextToFramePadding();
            bool arrayNode = ImGui.TreeNode($"{key}[{node.AsList.Count}]");
            ImGui.NextColumn();
            ImGui.NextColumn();

            if (arrayNode)
            {
                ImGui.PushID(key);
                
                int i = 0;
                foreach (JsonNode n in node.AsList)
                {
                    renderSwitch(i.ToString(), n);
                    i++;
                }
                ImGui.TreePop();
                ImGui.PopID();
            }
        }

        private void renderObject(string key, JsonNode node)
        {
            ImGui.AlignTextToFramePadding();
            bool objectNode = ImGui.TreeNode(key);
            ImGui.NextColumn();
            ImGui.NextColumn();
            
            if (objectNode)
            {
                ImGui.PushID(key);
                foreach (KeyValuePair<string, JsonNode> kv in node.AsDictionary)
                {
                    renderSwitch(kv.Key, kv.Value);
                }
                ImGui.TreePop();
                ImGui.PopID();
            }
        }
    }
}
