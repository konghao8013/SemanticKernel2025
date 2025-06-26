// See https://aka.ms/new-console-template for more information


using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SK17FunctionCallingLog.Filters;

var aiProviderCode = "oneapi";

var kernel = ServiceExtensions.GetKernel(aiProviderCode);
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();


kernel.Plugins.Clear();
kernel.ImportPluginFromFunctions("HelperFunctions",
        [
            kernel.CreateFunctionFromMethod((string cityName) =>
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
                }, "GetWeatherForCity", "Gets the current weather for the specified city."),
        ]);

kernel.FunctionInvocationFilters.Clear();

var loggerFactory = kernel.GetRequiredService<ILoggerFactory>();
var filterLogger = loggerFactory.CreateLogger<FunctionCallLogFilter>();

kernel.FunctionInvocationFilters.Add(new FunctionCallLogFilter(filterLogger));
OpenAIPromptExecutionSettings settings = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };
var response = await kernel.InvokePromptAsync("What is the likely color of the sky in London today?", new(settings));

Console.WriteLine($"AI Response: {response.GetValue<string>()}");


Console.WriteLine("Hello, World!");
