using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using NLog.Extensions.Logging;
using NLog.Targets;
using OpenAI.Files;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class ServiceExtensions
{
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
    /// <summary>
    /// 添加日志输出
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
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

    public static Kernel GetKernel(string aiProviderCode)
    {
        var provider = BuildServiceProvider();
        var kernel = provider.GetKeyedService<Kernel>(aiProviderCode);

        Console.WriteLine($"[{aiProviderCode}] AI Kernel  is ready!");

        return kernel;
    }

    private static IServiceProvider BuildServiceProvider()
    {
        // 初始化依赖注入容器
        var services = new ServiceCollection();
        services.AddNLogging();
        //注册 AI服务提供商
        services.RegisterKernels();
        return services.BuildServiceProvider();
    }



    public static async Task DownloadFileContentAsync(OpenAIFileClient fileClient, string fileId, bool launchViewer = false)
    {
        OpenAIFile fileInfo = fileClient.GetFile(fileId);
        if (fileInfo.Purpose == FilePurpose.AssistantsOutput)
        {
            string filePath = Path.Combine(Path.GetTempPath(), Path.GetFileName(fileInfo.Filename));
            if (launchViewer)
            {
                filePath = Path.ChangeExtension(filePath, ".png");
            }
            BinaryData content = await fileClient.DownloadFileAsync(fileId);
            File.WriteAllBytes(filePath, content.ToArray());
            Console.WriteLine($"  File #{fileId} saved to: {filePath}");
            if (launchViewer)
            {
                Process.Start(
                    new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/C start {filePath}"
                    });
            }
        }
    }
}

