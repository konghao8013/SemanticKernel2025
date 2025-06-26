using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SK13PromptFilter.Filters
{
    public sealed class SensitiveInfoFilter : IPromptRenderFilter
    {
        private static readonly Dictionary<string, string> SensitivePatterns = new()
    {
        { "信用卡", @"\b(?:\d{4}[-\s]?){3}\d{4}\b" },
        { "邮箱", @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}\b" },
        { "手机号", @"\b(?:\+?\d{1,3}[-\s]?)?\(?\d{3}\)?[-\s]?\d{3}[-\s]?\d{4}\b" },
        { "身份证", @"\b\d{15}|\d{18}\b" }
    };

        public async Task OnPromptRenderAsync(PromptRenderContext context, Func<PromptRenderContext, Task> next)
        {
            await next(context);

            string prompt = context.RenderedPrompt;

            foreach (var pattern in SensitivePatterns)
            {
                prompt = Regex.Replace(prompt, pattern.Value, $"[REDACTED {pattern.Key}]", RegexOptions.IgnoreCase);
            }

            context.RenderedPrompt = prompt;
        }
    }
}
