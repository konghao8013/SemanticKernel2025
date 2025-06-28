using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SK16FunctionFilter.Filters
{
    public sealed class FunctionCallsAuditFilter : IAutoFunctionInvocationFilter
    {
        public async Task OnAutoFunctionInvocationAsync(AutoFunctionInvocationContext context, Func<AutoFunctionInvocationContext, Task> next)
        {
            //var options = new JsonSerializerOptions
            //{
            //    WriteIndented = true,       // 启用缩进
            //    IndentCharacter = ' ',      // 缩进字符（默认是空格，可选 '\t'）
            //    IndentSize = 4              // 缩进大小（默认 2）
            //};

            //Console.WriteLine(JsonSerializer.Serialize(context, options));
            var chatHistory = context.ChatHistory;
            var functionCalls = FunctionCallContent.GetFunctionCalls(chatHistory.Last()).ToArray();
            if (functionCalls is { Length: > 0 })
            {
                foreach (var functionCall in functionCalls)
                {
                    Console.WriteLine($"方法请求：Request #{context.RequestSequenceIndex}. Function call: {functionCall.PluginName}.{functionCall.FunctionName}.");
                }
            }
            await next(context);
        }
    }
}
