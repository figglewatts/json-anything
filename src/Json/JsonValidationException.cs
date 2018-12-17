using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonAnything.Json
{
    public class JsonValidationException : Exception
    {
        public JsonValidationException() : base() {}

        public JsonValidationException(string message) : base(message) {}

        public JsonValidationException(string message, Exception inner) : base(message, inner) {}
    }
}
