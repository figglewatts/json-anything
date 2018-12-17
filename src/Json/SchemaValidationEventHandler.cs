using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJsonSchema.Validation;

namespace JsonAnything.Json
{
    public class SchemaValidationEventArgs : EventArgs
    {
        public List<ValidationError> ValidationErrors { get; }
        public JsonNode Node { get; }

        public SchemaValidationEventArgs(List<ValidationError> errors, JsonNode node)
        {
            ValidationErrors = errors;
            Node = node;
        }
    }

    public delegate void SchemaValidationEventHandler(object sender, SchemaValidationEventArgs args);
}
