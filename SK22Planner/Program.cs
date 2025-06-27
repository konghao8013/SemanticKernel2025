// See https://aka.ms/new-console-template for more information

// 1. 配置自动函数调用
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

Console.WriteLine("请输入AI服务提供商编码：");
var aiProviderCode = Console.ReadLine();

var kernel = ServiceExtensions.GetKernel(aiProviderCode);
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

OpenAIPromptExecutionSettings settings = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };

// 2. 注册插件（自动暴露函数描述）
kernel.ImportPluginFromFunctions("FinancePlugin", [
    kernel.CreateFunctionFromMethod(()=>{
    return "20人名币";
    }, "GetStockPrice", "获取股票实时价格")
]);

// 3. 直接对话触发
var chatResult = await chatCompletionService.GetChatMessageContentAsync(
    "查询微软股票的最新价格", settings, kernel
);
Console.WriteLine(chatResult.Content);

Console.WriteLine("Hello, World!");
