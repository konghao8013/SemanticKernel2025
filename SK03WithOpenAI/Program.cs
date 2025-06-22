//public class Program
//{
//    public static void Main(string[] args)
//    {
//        Console.WriteLine("Hello, World!");
//        // Add your code here to interact with OpenAI or other services. 
//        // For example, you can create a kernel, invoke prompts, etc. 
//        // See the SK02WithZhipu example for reference. 
//    }
//}


using Microsoft.SemanticKernel;

public class Program
{
    public async static Task Main(string[] args)
    {
        await TestStreamingChatMessage();
        Console.WriteLine("Hello, World!");
        // Add your code here to interact with OpenAI or other services. 
        // For example, you can create a kernel, invoke prompts, etc. 
        // See the SK02WithZhipu example for reference. 
    }


    static Kernel GetKernel()
    {
        var deepseekApiKey = File.ReadAllText("D:\\ai-apis\\deepseekey.txt");
        // zhipuApiKey = "ddddc01549175d4c18e65a70e0d8329d.jAFiuW3FfK5PbxuE";

        // Create kernel builder
        var builder = Kernel.CreateBuilder();

        var endpoint = new Uri("https://api.deepseek.com");
        // Add OpenAI Chat completion
        builder.AddOpenAIChatCompletion(
            modelId: "deepseek-chat", // 可选模型编码：glm-4-plus、glm-4-0520、glm-4 、glm-4-air、glm-4-airx、glm-4-long、 glm-4-flash(免费)
            apiKey: deepseekApiKey,
            endpoint: endpoint);
        // Build kernel
        var kernel = builder.Build();
        return kernel;
    }
    static async Task TestStreamingChatMessage()
    {

        var kernel = GetKernel();

        var response = await kernel.InvokePromptAsync("介绍下Deepseek的产品和服务");
        Console.WriteLine(response.GetValue<String>());
        Console.ReadLine();
    }
}