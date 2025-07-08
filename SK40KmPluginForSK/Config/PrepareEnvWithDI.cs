//#r "nuget:Microsoft.Extensions.Configuration"
//#r "nuget:Microsoft.Extensions.Configuration.Json"
//#r "nuget:Microsoft.Extensions.Configuration.Binder"
//#r "nuget:Microsoft.SemanticKernel"
//#r "nuget:Microsoft.SemanticKernel.Yaml"
//#r "nuget:Microsoft.SemanticKernel.Connectors.OpenAI"
//#r "nuget:Microsoft.SemanticKernel.Connectors.AzureOpenAI"
//#r "nuget:NLog.Extensions.Logging"

//#!import Config/AiProvider.cs
//#!import Config/AiOptions.cs
//#!import Config/AiSettings.cs
//#!import Config/OpenAiHttpClientHandler.cs
//#!import Config/AiProviderRegister.cs
//#!import Config/AzureOpenAiRegister.cs
//#!import Config/OpenAiRegister.cs
//#!import Config/OpenAiCompatibleAiRegister.cs
//#!import Config/AiProviderRegisterFactory.cs
//#!import Config/NLogServiceCollectionExtensions.cs
//#!import Config/KernelServiceCollectionExtensions.cs

#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0010


using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.Extensions.DependencyInjection;


public class ParepareEnvWithDI
{
    public static Kernel GetKernel(string aiProviderCode)
    {
        var provider = BuildServiceProvider();
        var kernel = provider.GetKeyedService<Kernel>(aiProviderCode);

        Console.WriteLine($"[{aiProviderCode}] AI Kernel  is ready!");

        return kernel;
    }

    public static IServiceProvider BuildServiceProvider()
    {
        // 初始化依赖注入容器
        var services = new ServiceCollection();
        services.AddNLogging();
        services.RegisterKernels();
        return services.BuildServiceProvider();
    }
}