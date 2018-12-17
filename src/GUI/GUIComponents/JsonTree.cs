using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using IconFonts;
using ImGuiNET;
using JsonAnything.Json;
using JsonAnything.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NJsonSchema;
using OpenTK;

namespace JsonAnything.GUI.GUIComponents
{
    public class JsonTree : ImGuiComponent
    {
        private string text = "";
        private JsonNode _json;
        private readonly JsonSerializer _jsonSerializer = new JsonSerializer();
        private bool _rootTreeOpen = true;
        private string _jsonFileName = "";

        private JsonSchema4 _schema;
        private SchemaValidationEventHandler _validationEventHandler;

        // used for generating IDs
        private int _renderCount = 0;

        public JsonTree(MainWindow window)
            : base(window)
        {
            _jsonSerializer.Formatting = Formatting.Indented;
            _jsonSerializer.Converters.Add(new JsonNodeConverter());
            _validationEventHandler = (sender, args) =>
            {
                ImGui.PushStyleColor(ColorTarget.Text, new System.Numerics.Vector4(1, 0, 0, 1));
                ImGui.SameLine(ImGui.GetColumnWidth(0) - 16 - ImGui.GetStyle().FramePadding.X);
                ImGui.Text(FontAwesome5.ExclamationCircle);
                if (ImGui.IsItemHovered(HoveredFlags.Default))
                {
                    ImGuiNative.igBeginTooltip();
                    ImGui.Text(ValidationErrorStringConverter.Convert(args.ValidationErrors[0], args.Node));
                    ImGuiNative.igEndTooltip();
                }
                ImGui.PopStyleColor();
            };
        }

