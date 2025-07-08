using Microsoft.SemanticKernel.ChatCompletion;

var kernel = ServiceExtensions.GetKernel("azure-openai");

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

var data = new VectorStoreRecordData("some metadata", embedding: new float[] { 0.1f, 0.2f, 0.3f });

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
