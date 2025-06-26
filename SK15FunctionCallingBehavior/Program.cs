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



string promptTemplateConfig = """
            template_format: semantic-kernel
            template: What is the likely color of the sky in Boston today?
            execution_settings:
              default:
                function_choice_behavior:
                  type: auto
            """;

KernelFunction promptFunction = KernelFunctionYaml.FromPromptYaml(promptTemplateConfig);

Console.WriteLine(await kernel.InvokeAsync(promptFunction));

Console.WriteLine("Hello, World!");
