// See https://aka.ms/new-console-template for more information

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

Console.WriteLine("请输入AI服务提供商编码：");
var aiProviderCode = Console.ReadLine();

var kernel = ServiceExtensions.GetKernel(aiProviderCode);
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();


kernel.Plugins.Clear();
kernel.ImportPluginFromFunctions("HelperFunctions",
        [
            kernel.CreateFunctionFromMethod(() => DateTime.UtcNow.ToString("R"), "GetCurrentDateTimeInUtc", "Retrieves the current date time in UTC."),
            kernel.CreateFunctionFromMethod((string cityName, string currentDateTimeInUtc) =>
                cityName switch
                {
                    "Boston" => "61 and rainy",
                    "London" => "55 and cloudy",
                    "Miami" => "80 and sunny",
                    "Paris" => "60 and rainy",
                    "Tokyo" => "50 and sunny",
                    "Sydney" => "75 and sunny",
                    "Tel Aviv" => "80 and sunny",
                    _ => "31 and snowing",
                }, "GetWeatherForCity", "Gets the current weather for the specified city and specified date time."),
        ]);

// See https://aka.ms/new-console-template for more information
//OpenAIPromptExecutionSettings settings = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };
//var response = await kernel.InvokePromptAsync("What is the likely color of the sky in Boston today?", new(settings));


//Console.WriteLine(response.GetValue<string>());



//string promptTemplateConfig = """
//            template_format: semantic-kernel
//            template: What is the likely color of the sky in Boston today?
//            execution_settings:
//              default:
//                function_choice_behavior:
//                  type: auto
//            """;

//KernelFunction promptFunction = KernelFunctionYaml.FromPromptYaml(promptTemplateConfig);

//Console.WriteLine(await kernel.InvokeAsync(promptFunction));


//被动调用

//OpenAIPromptExecutionSettings settings = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(autoInvoke: false) };

//ChatHistory chatHistory = [];
//chatHistory.AddUserMessage("What is the likely color of the sky in Boston today?");
//while (true)
//{
//    // Start or continue chat based on the chat history
//    ChatMessageContent result = await chatCompletionService.GetChatMessageContentAsync(chatHistory, settings, kernel);
//    if (result.Content is not null)
//    {
//        Console.Write(result.Content);
//        // Expected output: "The color of the sky in Boston is likely to be gray due to the rainy weather."
//        chatHistory.Add(result);
//    }
//    // Get function calls from the chat message content and quit the chat loop if no function calls are found.
//    IEnumerable<FunctionCallContent> functionCalls = FunctionCallContent.GetFunctionCalls(result);
//    if (!functionCalls.Any())
//    {
//        break;
//    }
//    // Preserving the original chat message content with function calls in the chat history.
//    chatHistory.Add(result);
//    // Iterating over the requested function calls and invoking them sequentially.
//    // The code can easily be modified to invoke functions in concurrently if needed.
//    foreach (FunctionCallContent functionCall in functionCalls)
//    {
//        try
//        {
//            // Invoking the function
//            FunctionResultContent resultContent = await functionCall.InvokeAsync(kernel);
//            // Adding the function result to the chat history
//            chatHistory.Add(resultContent.ToChatMessage());
//        }
//        catch (Exception ex)
//        {
//            // Adding function exception to the chat history.
//            chatHistory.Add(new FunctionResultContent(functionCall, ex).ToChatMessage());
//            // or
//            //chatHistory.Add(new FunctionResultContent(functionCall, "Error details that the AI model can reason about.").ToChatMessage());
//        }
//    }
//}
//chatHistory.Display();
//Console.WriteLine(chatHistory.Select(a=>a.value))


//KernelFunction getWeatherFunction = kernel.Plugins.GetFunction("HelperFunctions", "GetWeatherForCity");

//OpenAIPromptExecutionSettings settings = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Required(functions: [getWeatherFunction]) };

//Console.WriteLine(await kernel.InvokePromptAsync("Given that it is now the 9th of September 2024, 11:29 AM, what is the likely color of the sky in Boston?", new(settings)));



OpenAIPromptExecutionSettings settings = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.None() };
//只是获取函数调用信息，不会执行函数
Console.WriteLine(await kernel.InvokePromptAsync("Tell me which provided functions I would need to call to get the color of the sky in Boston for today.", new(settings)));
Console.WriteLine("Hello, World!");
