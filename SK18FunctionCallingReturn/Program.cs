// See https://aka.ms/new-console-template for more information


using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SK18FunctionCallingReturn.Plugins;

Console.WriteLine("请输入AI服务提供商编码：");
var aiProviderCode = Console.ReadLine();

var kernel = ServiceExtensions.GetKernel(aiProviderCode);
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();


//kernel.Plugins.Clear();
//kernel.ImportPluginFromType<WeatherPlugin1>();

//OpenAIPromptExecutionSettings settings = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };
//FunctionResult result = await kernel.InvokePromptAsync("What is the current weather?", new(settings));
//Console.WriteLine(result);

//kernel.Plugins.Clear();
//kernel.ImportPluginFromType<WeatherPlugin2>();

//OpenAIPromptExecutionSettings settings = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };
//FunctionResult result = await kernel.InvokePromptAsync("What is the current weather?", new(settings));
//Console.WriteLine(result);


kernel.AutoFunctionInvocationFilters.Add(new AddReturnTypeSchemaFilter());
// Import the plugin that provides descriptions for the return type properties.   
// This additional information is used when extracting the schema from the return type.
kernel.Plugins.Clear();
kernel.ImportPluginFromType<WeatherPlugin3>();
OpenAIPromptExecutionSettings settings = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };
FunctionResult result = await kernel.InvokePromptAsync("What is the current weather?", new(settings));
Console.WriteLine(result);
Console.WriteLine("Hello, World!");
