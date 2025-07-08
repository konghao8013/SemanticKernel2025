// See https://aka.ms/new-console-template for more information

using dotenv.net;
using Microsoft.Identity.Client;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;

var env = DotEnv.Fluent()
    .WithEnvFiles("Config/.env")
    .WithExceptions()
    .WithTrimValues()
    .WithDefaultEncoding()
    .Read();

var build = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(
        deploymentName: env["AZURE_CHAT_DEPLOYMENT"],
        endpoint: env["AZURE_OPENAI_ENDPOINT"],
        apiKey: env["AZURE_OPENAI_API_KEY"]
    );
build.Services.AddNLogging();
var kernel = build
    .Build();

var memoryPlugin = new MemoryPlugin(env["MEMORY_ENDPOINT"], env["MEMORY_API_KEY"], true);
kernel.ImportPluginFromObject(memoryPlugin, "memory");



var text = """
Kernel Memory leverages vector storage to save the meaning of the documents ingested into the service, solutions like Azure AI Search, Qdrant, Elastic Search, Redis etc.

Typically, storage solutions offer a maximum capacity for each collection, and often one needs to clearly separate data over distinct collections for security, privacy or other important reasons.

In KM terms, these collection are called “indexes”.

When storing information, when searching, and when asking questions, KM is always working within the boundaries of one index. Data in one index never leaks into other indexes.
""";



await memoryPlugin.SaveAsync(text);


//await memoryPlugin.SaveAsync("[{name:'a1',value:'试验日期',readonly:'true'},{name:'b1',value:'',readonly:'false'},{name:'a2',value:'试验环境',readonly:'true'},{name:'b2',value:'',readonly:'false'}]","");
//await memoryPlugin.SaveAsync("我的名字叫孔浩");
//await memoryPlugin.SaveAsync("工程划分编号 KH998-008-009");

await memoryPlugin.SaveFileAsync("Config/KM_Architecture.pdf", "km_architecture");



var skPrompt = """
Question to Memory: {{$input}}

Answer from Memory: {{memory.ask $input}}

If the answer is empty say 'I don't know' otherwise reply with a preview of the answer,
truncated to 15 words. Prefix with one emoji relevant to the content.
""";
//var skPrompt = "" +
//    "1.根据我输入的内容{{$input}}，查找表单中可以录入的单元格坐标" +
//    "2.返回Jons格式[{title:'单元格名称',value:'单元格填入的值',name:'填入单元格的坐标'}]" +
//    "";

var myFunction = kernel.CreateFunctionFromPrompt(skPrompt);


Console.WriteLine("Semantic Function ready.");

var answer = await myFunction.InvokeAsync(kernel, "What's Kernel Memory?");
//var answer = await myFunction.InvokeAsync(kernel, "试验环境应该填写到表单哪个单元格位置？");
//answer.Display();

// 尝试从 Metadata 中获取 usage 信息
if (answer.Metadata.TryGetValue("usage", out var usageObj) &&
    usageObj is not null)
{
    // 通常 usageObj 是一个 Dictionary<string, object>，可以进一步解析
    if (usageObj is IReadOnlyDictionary<string, object> usageDict)
    {
        if (usageDict.TryGetValue("prompt_tokens", out var promptTokens))
            Console.WriteLine($"Prompt tokens: {promptTokens}");

        if (usageDict.TryGetValue("completion_tokens", out var completionTokens))
            Console.WriteLine($"Completion tokens: {completionTokens}");

        if (usageDict.TryGetValue("total_tokens", out var totalTokens))
            Console.WriteLine($"Total tokens: {totalTokens}");
    }
    else
    {
        // 有时 usageObj 直接是 JsonElement，需要反序列化
        Console.WriteLine($"Usage: {usageObj}");
    }
}
else
{
    Console.WriteLine("No token usage info available.");
}

Console.WriteLine($"Answer: {answer.RenderedPrompt}");



//kernel.Display();
//Console.WriteLine(kernel.ToString());
Console.WriteLine("Hello, World!");
