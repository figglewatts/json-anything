using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonAnything.Util
{
    /// <summary>
    /// Allows for pseudo-switching based on Type.
    /// See <see href="https://stackoverflow.com/questions/7252186/switch-case-on-type-c-sharp/7301514#7301514"></see>.
    /// 
    /// <example>
    /// <code>
    /// var ts = new TypeSwitch()
    ///     .Case((int x) = Console.WriteLine("int"))
    ///     .Case((bool x) = Console.WriteLine("bool"))
    ///     .Case((string x) = Console.WriteLine("string"));
    /// 
    /// ts.Switch(42);     
    /// ts.Switch(false);  
    /// ts.Switch("hello"); 
    /// </code>
    /// </example>
    /// </summary>
    public class TypeSwitch
    {
        private readonly Dictionary<Type, Action<object>> _matches = new Dictionary<Type, Action<object>>();
        private Action<object> _default;

        public TypeSwitch Case<T>(Action<T> action)
        {
            _matches.Add(typeof(T), (x) => action((T)x)); 
            return this;
        }
        
        public TypeSwitch Default(Action<object> action) 
        { 
            _default = action;
            return this;
        }

        public void Switch(object x)
        {
            if (_matches.TryGetValue(x.GetType(), out Action<object> action))
            {
                action(x);
            }
            else
            {
                _default(x);
            }
        }
    }
}
