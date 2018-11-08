using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonAnything.Json
{
    [Flags]
    public enum NodeType
    {
        String = 1 << 0,
        Number = 1 << 1,
        Integer = 1 << 2,
        Boolean = 1 << 3,
        Array = 1 << 4,
        Object = 1 << 5,
        Null = 1 << 6
    }
}
