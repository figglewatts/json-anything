using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonAnything.Util;
using NJsonSchema;
using NJsonSchema.Validation;

namespace JsonAnything.Json
{
    public static class ValidationErrorStringConverter
    {
        private static Dictionary<ValidationErrorKind, Func<ValidationError, JsonNode, string>> _conversionMap =
            new Dictionary<ValidationErrorKind, Func<ValidationError, JsonNode, string>>
            {
                { ValidationErrorKind.NumberTooSmall, (error, node) => $"Number {(node.Type == NodeType.Integer ? node.AsInt : node.AsFloat)} too small, minimum: {error.Schema.Minimum}"},
                { ValidationErrorKind.NumberTooBig, (error, node) => $"Number {(node.Type == NodeType.Integer ? node.AsInt : node.AsFloat)} too big, maximum: {error.Schema.Maximum}"}
            };

        public static string Convert(ValidationError error, JsonNode node)
        {
            return _conversionMap[error.Kind](error, node);
        }
    }
}
