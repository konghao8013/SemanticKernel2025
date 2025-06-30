// See https://aka.ms/new-console-template for more information





using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.OpenAI;
using Microsoft.SemanticKernel.Agents.Orchestration.Concurrent;
using Microsoft.SemanticKernel.Agents.Orchestration.Transforms;
using Microsoft.SemanticKernel.Agents.Runtime.InProcess;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OpenAI;
using OpenAI.Assistants;
using OpenAI.Files;
using SK14Plugin.Plugins;
using SK27OpenAiAssistantAgent.Models;
using System.ClientModel;
using System.Text.Json;
using System.Text.Json.Serialization;

var kernel = ServiceExtensions.GetKernel("azure-openai");

//var aiOptions = AiSettings.LoadAiProvidersFromFile();

//var providerOpts = aiOptions.Providers.FirstOrDefault(a => a.Code == "azure-openai");
//OpenAIClient GetOpenAIClient()
//{
//    // Create a azure openai client
//    var azureOpenAIClient = OpenAIAssistantAgent.CreateAzureOpenAIClient(
//        apiKey: new ApiKeyCredential(providerOpts.ApiKey),
//        endpoint: new Uri(providerOpts.ApiEndpoint));

//    return azureOpenAIClient;
//}

//#pragma warning disable OPENAI001 
//AssistantClient GetAssistantClient()
//{
//    // Create a azure openai client
//    var azureOpenAIClient = GetOpenAIClient();
//    // Create an assistant client using the azure openai client    
//    var assistantClient = azureOpenAIClient.GetAssistantClient();
//    return assistantClient;
//}





////// 创建sk 提示词模板
//PromptTemplateConfig templateConfig = new()
//{
//    Template = "回答用户的问题，并将结果翻译为{{$language}}",
//    TemplateFormat = "semantic-kernel"
//};

////// 使用提示词模板创建 assistant
//var assistantClient = GetAssistantClient();

//var assistant = await assistantClient.CreateAssistantFromTemplateAsync(
//    modelId: "gpt-4o",
//    config: templateConfig
//);

////Console.WriteLine(assistant.Name);

////// 获取插件
//KernelPlugin mathPlugin = KernelPluginFactory.CreateFromType<MathPlugin>();

//// 创建agent并指定使用的plugins
//var assistantAgent = new OpenAIAssistantAgent(
//    definition: assistant,
//    client: assistantClient,
//    templateFormat: "semantic-kernel",
//    templateFactory: new KernelPromptTemplateFactory(),
//    plugins: [mathPlugin]
//)
//{
//    Arguments = new KernelArguments(){
//        { "language", "English" }
//    }
//};

////assistantAgent.Display();
//Console.WriteLine(assistantAgent.Name);

//// 创建会话
//var thread = await assistantClient.CreateThreadAsync();
//var threadId = thread.Value.Id;

//try
//{
//    var input = "2321+2113-123=?";
//    ChatMessageContent request = new(AuthorRole.User, input);
//    // 将用户请求添加到会话
//    await assistantAgent.AddChatMessageAsync(threadId, request);


//    // 通过agent去运行会话
//    await foreach (ChatMessageContent message in assistantAgent.InvokeAsync(threadId))
//    {
//        Console.WriteLine(message.Content);
//    }

//    // 获取指定会话的历史记录
//    var history = await assistantAgent.GetThreadMessagesAsync(threadId).ToArrayAsync();
//    history.Display();
//}
//finally
//{
//    // 清理
//    await assistantClient.DeleteThreadAsync(threadId);
//    await assistantClient.DeleteAssistantAsync(assistant.Id);
//}




//var assistantClient = GetAssistantClient();

//// Create an assistant using the assistant client
//var assistant = await assistantClient.CreateAssistantAsync(
//    modelId: "gpt-4o",
//    name: "chart-maker",
//    description: "A chart making assistant",
//    instructions: "Create charts as requested without explanation.",
//    enableCodeInterpreter: true
//);

//// Create an agent using the assistant and assistant client
//var agent = new OpenAIAssistantAgent(assistant, assistantClient);

//var thread = await assistantClient.CreateThreadAsync();

////thread.Display();

//// Add a chat message to the thread

//var request = """
//                Display this data using a bar-chart (not stacked):

//                Banding  Brown Pink Yellow  Sum
//                X00000   339   433     126  898
//                X00300    48   421     222  691
//                X12345    16   395     352  763
//                Others    23   373     156  552
//                Sum      426  1622     856 2904
//            """;

