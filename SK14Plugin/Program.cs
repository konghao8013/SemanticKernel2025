
using Microsoft.SemanticKernel;
using SK14Plugin.Plugins;

var builder = Kernel.CreateBuilder();
var kernel = builder.Build();

kernel.Plugins.AddFromFunctions("time_plugin",
[
   KernelFunctionFactory.CreateFromMethod(
        method: () => DateTime.Now,
        functionName: "get_time",
        description: "Get the current time"
    ),
    KernelFunctionFactory.CreateFromMethod(
        method: (DateTime start, DateTime end) => (end - start).TotalSeconds,
        functionName: "diff_time",
        description: "Get the difference between two times in seconds"
    )
]);

var time = await kernel.InvokeAsync<DateTime>("time_plugin", "get_time", new());
Console.WriteLine($"The current time is {time}.");

kernel.Plugins.Clear();
kernel.Plugins.AddFromType<MathPlugin>();
// Test the math plugin
double answer = await kernel.InvokeAsync<double>("MathPlugin", "Sqrt", new() { { "number1", 9 } });
Console.WriteLine($"The square root of 9 is {answer}.");
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
