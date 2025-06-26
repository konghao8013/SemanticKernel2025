// See https://aka.ms/new-console-template for more information

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SK16FunctionFilter;
using SK16FunctionFilter;

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
                    "Beijing" => throw new Exception("Weather data not available for Beijing."),
                    _ => "31 and snowing",
                }, "GetWeatherForCity", "Gets the current weather for the specified city and specified date time."),
        ]);

kernel.FunctionInvocationFilters.Clear();
kernel.FunctionInvocationFilters.Add(new ExceptionHandleFilter());
OpenAIPromptExecutionSettings settings = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };
var response = await kernel.InvokePromptAsync("What is the likely color of the sky in Beijing today?", new(settings));

Console.WriteLine($"AI Response: {response.GetValue<string>()}");
Console.WriteLine("Hello, World!");