        public void LoadJson(string filePath)
        {
            // TODO: exception checking
            
            _jsonFileName = filePath;
            using (StreamReader sr = new StreamReader(filePath))
            using (JsonReader jr = new JsonTextReader(sr))
            {
                _json = _jsonSerializer.Deserialize<JsonNode>(jr);
            }

            _window.SetTitle(Path.GetFileName(filePath));
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
        
        public override void Render()
        {
            if (_json == null) return;

            _renderCount = 0;
            
            ImGui.Columns(2, "columns", true);
            ImGui.Separator();
            ImGui.PushID(_jsonFileName);

            ImGui.AlignTextToFramePadding();
            ImGui.PushItemWidth(-1);
            ImGui.SetNextTreeNodeOpen(_rootTreeOpen);
                
            renderNode(_json);

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
            {
                _schema = JsonSchema4.FromJsonAsync(sr.ReadToEnd()).Result;
                if (_schema.SchemaVersion == null)
                {
                    // schema is not a valid JSON schema
                    throw new JsonException("Invalid JSON schema, schema needs $schema tag!");
                }
            }

            _json = createJsonFromSchema(_schema);
        }

        public void UnloadSchema()
        {

        }

        private JsonNode createJsonFromSchema(JsonSchema4 schema)
        {
            // TODO: more comprehensive schema stuff like default values

            JsonObjectType type = schema.Type;
            int typeFlags = (int)type;

            if (type.HasFlag(JsonObjectType.Boolean))
            {
                return new JsonNode(false, NodeType.Boolean, new JValue(false), schema);
            }
            if (type.HasFlag(JsonObjectType.Integer))
            {
                return new JsonNode(0, NodeType.Integer, new JValue(0L), schema);
            }
            if (type.HasFlag(JsonObjectType.Number))
            {
                return new JsonNode(0, NodeType.Number, new JValue(0F), schema);
            }
            if (type.HasFlag(JsonObjectType.Null))
            {
                return new JsonNode(null, NodeType.Null, new JValue(new object()), schema);
            }

            if (type.HasFlag(JsonObjectType.String))
            {
                return new JsonNode("", NodeType.String, new JValue(""), schema);
            }

            if (type.HasFlag(JsonObjectType.Array))
            {
                // TODO: "contains" keyword
                // TODO: "minItems" "maxItems" keyword
                // TODO: "uniqueItems" keyword
                        
                List<JsonNode> array = new List<JsonNode>();
                if (schema.Items.Count == 1)
                {
                    array.Add(createJsonFromSchema(schema.Item));
                }
                else
                {
                    array.AddRange(schema.Items.Select(createJsonFromSchema));
                }

                return new JsonNode(array, NodeType.Array, new JArray(array), schema);
            }

            if (type.HasFlag(JsonObjectType.Object))
            {
                // TODO: "minProperties" keyword
                        
                Dictionary<string, JsonNode> obj = new Dictionary<string, JsonNode>();

                foreach (KeyValuePair<string, JsonProperty> property in schema.Properties)
                {
                    obj[property.Key] = createJsonFromSchema(property.Value);
                }

                return new JsonNode(obj, NodeType.Object, new JObject(obj), schema);
            }

            return new JsonNode(null, NodeType.Null, new JValue(new object()), schema);
        }

        private void renderNode(JsonNode node)
        {
            switch (node.Type)
            {
                case NodeType.String:
                {
                    renderString(node);
                    break;
                }
                case NodeType.Number:
                {
                    renderFloat(node);
                    break;
                }
                case NodeType.Integer:
                {
                    renderInt(node);
                    break;
                }
                case NodeType.Boolean:
                {
                    renderBool(node);
                    break;
                }
                case NodeType.Null:
                {
                    renderNull(node);
                    break;
                }
                case NodeType.Array:
                {
                    renderArray(node);
                    break;
                }
                case NodeType.Object:
                {
                    renderObject(node);
                    break;
                }
                default:
                {
                    ImGui.Text("Nothing here!");
                    ImGui.NextColumn();
                    ImGui.NextColumn();
                    break;
                }
            }
        }

        private void renderString(JsonNode node)
        {
            ImGui.AlignTextToFramePadding();
            string key = node.Key ?? "string";
            string s = node.AsString;
            ImGui.Text(key);
            ImGui.NextColumn();
            ImGui.PushItemWidth(-1);
            ImGuiNETExtensions.InputText($"##{_renderCount}", ref s);
            ImGui.PopItemWidth();
            ImGui.NextColumn();
            if (node.Schema != null && !node.AsString.Equals(s))
            {
                //node.Token.Validate(node.Schema, _validationEventHandler);
            }
            node.AsString = s;
            _renderCount++;
        }

        private void renderFloat(JsonNode node)
        {
            ImGui.AlignTextToFramePadding();
            string key = node.Key ?? "float";
            float f = node.AsFloat;
            ImGui.Text(key);
            ImGui.NextColumn();
            ImGui.PushItemWidth(-1);
            ImGui.DragFloat($"##{_renderCount}", ref f, float.MinValue, float.MaxValue, 0.01f, "%.4f");
            ImGui.PopItemWidth();
            ImGui.NextColumn();
            //if (Math.Abs(node.AsFloat - f) > float.Epsilon) _jsonDirty = true;
            node.AsFloat = f;
            _renderCount++;
        }

        private void renderInt(JsonNode node)
        {
            ImGui.AlignTextToFramePadding();
            string key = node.Key ?? "int";
            int i = node.AsInt;
            ImGui.Text(key);
            node.Validate(_validationEventHandler);
            ImGui.NextColumn();
            ImGui.PushItemWidth(-1);
            ImGui.DragInt($"##{_renderCount}", ref i, 1.0f, int.MinValue, int.MaxValue, i.ToString());
            ImGui.PopItemWidth();
            ImGui.NextColumn();
            //if (node.Schema != null) node.Token.Validate(node.Schema, _validationEventHandler);
            node.AsInt = i;
            _renderCount++;
        }

        private void renderBool(JsonNode node)
        {
            ImGui.AlignTextToFramePadding();
            string key = node.Key ?? "bool";
            bool b = node.AsBool;
            ImGui.Text(key);
            ImGui.NextColumn();
            ImGui.PushItemWidth(-1);
            ImGui.Checkbox($"##{_renderCount}", ref b);
            ImGui.PopItemWidth();
            ImGui.NextColumn();
            node.AsBool = b;
            _renderCount++;
        }

        private void renderNull(JsonNode node)
        {
            ImGui.AlignTextToFramePadding();
            string key = node.Key ?? "null";
            string s = "null";
            ImGui.Text(key);
            ImGui.NextColumn();
            ImGui.PushItemWidth(-1);
            ImGuiNETExtensions.InputText($"##{node.GetHashCode()}", ref s, 1024U, InputTextFlags.ReadOnly);
            ImGui.PopItemWidth();
            ImGui.NextColumn();
            _renderCount++;
        }

        private void renderArray(JsonNode node)
        {
            ImGui.AlignTextToFramePadding();
            string key = node.Key ?? "array";
            bool arrayNode = ImGui.TreeNode($"{key}[{node.AsList.Count}]##{node.GetHashCode()}");
            ImGui.NextColumn();
            ImGui.NextColumn();

            if (arrayNode)
            {
                ImGui.PushID(key);
                
                foreach (JsonNode n in node.AsList)
                {
                    renderNode(n);
                }
                ImGui.TreePop();
                ImGui.PopID();
            }
            _renderCount++;
        }

        private void renderObject(JsonNode node)
        {
            ImGui.AlignTextToFramePadding();
            string key = node.Key ?? "object";
            bool objectNode = ImGui.TreeNode(key);
            ImGui.NextColumn();
            ImGui.NextColumn();
            
            if (objectNode)
            {
                ImGui.PushID(key);
                foreach (KeyValuePair<string, JsonNode> kv in node.AsDictionary)
                {
                    renderNode(kv.Value);
                }
                ImGui.TreePop();
                ImGui.PopID();
            }
            _renderCount++;
        }
    }
}
