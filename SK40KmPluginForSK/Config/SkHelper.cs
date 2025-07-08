using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

public static class SkHelper
{
    public static  Kernel BuildKernel()
    {
        // 引入交互式的内核命名空间，以便用户输入
        //using PolyglotKernel = Microsoft.DotNet.Interactive.Kernel;
        Console.WriteLine("请输入AI服务提供商编码：");
        var aiProviderCode = Console.ReadLine();

        //Create Kernel builder
        var builder = Kernel.CreateBuilder();

        builder.AddChatCompletionService(aiProviderCode);

        var kernel = builder.Build();

        return kernel;
    }
}