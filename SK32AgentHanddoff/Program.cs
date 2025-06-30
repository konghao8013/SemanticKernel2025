using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Orchestration.Handoff;
using Microsoft.SemanticKernel.Agents.Runtime.InProcess;
using Microsoft.SemanticKernel.ChatCompletion;
using SK32AgentHanddoff.Models;
using SK32AgentHanddoff.Plugins;

var kernel = ServiceExtensions.GetKernel("azure-openai");

ChatCompletionAgent CreateAgent(
    string instructions,
    string? description = null,
    string? name = null)
{
    return new ChatCompletionAgent
    {
        Name = name,
        Description = description,
        Instructions = instructions,
        Kernel = kernel.Clone()
    };
}



// 定义 agents 和工具

// 分流客服智能体：负责初步分流客户问题
ChatCompletionAgent triageAgent =
    CreateAgent(
        instructions: "一个负责分流客户问题的客服智能体",
        name: "TriageAgent",
        description: "处理客户请求");

// 订单状态查询智能体：负责处理订单状态相关请求
ChatCompletionAgent statusAgent =
    CreateAgent(
        name: "OrderStatusAgent",
        instructions: "处理订单状态请求",
        description: "一个负责查询订单状态的客服智能体");
// 添加订单状态插件
statusAgent.Kernel.Plugins.Add(KernelPluginFactory.CreateFromObject(new OrderStatusPlugin()));

// 订单退货智能体：负责处理订单退货相关请求
ChatCompletionAgent returnAgent =
    CreateAgent(
        name: "OrderReturnAgent",
        instructions: "处理订单退货请求",
        description: "一个负责处理订单退货的客服智能体");
// 添加订单退货插件
returnAgent.Kernel.Plugins.Add(KernelPluginFactory.CreateFromObject(new OrderReturnPlugin()));

// 订单退款智能体：负责处理订单退款相关请求
ChatCompletionAgent refundAgent =
    CreateAgent(
        name: "OrderRefundAgent",
        instructions: "处理订单退款请求",
        description: "一个负责处理订单退款的客服智能体");
// 添加订单退款插件
refundAgent.Kernel.Plugins.Add(KernelPluginFactory.CreateFromObject(new OrderRefundPlugin()));



#pragma warning disable SKEXP0110 

// 初始化一个监视器
OrchestrationMonitor monitor = new();

// 定义交接流程：首先由分流客服智能体处理，然后根据问题类型交接给对应的智能体
var handoffs = OrchestrationHandoffs
    .StartWith(triageAgent)
    .Add(source: triageAgent, targets: [statusAgent, returnAgent, refundAgent]) // 分流客服可交接给状态、退货、退款智能体
    .Add(source: statusAgent, target: triageAgent, description: "如非订单状态相关问题则交回分流客服")
    .Add(source: returnAgent, target: triageAgent, description: "如非退货相关问题则交回分流客服")
    .Add(source: refundAgent, target: triageAgent, description: "如非退款相关问题则交回分流客服");

var orchestration =
    new HandoffOrchestration(handoffs, members: [triageAgent, statusAgent, returnAgent, refundAgent])
    {
        Name = "CustomerSupportOrchestration",
        Description = "处理客户请求并根据问题类型交接给对应的智能体",
        ResponseCallback = monitor.ResponseCallback,
        InteractiveCallback = async () =>
        {
            var lastMessage = monitor.History.LastOrDefault();
            // 交互式回调：当需要用户输入时调用
            Console.WriteLine(lastMessage?.Content);
            var userInput = Console.ReadLine(); //await PolyglotKernel.GetInputAsync(lastMessage?.Content);

            var msg = new ChatMessageContent(AuthorRole.User, userInput);
            // 将用户输入添加到历史记录中
            monitor.History.Add(msg);
            return msg;
        }
    };



InProcessRuntime runtime = new InProcessRuntime();
await runtime.StartAsync();


var result = await orchestration.InvokeAsync(
    input: "我需要查询我的订单信息",
    runtime: runtime);


string output = await result.GetValueAsync(TimeSpan.FromSeconds(900));

Console.WriteLine("Orchestration Result:"+ output);

Console.WriteLine(String.Join("\r\n", monitor.History.Select(a => a.Content)));

//处理完成后，停止运行时以清理资源。
await runtime.RunUntilIdleAsync();

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
