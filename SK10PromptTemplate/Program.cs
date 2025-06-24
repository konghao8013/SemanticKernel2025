
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Services;

Console.WriteLine("请输入AI服务提供商编码：");
var aiProviderCode = Console.ReadLine();

var kernel = ServiceExtensions.GetKernel(aiProviderCode);
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();


var executionSettings = new OpenAIPromptExecutionSettings()
{
    Temperature = 0.1f,
    MaxTokens = 50,
    TopP = 0.9f
};



async Task Chat(string userPrompt)
{
    var response = await chatCompletionService.GetChatMessageContentAsync(userPrompt, executionSettings);
    Console.WriteLine($"AI Response: {response.Content}");
}



//var prompt = "The weather today in {{ $location }} is {{GetForecast}}.";

//kernel.Plugins.Clear();
//kernel.Plugins.AddFromType<WeatherPlugin>();

//var res = await kernel.InvokePromptAsync(prompt, new() { ["location"] = "Shenzhen" });
//Console.WriteLine($"AI Response: {res.GetValue<String>()}");


var prompt = "The weather today in {{$city}} is {{GetForecast $city}}.";

kernel.Plugins.Clear();
kernel.Plugins.AddFromType<WeatherPlugin>();


var res = await kernel.InvokePromptAsync(prompt, new() { ["city"] = "Shenzhen" });
Console.WriteLine($"AI Response: {res.GetValue<String>()}");
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
