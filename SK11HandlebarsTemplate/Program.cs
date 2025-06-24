// See https://aka.ms/new-console-template for more information
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

using Microsoft.SemanticKernel.PromptTemplates.Handlebars;

Console.WriteLine("请输入AI服务提供商编码：");
var aiProviderCode = Console.ReadLine();

var kernel = ServiceExtensions.GetKernel(aiProviderCode);
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();


var executionSettings = new OpenAIPromptExecutionSettings()
{
    Temperature = 0.1f,
    MaxTokens = 50,
    TopP = 0.9f
};



async Task Chat(string userPrompt)
{
    var response = await chatCompletionService.GetChatMessageContentAsync(userPrompt, executionSettings);
    Console.WriteLine($"AI Response: {response.Content}");
}





var promptTemplateConfig = new PromptTemplateConfig()
{
    Template = """
           <message role="system">Instructions: What is the intent of this request?
           Do not explain the reasoning, just reply back with the intent. If you are unsure, reply with {{choices.[0]}}.
           Choices: {{choices}}.
           </message>
           {{#each fewShotExamples}}
               {{#each this}}
                   <message role="{{role}}">{{content}}</message>
               {{/each}}
           {{/each}}
           {{#each chatHistory}}
               <message role="{{role}}">{{content}}</message>
           {{/each}}
           <message role="user">{{request}}</message>
           """,
    TemplateFormat = "handlebars"
};


List<string> choices = ["Unknown", "SendEmail", "SendMessage", "CreateDocument"];
// Create few-shot examples
List<ChatHistory> fewShotExamples =
[
    [
        new ChatMessageContent(AuthorRole.User, "Can you send a very quick approval to the marketing team?"),
        new ChatMessageContent(AuthorRole.Assistant, "SendMessage")
    ],
    [
        new ChatMessageContent(AuthorRole.User, "Can you send the full update to the marketing team?"),
        new ChatMessageContent(AuthorRole.Assistant, "SendEmail")
    ]
];

ChatHistory history = [];


KernelArguments kernelArguments = new()
{
    { "choices", choices },
    { "chatHistory", history },
    { "fewShotExamples", fewShotExamples }
};




// Create the handlebars prompt template factory
var promptTemplateFactory = new HandlebarsPromptTemplateFactory();
// Create the prompt template
var promptTemplate = promptTemplateFactory.Create(promptTemplateConfig);

var request = "整理今天的会议记录并归档";
kernelArguments.Add("request", request);

// Render the prompt
var renderedPrompt = await promptTemplate.RenderAsync(kernel, kernelArguments);

Console.WriteLine($"Rendered Prompt: {renderedPrompt}");


var getIntentFunction = kernel.CreateFunctionFromPrompt(renderedPrompt);

var intent = await getIntentFunction.InvokeAsync(kernel);
// you can also use the following code to get the intent
// var intent = await kernel.InvokeAsync(getIntentFunction);
Console.WriteLine($"Intent: {intent}");
history.Add(new ChatMessageContent(AuthorRole.User, request));
history.Add(new ChatMessageContent(AuthorRole.Assistant, intent.ToString()));


//延迟渲染

//// Create the handlebars prompt template factory
//var promptTemplateFactory = new HandlebarsPromptTemplateFactory();
//// Create the semantic function from prompt template
//var getIntentFunction = kernel.CreateFunctionFromPrompt(promptTemplateConfig, promptTemplateFactory);

//var request = await PolyglotKernel.GetInputAsync("请输入：");
//// Update request in kernel arguments
//kernelArguments["request"] = request;
//// Invoke prompt
//var intent = await kernel.InvokeAsync(getIntentFunction, kernelArguments);
//intent.Display();
//// Append to history
//history.AddUserMessage(request!);
//history.AddAssistantMessage(intent.ToString());


Console.WriteLine("Hello, World!");