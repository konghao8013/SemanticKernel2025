using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK16FunctionFilter.Filters
{

    public class ApprovalFilter() : IFunctionInvocationFilter
    {
        public async Task OnFunctionInvocationAsync(FunctionInvocationContext context, Func<FunctionInvocationContext, Task> next)
        {
            if (context.Function.PluginName == "DynamicsPlugin" && context.Function.Name == "create_order")
            {
                Console.WriteLine("System > The agent wants to create an approval, do you want to proceed? (Y/N)");
                Console.WriteLine("Y/N");
                var shouldProceed = Console.ReadLine();

                if (shouldProceed != "Y")
                {
                    context.Result = new FunctionResult(context.Result, "The order creation was not approved by the user");
                    return;
                }
            }

            await next(context);
        }
    }
}
