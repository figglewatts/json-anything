using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Schema;
using NJsonSchema;

namespace JsonAnything.Json
{
    [Flags]
    public enum NodeType
    {
        String = JsonObjectType.String,
        Number = JsonObjectType.Number,
        Integer = JsonObjectType.Integer,
        Boolean = JsonObjectType.Boolean,
        Array = JsonObjectType.Array,
        Object = JsonObjectType.Object,
        Null = JsonObjectType.Null
    }
}
