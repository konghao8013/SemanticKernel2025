using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.TextGeneration;
using Microsoft.Extensions.DependencyInjection;





public class Program
{

    static async Task Main(string[] args)
    {
        await English01();

        Console.ReadLine();
    }

    static async Task Test01()
    {
        var kernel = GetKernel();
        var textGenerationService = kernel.Services.GetRequiredService<ITextGenerationService>();
        var text = await textGenerationService.GetTextContentAsync("今天天气不错，");
        Console.WriteLine(text);
    }
    static async Task TestHistory()
    {
        var kernel = GetKernel();
        //  AI对话
        // Get chat completion service
        var chatCompletionService = kernel.Services.GetRequiredService<IChatCompletionService>();
        // Create chat history
        var chatHistory = new ChatHistory();
        // chatHistory.AddSystemMessage("你是一名善解人意的心理咨询师！");
        while (true)
        {
            Console.WriteLine("enter message...");
            // Get input from user
            var message = Console.ReadLine();
            if (string.IsNullOrEmpty(message))
            {
                break;
            }

            Console.WriteLine($"You:{message}");
            // Add user message to chat history 
            chatHistory.AddUserMessage(message);
            // Get response from chat completion service with history
            var response = await chatCompletionService.GetChatMessageContentAsync(chatHistory);
            Console.WriteLine($"AI:{response.Content}");
            // Add AI response to chat history
            chatHistory.AddAssistantMessage(response.Content);
            // response.Display();
        }
    }

    static async Task TestStreamingChatMessage()
    {
        var kernel = GetKernel();
        //      流失输出
        //
        //     Get chat completion service
        var chatCompletionService = kernel.Services.GetRequiredService<IChatCompletionService>();
        Console.WriteLine("enter message...");
        // Get input from user
        var message = Console.ReadLine();
        Console.WriteLine($"You: {message}");
        // Get response from chat completion service     
        var chatResult = chatCompletionService.GetStreamingChatMessageContentsAsync(message);
        string response = "";
        await foreach (var chunk in chatResult)
        {
            if (chunk.Role.HasValue) Console.Write(chunk.Role + ": ");
            response += chunk;
            await Task.Delay(100);
            Console.Write(chunk);
        }
    }
    static Kernel GetKernel()
    {
        var key = File.ReadAllText("D:\\ai-apis\apikey.txt");
        // Create kernel builder
        var builder = Kernel.CreateBuilder();
        // Add OpenAI chat completion
        // builder.AddOpenAIChatCompletion(
        //     modelId: "YOUR_MODEL_ID",
        //     apiKey: "YOUR_API_KEY");
        // Add Azure OpenAI chat completion
        builder.AddAzureOpenAIChatCompletion(
            deploymentName: "gpt-4o",
            endpoint: "https://my-openapi.openai.azure.com/",
            apiKey: key);
        // Build kernel
        var kernel = builder.Build();
        return kernel;
    }
    static async Task English01()
    {

        var kernel = GetKernel();

        var prompt = """
你是一名专业的英语口语教练，请帮助我练习英语口语。
* 由于我的词汇量不足，因此我的表达中会中英混合，当出现这种情况时，请告诉我正确的英语表达。
* 由于我的语法薄弱，当表达出现语法错误时，请及时进行语法纠正。
* 由于我的词汇量不足，当我要求解释某些句子或单词时，请通过中文向我解释。
* 由于有些单词不会发音，当我询问时，请以中文的形式告诉我发音技巧。比如对于单词`ambulance`，的中文发音是`安比卢恩斯`。
""";



        //AI对话
        //Get chat completion service
        var chatCompletionService = kernel.Services.GetRequiredService<IChatCompletionService>();
        // Create chat history
        var chatHistory = new ChatHistory();
        chatHistory.AddSystemMessage(prompt);
        while (true)
        {
            Console.WriteLine("enter message...");
            // Get input from user
            var message = Console.ReadLine();
            if (string.IsNullOrEmpty(message))
            {
                break;
            }

            Console.WriteLine($"You:{message}");
            // Add user message to chat history 
            chatHistory.AddUserMessage(message);
            // Get response from chat completion service with history
            var response = await chatCompletionService.GetChatMessageContentAsync(chatHistory);
            Console.WriteLine($"AI:{response.Content}");
            // Add AI response to chat history
            chatHistory.AddAssistantMessage(response.Content);
            // response.Display();
        }


        //kernel.Display();
    }

}