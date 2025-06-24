


using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using SK13PromptFilter.Filters;


//Console.WriteLine("请输入AI服务提供商编码：");
//var aiProviderCode = Console.ReadLine();

//var kernel = ServiceExtensions.GetKernel(aiProviderCode);
//var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
//kernel.PromptRenderFilters.Add(new MyPromptFilter());
//// See https://aka.ms/new-console-template for more information


static Kernel GetKernel()
{
   
    // Create kernel builder
    var builder = Kernel.CreateBuilder();
    // Add OpenAI chat completion
    // builder.AddOpenAIChatCompletion(
    //     modelId: "YOUR_MODEL_ID",
    //     apiKey: "YOUR_API_KEY");
    // Add Azure OpenAI chat completion
    builder.AddOpenAIChatCompletion(
        modelId: "deepseek-chat",
        endpoint: new Uri("http://192.168.3.126:3000/v1"),
        apiKey: "sk-T4Qx0V1APvF7D8tZ1f554e51Ad71499a93AdB3F43aE29c1f"
        );

    builder.Services.AddSingleton<IPromptRenderFilter, SensitiveInfoFilter>();
    // Build kernel
    var kernel = builder.Build();
    return kernel;
}


//var builder = Kernel.CreateBuilder();
//builder.AddOpenAIChatCompletion("模型名称", "API密钥");

var kernel = GetKernel();


var input = "你好，我是 Alice Smith (alice.smith@gmail.com)。我的信用卡是 4111-1111-1111-1111，电话是 (555) 123-4567。";

var extractInfoPrompt = "从用户请求中提取用户信息：{{$input}}";

var extractFunction = kernel.CreateFunctionFromPrompt(extractInfoPrompt);

KernelArguments kernelArguments = new()
{
    { "input", input }
};

kernel.PromptRenderFilters.Clear();

Console.WriteLine("不应用过滤器的结果：");

var result = await extractFunction.InvokeAsync(kernel, kernelArguments);

Console.WriteLine(result.ToString());



//var input = "你好，我是 Alice Smith (alice.smith@gmail.com)。我的信用卡是 4111-1111-1111-1111，电话是 (555) 123-4567。";


//var extractInfoPrompt = "从用户请求中提取用户信息：{{$input}}";

//var promptTemplateFactory = new KernelPromptTemplateFactory();

//promptTemplateFactory.TryCreate(new PromptTemplateConfig(extractInfoPrompt), out var template);

//KernelArguments kernelArguments = new()
//{
//    { "input", input }
//};

//var renderedPrompt = await template.RenderAsync(kernel, kernelArguments);

//var extractFunction = kernel.CreateFunctionFromPrompt(renderedPrompt);

//var response = await extractFunction.InvokeAsync(kernel);

//Console.WriteLine(response.ToString());

Console.WriteLine("Hello, World!");
