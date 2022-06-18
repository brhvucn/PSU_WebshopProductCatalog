using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.ValueObjects;

namespace Webshop.Domain.Common
{
    public static class Errors
    {
        public static class General
        {
            public static Error UnspecifiedError(string message) => new Error("unspecified.error", message);
            public static Error NotFound<T>(T id) where T : struct => new Error("entity.not.found", $"Could not find entity with ID {id}.", statusCode: 404);
            public static Error ValueIsRequired(string valueName) => new Error("value.is.required", $"Value '{valueName}' is required.");
            public static Error ValueTooSmall(string valueName, int minValue) => new Error("value.too.small", $"Value '{valueName}' should be at least {minValue}.");
            public static Error ValueTooLarge(string valueName, int maxValue) => new Error("value.too.large", $"Value '{valueName}' should not exceed {maxValue}.");
            public static Error UnexpectedValue(string value) => new Error("unexpected.value", $"Value '{value}' is not valid in this context");
            public static Error Unauthorized() => new Error("unauthorizaed", $"Could not authorize access to entity");
            public static Error ValueIsEmpty(string value) => new Error("value.empty", $"The value cannot be empty: {value} ");

            public static Error ValueOutOfRange(string valueName, int minValue, int maxValue) =>
                new Error("value.out.of.Range", $"Value '{valueName}' should be between {minValue} and {maxValue}.");
        }
    }
}
