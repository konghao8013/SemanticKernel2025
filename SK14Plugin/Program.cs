
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SK14Plugin.Plugins;
using System.Text.Json;

//var builder = Kernel.CreateBuilder();
//var kernel = builder.Build();

//kernel.Plugins.AddFromFunctions("time_plugin",
//[
//   KernelFunctionFactory.CreateFromMethod(
//        method: () => DateTime.Now,
//        functionName: "get_time",op
//        description: "Get the current time"
//    ),
//    KernelFunctionFactory.CreateFromMethod(
//        method: (DateTime start, DateTime end) => (end - start).TotalSeconds,
//        functionName: "diff_time",
//        description: "Get the difference between two times in seconds"
//    )
//]);

//var time = await kernel.InvokeAsync<DateTime>("time_plugin", "get_time", new());
//Console.WriteLine($"The current time is {time}.");

//kernel.Plugins.Clear();
//kernel.Plugins.AddFromType<MathPlugin>();
//// Test the math plugin
//double answer = await kernel.InvokeAsync<double>("MathPlugin", "Sqrt", new() { { "number1", 9 } });
//Console.WriteLine($"The square root of 9 is {answer}.");
//// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");



// 引入交互式的内核命名空间，以便用户输入
Console.WriteLine("请输入AI服务提供商编码：");
var aiProviderCode = Console.ReadLine();

var kernel = ServiceExtensions.GetKernel(aiProviderCode);
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();



// 注册插件
kernel.Plugins.Clear();
kernel.Plugins.AddFromType<MathPlugin>();
kernel.Plugins.AddFromFunctions("time_plugin",
[
   KernelFunctionFactory.CreateFromMethod(
       method: () => {
       return $"{DateTime.Now.AddHours(-8).ToString("yyyy年MM月dd日 HHmmss.fff")}";
       },
       functionName: "get_time",
       description: "Get the current time"
   )
]);


Console.WriteLine("请输入你的请求：");
var request=Console.ReadLine();
// Enable auto function calling
OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};
// Get the response from the AI
var result = await chatCompletionService.GetChatMessageContentAsync(
    prompt: request,
    executionSettings: openAIPromptExecutionSettings,
    kernel: kernel);




Console.WriteLine(JsonSerializer.Serialize(result));
Console.WriteLine($"AI Response: {result.Content}");
Console.WriteLine("How word!");