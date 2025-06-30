using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK13PromptFilter.Filters
{
    public class MyPromptFilter : IPromptRenderFilter
    {
        public async Task OnPromptRenderAsync(PromptRenderContext context, Func<PromptRenderContext, Task> next)
        {
            // 渲染前的逻辑
            await next(context);
            // 渲染后的逻辑
        }
    }
}
