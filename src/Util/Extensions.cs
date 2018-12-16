using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonAnything.Util
{
    public static class Extensions
    {
        public static T ToEnum<T>(this string value) { return (T)Enum.Parse(typeof(T), value, true); }

        public static IEnumerable<Enum> GetFlags(this Enum value)
        {
            foreach (Enum e in Enum.GetValues(value.GetType()))
            {
                if (value.HasFlag(e))
                {
                    yield return e;
                }
            }
        }
    }
}
