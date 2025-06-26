// See https://aka.ms/new-console-template for more information


using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SK18FunctionCallingReturn.Plugins;

Console.WriteLine("请输入AI服务提供商编码：");
var aiProviderCode = Console.ReadLine();

var kernel = ServiceExtensions.GetKernel(aiProviderCode);
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();


kernel.Plugins.Clear();
kernel.ImportPluginFromType<WeatherPlugin1>();

OpenAIPromptExecutionSettings settings = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };
FunctionResult result = await kernel.InvokePromptAsync("What is the current weather?", new(settings));
Console.WriteLine(result);

Console.WriteLine("Hello, World!");
