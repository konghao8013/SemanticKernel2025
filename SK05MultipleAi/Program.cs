// See https://aka.ms/new-console-template for more information

//using SK05MultipleAi;

//var aiOptions = AiSettings.LoadAiProvidersFromFile();
//Console.WriteLine("Hello, World!");

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using SK05MultipleAi;


// 引入交互式的内核命名空间，以便用户输入
Console.WriteLine("请输入AI服务提供商的编码（例如：OpenAI、AzureOpenAI、OpenAI_Compatible等）：");

var aiProviderCode = Console.ReadLine(); ;

//Create Kernel builder
var builder = Kernel.CreateBuilder();

builder.AddChatCompletionService(aiProviderCode);

var kernel = builder.Build();

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
var response = await chatCompletionService.GetChatMessageContentAsync("你是谁？");
Console.WriteLine(response.Content);
