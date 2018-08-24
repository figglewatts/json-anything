using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace JsonAnything.Json
{
    public class JsonNode : IList<JsonNode>, IDictionary<string, JsonNode>
    {
        private object _internalValue;
        private int _count;
        private bool _isReadOnly;

        public NodeType Type { get; private set; }

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
            set => _internalValue = value;
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

        public JsonNode(object o, NodeType type)
        {
            _internalValue = o;
            Type = type;
        }

        public static implicit operator JsonNode(string s)
        {
            return new JsonNode(s, NodeType.String);
        }

        public static implicit operator string(JsonNode node) { return node.AsString; }

        public static implicit operator JsonNode(double d)
        {
            return new JsonNode(d, NodeType.Float);
        }

        public static implicit operator JsonNode(float f)
        {
            return new JsonNode(f, NodeType.Float);
        }

        public static implicit operator JsonNode(int i)
        {
            return new JsonNode(i, NodeType.Int);
        }

        public static implicit operator JsonNode(bool b)
        {
            return new JsonNode(b, NodeType.Bool);
        }

        public static implicit operator JsonNode(List<JsonNode> a)
        {
            return new JsonNode(a, NodeType.Array);
        }

        public static implicit operator JsonNode(Dictionary<string, JsonNode> o)
        {
            return new JsonNode(o, NodeType.Object);
        }

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
    }
}
