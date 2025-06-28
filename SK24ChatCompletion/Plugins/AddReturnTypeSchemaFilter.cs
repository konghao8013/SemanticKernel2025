using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK18FunctionCallingReturn.Plugins
{
    /// <summary>
    /// A auto function invocation filter that replaces the original function's result with a new result that includes both the original result and its schema.
    /// </summary>
    public sealed class AddReturnTypeSchemaFilter : IAutoFunctionInvocationFilter
    {
        public async Task OnAutoFunctionInvocationAsync(AutoFunctionInvocationContext context, Func<AutoFunctionInvocationContext, Task> next)
        {
            // Invoke the function
            await next(context);
            // Crete the result with the schema
            FunctionResultWithSchema resultWithSchema = new()
            {
                Value = context.Result.GetValue<object>(),                  // Get the original result
                Schema = context.Function.Metadata.ReturnParameter?.Schema  // Get the function return type schema
            };
            // Return the result with the schema instead of the original one
            context.Result = new FunctionResult(context.Result, resultWithSchema);
        }
        private sealed class FunctionResultWithSchema
        {
            public object? Value { get; set; }
            public KernelJsonSchema? Schema { get; set; }
        }
    }
}
