// See https://aka.ms/new-console-template for more information
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using SK20PluginBasic.Models;
using System.Text.Json;

Console.WriteLine("请输入AI服务提供商编码：");
var aiProviderCode = Console.ReadLine();

var kernel = ServiceExtensions.GetKernel(aiProviderCode);
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

kernel.Plugins.Clear();
var repairePlugin = await kernel.ImportPluginFromOpenApiAsync(
    pluginName: "RepairePlugin",
    filePath: "./PluginFiles/repaire-openapi_zh-cn.json"
    );


// List repairs
var result = await repairePlugin["listRepairs"].InvokeAsync(kernel);
Console.WriteLine(result.GetValue<string>());

var repairs = JsonSerializer.Deserialize<Repair[]>(result.ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
Console.WriteLine(repairs);

Console.WriteLine($"插件导入结果: {repairePlugin.Name} - {repairePlugin.Description}");

Console.WriteLine("Hello, World!");
