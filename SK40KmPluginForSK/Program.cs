// See https://aka.ms/new-console-template for more information

using dotenv.net;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;

var env = DotEnv.Fluent()
    .WithEnvFiles("Config/.env")
    .WithExceptions()
    .WithTrimValues()
    .WithDefaultEncoding()
    .Read();

var kernel = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(
        deploymentName: env["AZURE_CHAT_DEPLOYMENT"],
        endpoint: env["AZURE_OPENAI_ENDPOINT"],
        apiKey: env["AZURE_OPENAI_API_KEY"]
    )
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

await memoryPlugin.SaveFileAsync("Config/KM_Architecture.pdf", "km_architecture");



var skPrompt = """
Question to Memory: {{$input}}

Answer from Memory: {{memory.ask $input}}

If the answer is empty say 'I don't know' otherwise reply with a preview of the answer,
truncated to 15 words. Prefix with one emoji relevant to the content.
""";

var myFunction = kernel.CreateFunctionFromPrompt(skPrompt);

Console.WriteLine("Semantic Function ready.");

var answer = await myFunction.InvokeAsync(kernel, "What's Kernel Memory?");
//answer.Display();
Console.WriteLine($"Answer: {answer.RenderedPrompt}");


//kernel.Display();
//Console.WriteLine(kernel.ToString());
Console.WriteLine("Hello, World!");
