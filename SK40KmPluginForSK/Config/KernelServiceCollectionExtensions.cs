using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using NLog.Extensions.Logging;
using NLog.Targets;
using OpenAI;
using System.ClientModel;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0010


public static class KernelExtensions
{
    public static IKernelBuilder AddChatCompletionService(this IKernelBuilder builder, string aiProviderCode = null)
    {
        var aiOptions = AiSettings.LoadAiProvidersFromFile();
        if (string.IsNullOrWhiteSpace(aiProviderCode))
        {
            aiProviderCode = aiOptions.DefaultProvider;
        }
        var aiProvider = aiOptions.Providers.FirstOrDefault(x => x.Code == aiProviderCode);
        if (aiProvider == null)
        {
            throw new ArgumentException($"未找到编码为 {aiProviderCode} 的 AI 服务提供商");
        }
        var modelId = aiProvider.GetChatCompletionApiService()?.ModelId;
        if (string.IsNullOrWhiteSpace(modelId))
        {
            throw new ArgumentException($"未找到名称为 chat-completions 的 API 服务");
        }

        if (aiProvider.AiType == AiProviderType.OpenAI)
        {
            builder.AddOpenAIChatCompletion(
                modelId: modelId,
                apiKey: aiProvider.ApiKey);
        }

        if (aiProvider.AiType == AiProviderType.AzureOpenAI)
        {
            builder.AddAzureOpenAIChatCompletion(
                deploymentName: modelId,
                endpoint: aiProvider.ApiEndpoint,
                apiKey: aiProvider.ApiKey);
        }

        if (aiProvider.AiType == AiProviderType.OpenAI_Compatible)
        {
            OpenAIClientOptions clientOptions = new OpenAIClientOptions
            {
                Endpoint = new Uri(aiProvider.ApiEndpoint)
            };

            OpenAIClient client = new(new ApiKeyCredential(aiProvider.ApiKey), clientOptions);

            builder.AddOpenAIChatCompletion(modelId: modelId, openAIClient: client);
        }

        return builder;
    }

    public static IServiceCollection RegisterKernels(this IServiceCollection services)
    {
        // 从配置文件中加载AI配置
        var aiOptions = AiSettings.LoadAiProvidersFromFile();
        // 注册其他AI服务提供商
        foreach (var aiProvider in aiOptions.Providers)
        {
            var providerRegister = AiProviderRegisterFactory.Create(aiProvider!.AiType);

            providerRegister.Register(services, aiProvider);
        }
        return services;
    }

    public static IServiceCollection AddNLogging(this IServiceCollection services)
    {
        // 定义文件日志输出目标
        var fileTarget = new FileTarget()
        {
            FileName = "sk-demo.log",
            AutoFlush = true,
            DeleteOldFileOnStartup = true
        };
        // 定义控制台日志输出目标
        var consoleTarget = new ConsoleTarget();
        var config = new NLog.Config.LoggingConfiguration();
        // 定义日志输出规则(输出所有Trace级别及以上的日志到控制台)
        config.AddRule(
            NLog.LogLevel.Trace,
            NLog.LogLevel.Fatal,
            target: fileTarget,  // 这里采用文件输出
            "*");// * 表示所有Logger
                 // 注册NLog
        services.AddLogging(loggingBuilder => loggingBuilder.AddNLog(config));
        return services;
    }
}