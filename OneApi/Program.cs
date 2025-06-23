using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.Extensions.DependencyInjection;

// 引入交互式的内核命名空间，以便用户输入
Console.WriteLine("欢迎使用OneAPI示例程序！");

// var oneApiKey = await PolyglotKernel.GetInputAsync("请输入您的OneAPI Key："); 
var oneApiKey = "sk-T4Qx0V1APvF7D8tZ1f554e51Ad71499a93AdB3F43aE29c1f";

// Create kernel builder
var builder = Kernel.CreateBuilder();

var oneApiEndpoint = new Uri("http://192.168.3.126:3000/v1");
// Add OpenAI Chat completion
builder.AddOpenAIChatCompletion(
    modelId: "deepseek-chat",
    apiKey: oneApiKey,
    endpoint: oneApiEndpoint);
// Build kernel
var kernel = builder.Build();

var response = await kernel.InvokePromptAsync("介绍下OneApi的使用场景和优势");
Console.WriteLine(response.GetValue<string>());
Console.ReadLine();