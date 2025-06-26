using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK16FunctionFilter
{
    public sealed class ExceptionHandleFilter : IFunctionInvocationFilter
    {
       
        public ExceptionHandleFilter()
        {
          
        }
        public async Task OnFunctionInvocationAsync(FunctionInvocationContext context, Func<FunctionInvocationContext, Task> next)
        {
            try
            {
                // Try to invoke function
                await next(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Function invocation failed: {ex.Message}");
                context.Result = new FunctionResult(context.Result, "Function invocation failed. Please check...");
            }
        }
    }
}
