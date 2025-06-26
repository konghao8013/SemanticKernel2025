
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using OpenAI;
using System.ClientModel;
using System.ClientModel.Primitives;
using System.Diagnostics.CodeAnalysis;

public class OpenAiCompatibleAiRegister : AiProviderRegister
{
    public override AiProviderType AiProviderType => AiProviderType.OpenAI_Compatible;

    protected override void RegisterChatCompletionService(IKernelBuilder builder, IServiceProvider provider,
        AiProvider aiProvider)
    {
        var chatModelId = aiProvider.GetChatCompletionApiService()?.ModelId;
        if (string.IsNullOrWhiteSpace(chatModelId))
        {
            return;
        }

        var logger = provider.GetRequiredService<ILoggerFactory>().CreateLogger<OpenAiHttpClientHandler>();
        var httpClient = new HttpClient(new OpenAiHttpClientHandler(logger));

        OpenAIClientOptions clientOptions = new OpenAIClientOptions
        {
            Endpoint = new Uri(aiProvider.ApiEndpoint),
            Transport=new HttpClientPipelineTransport(httpClient)
        };

    

        OpenAIClient client = new(new ApiKeyCredential(aiProvider.ApiKey), clientOptions) ;
     
        builder.AddOpenAIChatCompletion(modelId: chatModelId, openAIClient: client);
    }

    [Experimental("SKEXP0010")]
    protected override void RegisterEmbeddingService(IKernelBuilder builder, IServiceProvider provider,
        AiProvider aiProvider)
    {
        var embeddingModelId = aiProvider.GetEmbeddingApiService()?.ModelId;
        if (string.IsNullOrWhiteSpace(embeddingModelId))
        {
            return;
        }

        OpenAIClientOptions clientOptions = new OpenAIClientOptions
        {
            Endpoint = new Uri(aiProvider.ApiEndpoint)
        };

        OpenAIClient client = new(new ApiKeyCredential(aiProvider.ApiKey), clientOptions);

        builder.AddOpenAITextEmbeddingGeneration(embeddingModelId, openAIClient: client);
    }
}