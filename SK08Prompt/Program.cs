// See https://aka.ms/new-console-template for more information

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;


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



var userRequest = "帮我订一张从北京到上海的票，明天出发。";

//var originPrompt = $"""
//请帮我识别用户意图:
//用户请求是：{userRequest}。
//意图：
//""";

//await Chat(originPrompt);

//var optimizedPrompt = $"""
//请帮我识别用户意图，仅返回意图，不要解释。
//用户请求是：{userRequest}。
//意图：
//""";

//await Chat(optimizedPrompt);


//var optimizedPrompt = $"""
//用户会输入与票务相关的文本信息，你的任务是从输入中识别以下三种意图之一：

//* 订票意图：用户希望购买或预订车票；
//* 退票意图：用户希望退掉已购买的车票；
//* 咨询意图：用户只是询问车票相关信息，不涉及购买或退票。

//用户请求是：{userRequest}。
//意图：
//""";

//await Chat(optimizedPrompt);


//var optimizedPrompt = $@"
//用户会输入与票务相关的文本信息，你的任务是从输入中识别以下三种意图之一：

//* 订票意图：用户希望购买或预订车票；
//* 退票意图：用户希望退掉已购买的车票；
//* 咨询意图：用户只是询问车票相关信息，不涉及购买或退票。

//请你以如下JSON格式返回识别结果：
//{{
//    ""intention"": ""<订票意图/退票意图/咨询意图>"",
//    ""reason"": ""<简要说明识别此意图的原因>""
//}}

//用户请求是：{userRequest}。
//意图：
//";


//await Chat(optimizedPrompt);



//var optimizedPrompt = $@"
//用户会输入与票务相关的文本信息，你的任务是从输入中识别以下三种意图之一：

//* 订票意图：用户希望购买或预订车票；
//* 退票意图：用户希望退掉已购买的车票；
//* 咨询意图：用户只是询问车票相关信息，不涉及购买或退票。

//请你以如下JSON格式返回识别结果：
//{{
//    ""intention"": ""<订票意图/退票意图/咨询意图>"",
//    ""reason"": ""<简要说明识别此意图的原因>""
//}}

//<example>
//用户请求：我的票能退吗？
//意图：
//{{
//    ""intention"": ""退票意图"",
//    ""reason"": ""询问是否可以退票""
//}}
//</example>


//用户请求是：{userRequest}
//意图：
//";

//await Chat(optimizedPrompt);


async Task ChatWithHistory(ChatHistory chatHistory)
{
    var response = await chatCompletionService.GetChatMessageContentAsync(chatHistory, executionSettings);
    Console.WriteLine($"AI Response: {response.Content}");
    chatHistory.AddAssistantMessage(response.Content);
}

//var optimizedPrompt = $@"
//你是一名铁路票务意图识别专家，负责在用户界面与后端服务之间准确判断用户意图。

//* 订票意图：用户希望购买或预订车票；
//* 退票意图：用户希望退掉已购买的车票；
//* 咨询意图：用户只是询问车票相关信息，不涉及购买或退票。

//请你以如下JSON格式返回识别结果：
//{{
//    ""intention"": ""<订票意图/退票意图/咨询意图>"",
//    ""reason"": ""<简要说明识别此意图的原因>""
//}}

//<example>
//用户请求：我的票能退吗？
//意图：
//{{
//    ""intention"": ""退票意图"",
//    ""reason"": ""询问是否可以退票""
//}}
//<example>

//";

//var chatHistory = new ChatHistory();
//chatHistory.AddSystemMessage(optimizedPrompt);
//chatHistory.AddUserMessage(userRequest);

//await ChatWithHistory(chatHistory);




//var optimizedPrompt = $@"
//你是一名铁路票务意图识别专家，负责在用户界面与后端服务之间准确判断用户意图。

//* 订票意图：用户希望购买或预订车票；
//* 退票意图：用户希望退掉已购买的车票；
//* 咨询意图：用户只是询问车票相关信息，不涉及购买或退票。

//仅从以上三种意图中选择，不要新增意图；
//若无法明确判断，则默认“咨询意图”，理由写“输入不够明确，默认为咨询”。

//请你以如下JSON格式返回识别结果：
//{{
//    ""intention"": ""<订票意图/退票意图/咨询意图>"",
//    ""reason"": ""<简要说明识别此意图的原因>""
//}}

//<example>
//用户请求：我的票能退吗？
//意图：
//{{
//    ""intention"": ""退票意图"",
//    ""reason"": ""询问是否可以退票""
//}}
//<example>

//";

//userRequest = "今天天气不错，适合去哪旅游呢？";

//var chatHistory = new ChatHistory();
//chatHistory.AddSystemMessage(optimizedPrompt);
//chatHistory.AddUserMessage(userRequest);

//await ChatWithHistory(chatHistory);



var optimizedPrompt = $@"
你是一名铁路票务意图识别专家，负责在用户界面与后端服务之间准确判断用户意图。

* 订票意图：用户希望购买或预订车票；
* 退票意图：用户希望退掉已购买的车票；
* 咨询意图：用户只是询问车票相关信息，不涉及购买或退票。

仅从以上三种意图中选择，不要新增意图；
若无法明确判断，则默认“咨询意图”，理由写“输入不够明确，默认为咨询”。

请你以如下JSON格式返回识别结果：
{{
    ""intention"": ""<订票意图/退票意图/咨询意图>"",
    ""reason"": ""<简要说明识别此意图的原因>""
}}

当接收到输入时，请按以下思考流程：

1. 关键词扫描：首先查找“订票”相关词（如“订”、“买”）；
2. 退票检测：若无订票，再查“退票”或“取消”关键词；
3. 咨询判断：若前两者都无，则检测是否为问询（如“多少钱”、“几点”）；
4. 置信度评估：计算模型信心，若 < 0.7，按“咨询意图”处理并标注低置信度；
5. 格式输出：严格按 JSON 模板返回结果。

<example>
用户请求：我的票能退吗？
意图：
{{
    ""intention"": ""退票意图"",
    ""reason"": ""询问是否可以退票""
}}
<example>

";

//userRequest = "今天天气不错，适合去哪旅游呢？";

//var chatHistory = new ChatHistory();
//chatHistory.AddSystemMessage(optimizedPrompt);
//chatHistory.AddUserMessage(userRequest);

//await ChatWithHistory(chatHistory);


var userPrompt = optimizedPrompt;
var prompt = $"""
你是一名提示工程师，专注于提升提示词的清晰度、具体性和执行效果。请按照以下要求对我提供的原始提示进行优化：

1. **输出格式**  
   - **优化后提示**：给出改写并增强后的版本  
   - **改进说明**：简要说明优化了哪些方面（如更具体、结构化、避免歧义等）  
   - **进一步建议**：列出 2–3 条可选的改进思路或注意点  

2. **优化目标**  
   - 用词精准，避免模糊或冗长  
   - 明确受众与任务边界  
   - 结构清晰，易于模型解析  

---

原始提示：  {userPrompt}

""";

var response = await kernel.InvokePromptAsync<string>(prompt);

Console.WriteLine($"AI Response: {response}");

Console.WriteLine("Hello, World!");