//ChatMessageContent msg = new(AuthorRole.User, request);

//var threadId = thread.Value.Id;
//agent.InvokeAsync(new[]
//{
//   msg
//});



//var azureOpenAIClient = GetOpenAIClient();
//var fileClient = azureOpenAIClient.GetOpenAIFileClient();

//// Get the response from the agent
//#pragma warning disable SKEXP0110
//await foreach (ChatMessageContent message in agent.InvokeAsync(threadId))
//{
//    Console.WriteLine(message.Content);
//    foreach (KernelContent item in message.Items)
//    {
//        if (item is FileReferenceContent fileReference)
//        {
//            await ServiceExtensions.DownloadFileContentAsync(fileClient, fileReference.FileId, launchViewer: true);
//        }
//    }
//}



//var openAIClient = GetOpenAIClient();

//// 上传文件供后续assistant 使用
//var filePath = "./Resources/sales.csv";
//var file = await openAIClient.GetOpenAIFileClient()
//                .UploadFileAsync(filePath, FileUploadPurpose.Assistants);


//// get the assistant client
//var assistantClient = openAIClient.GetAssistantClient();

//// create assistant
//var assistant = await assistantClient.CreateAssistantAsync(
//    modelId: "gpt-4o",
//    enableCodeInterpreter: true, // 启用代码解释器
//    codeInterpreterFileIds: [file.Value.Id] // 指定代码解释器使用的文件
//);

//// create Agent
//var agent = new OpenAIAssistantAgent(assistant, assistantClient);

//// create thread
//var thread = await assistantClient.CreateThreadAsync();
//var threadId = thread.Value.Id;

//try
//{
//    var input = "Which segment had the most sales?";//List the top 5 countries that generated the most profit.//Create a tab delimited file report of profit by each country per month.
//    ChatMessageContent request = new(AuthorRole.User, input);
//    //agent.InvokeAsync
//    //await agent.AddChatMessageAsync(threadId, request);

//    await foreach (ChatMessageContent message in agent.InvokeAsync(threadId))
//    {
//        message.Display();
//    }
//}
//finally
//{
//    await assistantClient.DeleteThreadAsync(threadId);
//    await assistantClient.DeleteAssistantAsync(assistant.Id);
//}

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
        Kernel = kernel
    };
}

ChatCompletionAgent topicAgent = CreateAgent(
    name: "topic-recognition-agent",
    instructions: "你是一位擅长识别文章主题的专家。请根据给定的文章，识别其主要主题。",
    description: "文章主题识别专家"
);

ChatCompletionAgent sentimentAgent = CreateAgent(
    name: "sentiment-analysis-agent",
    instructions: "你是一位情感分析专家。请根据给定的文章，判断其情感倾向。",
    description: "情感分析专家"
);

ChatCompletionAgent entityAgent = CreateAgent(
    name: "entity-recognition-agent",
    instructions: "你是一位实体识别专家。请根据给定的文章，提取其中的实体信息。",
    description: "实体识别专家"
);

#pragma warning disable SKEXP0110 
#pragma warning disable SKEXP0110 


var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
// 创建一个结构化输出转换器，用于将每个代理的响应转换为统一的分析结果
StructuredOutputTransform<Analysis> transform = new(
    service: chatCompletionService,
    executionSettings: new OpenAIPromptExecutionSettings { ResponseFormat = typeof(Analysis) });


ChatHistory history = [];

var orchestration =
    new ConcurrentOrchestration<string, Analysis>(topicAgent, sentimentAgent, entityAgent)
    {
        Name = "ArticleAnalysisOrchestration",
        Description = "依次分析文章的主题、情感和实体信息。",
        ResultTransform = transform.TransformAsync,
        // 设置每个阶段的响应回调
        ResponseCallback = (ChatMessageContent response) => {
            // 输出每个阶段的响应
            history.Add(response);
            return ValueTask.CompletedTask;
        }
    };





InProcessRuntime runtime = new InProcessRuntime();
await runtime.StartAsync();

var filePath = "./Config/hamlet_full_play_summary.txt";
var content = await File.ReadAllTextAsync(filePath);

var result = await orchestration.InvokeAsync(
    input: content,
    runtime: runtime);

var output = await result.GetValueAsync(TimeSpan.FromSeconds(60));
Console.WriteLine(JsonSerializer.Serialize(output));
//foreach (var item in output)
//{
//    Console.WriteLine(item);
//}
Console.WriteLine("Hello, World!");
