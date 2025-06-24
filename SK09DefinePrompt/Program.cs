using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Text.Json;
using System.Text.Json.Serialization;

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
// See https://aka.ms/new-console-template for more information


//var article = """
//9月6日16时20分，今年第11号超强台风“摩羯”在海南文昌沿海登陆。超强台风“摩羯”风力有多大？记者只能“抱团”出镜。
//""";

//var prompt = $"""
//请使用 Emoji 风格编辑以下段落，该风格以引人入胜的标题、每个段落中包含表情符号和在末尾添加相关标签为特点。
//请确保保持原文的意思。
//{article}
//""";
//// 参数配置
//PromptExecutionSettings settings = new OpenAIPromptExecutionSettings()
//{
//    Temperature = 1,
//    TopP = 1,
//    FrequencyPenalty = 0,
//    PresencePenalty = 0
//};

//Console.WriteLine(JsonSerializer.Serialize(settings));

//var response = await chatCompletionService.GetChatMessageContentAsync(prompt, settings);
//Console.WriteLine($"AI Response: {response.Content}");


var prompt = """
请使用 Emoji 风格编辑以下段落，该风格以引人入胜的标题、每个段落中包含表情符号和在末尾添加相关标签为特点。
请确保保持原文的意思。
{{ $article }}
""";
// 参数配置
PromptExecutionSettings settings = new OpenAIPromptExecutionSettings()
{
    Temperature = 1,
    TopP = 1,
    FrequencyPenalty = 0,
    PresencePenalty = 0
};

var rewriteFunction = kernel.CreateFunctionFromPrompt(prompt, settings);
var article = """
9月6日16时20分，今年第11号超强台风“摩羯”在海南文昌沿海登陆。超强台风“摩羯”风力有多大？记者只能“抱团”出镜。
""";

var rewriteResult = await kernel.InvokeAsync(rewriteFunction, new() { ["article"] = article });
 Console.WriteLine($"AI Response: {rewriteResult.GetValue<string>()}");

Console.WriteLine("Hello, World!");
