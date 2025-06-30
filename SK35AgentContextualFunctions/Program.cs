using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.InMemory;
using Microsoft.SemanticKernel.Functions;

var kernel = ServiceExtensions.GetKernel("azure-openai");

//kernel.Plugins.AddFromType<TextEmbeddingGenerator>();


var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();


/// <summary>
/// 返回属于不同类别的函数列表。
/// 有些类别/函数与提示相关，而其他的则无关。
/// 这样做是为了演示提供程序的上下文函数选择能力。
/// </summary>
IReadOnlyList<AIFunction> GetAvailableFunctions()
{
    List<AIFunction> reviewFunctions = [
        AIFunctionFactory.Create(() => """
        [  
            {  
            "评论者": "John D.",  
            "日期": "2023-10-01",  
            "评分": 5,  
            "评论": "产品很棒，发货很快！"  
            },  
            {  
            "评论者": "Jane S.",  
            "日期": "2023-09-28",  
            "评分": 4,  
            "评论": "质量不错，但配送有点慢。"  
            },  
            {  
            "评论者": "Mike J.",  
            "日期": "2023-09-25",  
            "评分": 3,  
            "评论": "一般，符合预期。"  
            }  
        ]
        """
        , "GetCustomerReviews"),
    ];
    List<AIFunction> sentimentFunctions = [
        AIFunctionFactory.Create((string text) => "收集到的情感大多是积极的，也有一些中立和负面观点。", "CollectSentiments"),
        AIFunctionFactory.Create((string text) => "情感趋势识别：以积极为主，正面反馈逐渐增加。", "IdentifySentimentTrend"),
    ];
    List<AIFunction> summaryFunctions = [
        AIFunctionFactory.Create((string text) => "根据输入数据生成的摘要：关键点包括市场增长和客户满意度。", "Summarize"),
        AIFunctionFactory.Create((string text) => "提取的主题：创新、高效、客户满意度。", "ExtractThemes"),
    ];
    List<AIFunction> communicationFunctions = [
        AIFunctionFactory.Create((string address, string content) => "邮件已发送。", "SendEmail", "发送电子邮件"),
        AIFunctionFactory.Create((string number, string text) => "短信已发送。", "SendSms", "发送短信"),
        AIFunctionFactory.Create(() => "user@domain.com", "MyEmail", "获取我的电子邮件地址")
    ];

    List<AIFunction> dateTimeFunctions = [
        AIFunctionFactory.Create(() => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "GetCurrentDateTime", "获取当前本地时间"),
        AIFunctionFactory.Create(() => DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), "GetCurrentUtcDateTime", "获取当前UTC时间"),
    ];
    return [.. reviewFunctions, .. sentimentFunctions, .. summaryFunctions, .. communicationFunctions, .. dateTimeFunctions];
}


#pragma warning disable SKEXP0001

#pragma warning disable SKEXP0130 // 类型仅用于评估，在将来的更新中可能会被更改或删除。取消此诊断以继续。
ChatCompletionAgent agent = new()
{
    Name = "Assistant",
    UseImmutableKernel=true,
    Instructions = "你是一位友好的助手，能够使用工具回答用户问题。",
    Kernel = kernel,
    Arguments = new(new PromptExecutionSettings
    {
        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(
            options: new FunctionChoiceBehaviorOptions { RetainArgumentTypes = true })
    })
};
#pragma warning restore SKEXP0130 // 类型仅用于评估，在将来的更新中可能会被更改或删除。取消此诊断以继续。

#pragma warning disable SKEXP0110 (AIContextProviders)
#pragma warning disable SKEXP0130 (ContextualFunctionProvider)

// 创建一个上下文函数提供程序，使用内存向量存储和嵌入生成器
var allAvailableFunctions = GetAvailableFunctions();
var embeddingGenerator = kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();

var memoryStore = new InMemoryVectorStore(
    new InMemoryVectorStoreOptions() { EmbeddingGenerator = embeddingGenerator }
    );

var contextualFunctionProvider = new ContextualFunctionProvider(
        vectorStore: memoryStore,
        vectorDimensions: 1536,
        functions: allAvailableFunctions,
        maxNumberOfFunctions: 3, // 指定最多返回3个相关函数
        collectionName: "AvailableFunctions",
        options: new ContextualFunctionProviderOptions
        {
            NumberOfRecentMessagesInContext = 1 // 仅使用上一次智能体调用的最后1条消息作为上下文
        }
    );

ChatHistoryAgentThread agentThread = new();


// 将上下文函数提供程序添加到智能体线程中
agentThread.AIContextProviders.Add(contextualFunctionProvider);

ChatHistory history = [];
var message = await agent.InvokeAsync(
    message: "现在几点？",
    thread: agentThread,
    options: new AgentInvokeOptions()
    {
        OnIntermediateMessage = (ChatMessageContent intermediateMessage) =>
        {
            history.Add(intermediateMessage);
            return Task.CompletedTask;
        }
    }).FirstAsync();

Console.WriteLine(message.Message.Content);


// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
