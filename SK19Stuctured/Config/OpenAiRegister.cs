using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
public class OpenAiRegister : AiProviderRegister
{
    public override AiProviderType AiProviderType => AiProviderType.OpenAI;

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

        builder.AddOpenAIChatCompletion(
            modelId: chatModelId,
            apiKey: aiProvider.ApiKey,
            endpoint: new Uri(aiProvider.ApiEndpoint)
            , httpClient: httpClient);
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

        builder.AddOpenAITextEmbeddingGeneration(embeddingModelId, aiProvider.ApiKey);
    }
}