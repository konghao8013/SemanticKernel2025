// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Embeddings;
using Microsoft.SemanticKernel.Plugins.Web.Bing;

var kernel = ServiceExtensions.GetKernel("azure-openai");

//kernel.Plugins.AddFromType<TextEmbeddingGenerator>();


var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
#pragma warning disable SKEXP0050


var embeddingGenerator = kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();

var embeddingResult = await embeddingGenerator.GenerateAsync("Hello, Semantic Kernel!");
//embeddingResult.Length.Display();
//embeddingResult.Display();
var embeddingValues = embeddingResult.Vector;
foreach (float value in embeddingValues.ToArray())
{
    Console.WriteLine(value);
}
Console.ReadLine();
//var bingAPIKey = "f87b5d91d3bf4e339b95e81bbd2e74ee";

//var textSearch = new BingTextSearch(apiKey: bingAPIKey);

//var searchResult = await textSearch.SearchAsync("What's Semantic Kernel?");

//await foreach (var result in searchResult.Results)
//{
//    Console.WriteLine($"{result}");
//}
//Console.ReadLine();
//Console.WriteLine("Hello, World!");
