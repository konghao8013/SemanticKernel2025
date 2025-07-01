using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK17FunctionCallingLog.Filters
{
    public sealed class FunctionCallLogFilter(ILogger<FunctionCallLogFilter> logger) : IFunctionInvocationFilter
    {
        public async Task OnFunctionInvocationAsync(FunctionInvocationContext context, Func<FunctionInvocationContext, Task> next)
        {
            try
            {
                logger.LogInformation($"Invoking function {context.Function.PluginName}-{context.Function.Name} with parameters: {context.Arguments}");
                // Try to invoke function
                await next(context);
                logger.LogInformation($"Function {context.Function.PluginName}-{context.Function.Name} invoked successfully with result: {context.Result}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"KH Error invoking function {context.Function.PluginName}-{context.Function.Name}: {ex.Message}");
                context.Result = new FunctionResult(context.Result, "Function invocation failed. Please check...");
            }
        }
    }
}
