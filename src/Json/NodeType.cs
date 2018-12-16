using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Schema;

namespace JsonAnything.Json
{
    [Flags]
    public enum NodeType
    {
        String = JSchemaType.String,
        Number = JSchemaType.Number,
        Integer = JSchemaType.Integer,
        Boolean = JSchemaType.Boolean,
        Array = JSchemaType.Array,
        Object = JSchemaType.Object,
        Null = JSchemaType.Null,
    }
}
