// See https://aka.ms/new-console-template for more information
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Orchestration.GroupChat;
using Microsoft.SemanticKernel.Agents.Runtime.InProcess;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SK33AgentGroupChat.Plugins;

var kernel = ServiceExtensions.GetKernel("azure-openai");

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
ChatCompletionAgent writer = new ChatCompletionAgent
{
    Name = "copywriter",
    Description = "一名文案撰写人",
    Instructions = """
        你是一名拥有十年经验的文案撰写人，以简洁和冷幽默著称。
        你的目标是以专家视角精炼并决定唯一最佳的文案。
        每次回复只提供一个提案。
        你专注于当前目标，不浪费时间闲聊。
        在完善创意时会考虑他人的建议，如果创意已被采纳，则不再修改，返回采纳的内容。
        """,
    Kernel = kernel,
    Arguments = new KernelArguments(
        new OpenAIPromptExecutionSettings()
        {
            Temperature = 0.9f
        })
};

ChatCompletionAgent reviewer = new ChatCompletionAgent
{
    Name = "reviewer",
    Description = "一名编辑",
    Instructions = """
        你是一名艺术指导，对文案有独到见解，深受大卫·奥格威的影响。
        你的目标是判断给定的文案是否可以发布。
        如果可以，请回复已通过审核，并提供简要说明，说明中应包含文案和原因。
        如果不可以，请提供改进建议，但不要给出示例。
        """,
    Kernel = kernel,
};


#pragma warning disable SKEXP0110 // RoundRobinGroupChatManager 类尚处实验阶段（1.55.0-preview）


// 创建一个监视器，用于跟踪对话历史
OrchestrationMonitor monitor = new();

// 创建一个轮询的群聊管理器，最多调用5次
var groupChatManager = new HumanInTheLoopGroupChatManager
{
    MaximumInvocationCount = 5,
    InteractiveCallback = async () =>
    {
        Console.WriteLine("请输入你的意见：" + monitor.History.LastOrDefault()?.Content);
        var userInput = Console.ReadLine();// await PolyglotKernel.GetInputAsync(");

        ChatMessageContent input = new(AuthorRole.User, userInput);

        monitor.History.Add(input);

        Console.WriteLine($"\n# INPUT: {input.Content}\n");
        return input;
    }
};

// 创建一个群聊编排器
GroupChatOrchestration orchestration = new GroupChatOrchestration(
    manager: groupChatManager,
    members: [writer, reviewer])
{
    Name = "文案撰写和审查协作智能体",
    Description = "一个文案撰写和编辑的协作组，旨在生成高质量的文案。",
    ResponseCallback = monitor.ResponseCallback
};

InProcessRuntime runtime = new InProcessRuntime();
await runtime.StartAsync();
var result = await orchestration.InvokeAsync(
    input: "为一款价格实惠且驾驶有趣的新款电动SUV创建一句标语。",
    runtime: runtime);

string output = await result.GetValueAsync(TimeSpan.FromSeconds(60*60*1));

Console.WriteLine(output);

Console.WriteLine("Hello, World!");
