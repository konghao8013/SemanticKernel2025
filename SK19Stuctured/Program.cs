// See https://aka.ms/new-console-template for more information


using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SK18FunctionCallingReturn.Plugins;
using SK19Stuctured.Models;
using System.Text.Json;

//Console.WriteLine("请输入AI服务提供商编码：");
//var aiProviderCode = Console.ReadLine();

var kernel = ServiceExtensions.GetKernel("azure-openai");
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

//var executionSettings = new OpenAIPromptExecutionSettings
//{
//    ResponseFormat = "json_object"
//};

//var result = await kernel.InvokePromptAsync("有史以来最受欢迎五大香港电影?使用Json格式返回", new(executionSettings));

//Console.WriteLine(result.ToString());

//Console.WriteLine(result);


var executionSettings = new OpenAIPromptExecutionSettings
{
    ResponseFormat = typeof(MovieResult)
};

// Send a request and pass prompt execution settings with desired response format.
var result = await kernel.InvokePromptAsync("有史以来最受欢迎的五大香港电影", new(executionSettings));
Console.WriteLine(result.ToString());



Console.WriteLine("Hello, World!");
