using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

public class Program
{
    static async Task Main(string[] args)
    {
        // 初始化依赖注入容器
        var services = new ServiceCollection();

        // 注册Kernel服务
        services.RegisterKernels();

        // 构建ServiceProvider
        var serviceProvider = services.BuildServiceProvider();


        // 获取Kernel服务
        var kernel = serviceProvider.GetKeyedService<Kernel>("zhipu");
        var response = await kernel.InvokePromptAsync("一句话简单介绍Semantic Kernel。");
        Console.WriteLine(response.GetValue<string>());

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
        var chatCompletionResponse = await chatCompletionService.GetChatMessageContentAsync("一句话简单介绍LangChain。");
        Console.WriteLine(chatCompletionResponse.Content);
    }

}

