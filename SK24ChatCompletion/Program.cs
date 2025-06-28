// See https://aka.ms/new-console-template for more information



using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SK14Plugin.Plugins;
Console.WriteLine("请输入AI服务提供商编码：");
var aiProviderCode = Console.ReadLine();

var kernel = ServiceExtensions.GetKernel(aiProviderCode);
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();


//ChatCompletionAgent agent =
//    new()
//    {
//        Name = "QA-Agent",
//        Instructions = "Ask me anything!",
//        Kernel = kernel
//    };

//ChatHistory chat = [];

//Console.WriteLine("请输入您的问题：");

//// Add a user request to the chat history
//var userRequest = Console.ReadLine();
//ChatMessageContent message = new(AuthorRole.User, userRequest);
//chat.Add(message);

//await foreach (ChatMessageContent message2 in agent.InvokeAsync(chat))
//{
//    chat.Add(message2);
//}
//Console.WriteLine($"AI Response: {(string.Join("\r\n", chat.Select(a => a.Content)))}");



//ChatCompletionAgent agent =
//    new()
//    {
//        Name = "Math-Agent",
//        Instructions = "You can answer math question!",
//        Kernel = kernel,
//        Arguments = new KernelArguments(
//            new PromptExecutionSettings()
//            { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() }
//        ) // 配置自动调用插件
//    };

//// 导入插件
//var plugin = KernelPluginFactory.CreateFromType<MathPlugin>();
//agent.Kernel.Plugins.Add(plugin);

//ChatHistory chat = [];

//// Add a user request to the chat history
//Console.WriteLine("请输入您的问题：");
//var userRequest = Console.ReadLine();
//ChatMessageContent message = new(AuthorRole.User, userRequest);
//chat.Add(message);

//await foreach (ChatMessageContent message2 in agent.InvokeAsync(chat))
//{
//    chat.Add(message2);
//}
//Console.WriteLine($"AI Response: {(string.Join("\r\n", chat.Select(a => a.Content)))}");





//PromptTemplateConfig templateConfig = new()
//{
//    Template = "Translate into {{$language}}",
//    TemplateFormat = "semantic-kernel"
//};

//ChatCompletionAgent agent = new(
//    templateConfig: templateConfig,
//    templateFactory: new KernelPromptTemplateFactory())
//{
//    Name = "Translate-Agent",
//    Kernel = kernel,
//    Arguments = new KernelArguments(){
//            { "language", "Chinese" }
//        }
//};

//ChatHistory chat = [];


//Console.WriteLine("请输入您的问题：");
//var userRequest = Console.ReadLine();
//ChatMessageContent message = new(AuthorRole.User, userRequest);
//chat.Add(message);

//await foreach (ChatMessageContent message2 in agent.InvokeAsync(chat))
//{
//    chat.Add(message2);
//}
//Console.WriteLine($"AI Response: {(string.Join("\r\n", chat.Select(a => a.Content)))}");


#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0110
ChatCompletionAgent agent =
    new()
    {
        Name = "QA-Agent",
        Instructions = "Ask me anything!",
        Kernel = kernel,
        HistoryReducer = new ChatHistoryTruncationReducer(3)
    };

ChatHistory chat = [];

// Add a user request to the chat history

while (true)
{
    Console.WriteLine("请输入您的问题：");
    var userRequest = Console.ReadLine();
    ChatMessageContent message = new(AuthorRole.User, userRequest);
    if (userRequest.Equals("bye", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Exiting the chat.");
        break;
    }

    ChatMessageContent userMessage = new(AuthorRole.User, userRequest);
    chat.Add(userMessage);
    Console.WriteLine("User: " + userRequest);

    await agent.ReduceAsync(chat);

    await foreach (ChatMessageContent message2 in agent.InvokeAsync(chat))
    {
        chat.Add(message);
        Console.WriteLine("Agent: " + message2.Content);
    }
}

Console.WriteLine("Hello, World!");
