using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NJsonSchema;
using NJsonSchema.Validation;

namespace JsonAnything.Json
{
    public class JsonNode : IList<JsonNode>, IDictionary<string, JsonNode>
    {
        private dynamic _internalValue;

        public NodeType Type { get; }

        public string Key { get; set; }

        public JsonSchema4 Schema { get; }

        public JToken Token { get; private set; }

        public string AsString
        {
            get => _internalValue as string;
            set => _internalValue = value;
        }

        public float AsFloat
        {
            get => (float)_internalValue;
            set => _internalValue = value;
        }

        public int AsInt
        {
            get => (int)_internalValue;
            set
            {
                _internalValue = value;
                Token = new JValue(value);
            }
        }

        public bool AsBool
        {
            get => (bool)_internalValue;
            set => _internalValue = value;
        }

        public bool IsNull => _internalValue == null;

        public IList<JsonNode> AsList
        {
            get => _internalValue as IList<JsonNode>;
            set => _internalValue = value;
        }

        public IDictionary<string, JsonNode> AsDictionary
        {
            get => _internalValue as IDictionary<string, JsonNode>;
            set => _internalValue = value;
        }

        public JsonNode(dynamic o, NodeType type, JToken token = null, JsonSchema4 schema = null)
        {
            _internalValue = o;
            Type = type;
            Schema = schema;
            Token = token;
        }

        public static implicit operator string(JsonNode node) { return node.AsString; }

        public IEnumerator<JsonNode> GetEnumerator() { return AsList.GetEnumerator(); }

        IEnumerator<KeyValuePair<string, JsonNode>> IEnumerable<KeyValuePair<string, JsonNode>>.GetEnumerator()
        {
            return AsDictionary.GetEnumerator();
        }

        public void Add(KeyValuePair<string, JsonNode> item)
        {
            AsDictionary.Add(item);
        }

        public void Clear() { AsDictionary.Clear(); }

        public bool Contains(KeyValuePair<string, JsonNode> item)
        {
            return AsDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, JsonNode>[] array, int arrayIndex)
        {
            AsDictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, JsonNode> item)
        {
            return AsDictionary.Remove(item);
        }

        int ICollection<KeyValuePair<string, JsonNode>>.Count => AsList.Count;
        bool ICollection<KeyValuePair<string, JsonNode>>.IsReadOnly => AsList.IsReadOnly;

        public bool ContainsKey(string key)
        {
            return AsDictionary.ContainsKey(key);
        }

        public void Add(string key, JsonNode value)
        {
            AsDictionary.Add(key, value);
        }

        public bool Remove(string key)
        {
            return AsDictionary.Remove(key);
        }

        public bool TryGetValue(string key, out JsonNode value)
        {
            return AsDictionary.TryGetValue(key, out value);
        }

        public JsonNode this[string key]
        {
            get => AsDictionary[key];
            set => AsDictionary[key] = value;
        }

        public ICollection<string> Keys => AsDictionary.Keys;

        public ICollection<JsonNode> Values => AsDictionary.Values;

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        public void Add(JsonNode item) { AsList.Add(item); }

        void ICollection<JsonNode>.Clear() { AsList.Clear(); }

        public bool Contains(JsonNode item) { return AsList.Contains(item); }

        public void CopyTo(JsonNode[] array, int arrayIndex) { AsList.CopyTo(array, arrayIndex); }

        public bool Remove(JsonNode item) { return AsList.Remove(item); }

        int ICollection<JsonNode>.Count => AsList.Count;

        bool ICollection<JsonNode>.IsReadOnly => AsList.IsReadOnly;

        public int IndexOf(JsonNode item) { return AsList.IndexOf(item); }

        public void Insert(int index, JsonNode item) { AsList.Insert(index, item); }

        public void RemoveAt(int index) { AsList.RemoveAt(index); }

        JsonNode IList<JsonNode>.this[int index]
        {
            get => AsList[index];
            set => AsList[index] = value;
        }

        public void Validate(SchemaValidationEventHandler handler)
        {
            if (Schema == null || Token == null) throw new JsonValidationException("Cannot validate a node with no Schema or Token");

            List<ValidationError> errors = Schema.Validate(Token).ToList();
            if (errors.Count > 0) handler(this, new SchemaValidationEventArgs(errors, this));
        }

        public override int GetHashCode()
        {
            switch (Type)
            {
                case NodeType.Array: return AsList.GetHashCode();
                case NodeType.Object: return AsDictionary.GetHashCode();
                case NodeType.Integer: return AsInt.GetHashCode();
                case NodeType.Boolean: return AsBool.GetHashCode();
                case NodeType.Null: return 0;
                case NodeType.Number: return AsFloat.GetHashCode();
                case NodeType.String: return AsString.GetHashCode();
                default: return -1;
            }
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;

            if (!(obj is JsonNode item)) return false;

            switch (Type)
            {
                case NodeType.Integer:
                {
                    if (item.Type == NodeType.Integer) return item.AsInt == AsInt;
                    break;
                }
                case NodeType.Number:
                {
                    if (item.Type == NodeType.Number) return Math.Abs(item.AsFloat - AsFloat) < float.Epsilon;
                    break;
                }
                case NodeType.Boolean:
                {
                    if (item.Type == NodeType.Boolean) return item.AsBool == AsBool;
                    break;
                }
                case NodeType.Null:
                {
                    if (item.Type == NodeType.Null) return true;
                    break;
                }
                case NodeType.String:
                {
                    if (item.Type == NodeType.String) return item.AsString.Equals(AsString);
                    break;
                }
                case NodeType.Object:
                {
                    if (item.Type == NodeType.Object) return item.AsDictionary.Equals(AsDictionary);
                    break;
                }
                case NodeType.Array:
                {
                    if (item.Type == NodeType.Array) return item.AsList.Equals(AsList);
                    break;
                }
                default: return false;
            }

            return false;
        }
    }
}
