using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK16FunctionFilter.Filters
{
    public sealed class ContextEnhancementFilter : IFunctionInvocationFilter
    {
        public async Task OnFunctionInvocationAsync(FunctionInvocationContext context, Func<FunctionInvocationContext, Task> next)
        {
            try
            {
                var currentUser = new { Name = "Shengjie", UserId = "shengjie", Phone = "1234567890", Email = "ysjxxx@live.com" };
                // context.Arguments.Add("CurrentUser", currentUser);
                context.Kernel.Data.Clear();
                context.Kernel.Data.TryAdd("CurrentUserId", currentUser.UserId);

                // Try to invoke function
                await next(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"方法请求错误: {ex.Message}");
                context.Result = new FunctionResult(context.Result, "Function invocation failed. Please check...");
            }
        }
    }
}
