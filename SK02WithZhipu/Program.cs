
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    static async Task Main(string[] args)
    {
        await TestStreamingChatMessage();
        Console.ReadLine();
    }

    static Kernel GetKernel()
    {
        var zhipuApiKey = File.ReadAllText("D:\\ai-apis\\zhipukey.txt");
        // zhipuApiKey = "ddddc01549175d4c18e65a70e0d8329d.jAFiuW3FfK5PbxuE";

        // Create kernel builder
        var builder = Kernel.CreateBuilder();

        var zhipuEndpoint = new Uri("https://open.bigmodel.cn/api/paas/v4/");
        // Add OpenAI Chat completion
        builder.AddOpenAIChatCompletion(
            modelId: "glm-4-flash", // 可选模型编码：glm-4-plus、glm-4-0520、glm-4 、glm-4-air、glm-4-airx、glm-4-long、 glm-4-flash(免费)
            apiKey: zhipuApiKey,
            endpoint: zhipuEndpoint);
        // Build kernel
        var kernel = builder.Build();
        return kernel;
    }
    static async Task TestStreamingChatMessage()
    {
        
        var kernel = GetKernel();

        var response = await kernel.InvokePromptAsync("介绍下智谱AI的产品和服务");
        Console.WriteLine(response.GetValue<String>());
        Console.ReadLine();
    }
}