using Microsoft.SemanticKernel;
using OpenAI;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK05MultipleAi
{
    public static class KernelExtensions
    {
        public static IKernelBuilder AddChatCompletionService(this IKernelBuilder builder, string aiProviderCode = null)
        {
            var aiOptions = AiSettings.LoadAiProvidersFromFile();
            if (string.IsNullOrWhiteSpace(aiProviderCode))
            {
                aiProviderCode = aiOptions.DefaultProvider;
            }
            var aiProvider = aiOptions.Providers.FirstOrDefault(x => x.Code == aiProviderCode);
            if (aiProvider == null)
            {
                throw new ArgumentException($"未找到编码为 {aiProviderCode} 的 AI 服务提供商");
            }
            var modelId = aiProvider.GetChatCompletionApiService()?.ModelId;
            if (string.IsNullOrWhiteSpace(modelId))
            {
                throw new ArgumentException($"未找到名称为 chat-completions 的 API 服务");
            }

            if (aiProvider.AiType == AiProviderType.OpenAI)
            {
                builder.AddOpenAIChatCompletion(
                    modelId: modelId,
                    apiKey: aiProvider.ApiKey);
            }

            if (aiProvider.AiType == AiProviderType.AzureOpenAI)
            {
                builder.AddAzureOpenAIChatCompletion(
                    deploymentName: modelId,
                    endpoint: aiProvider.ApiEndpoint,
                    apiKey: aiProvider.ApiKey);
            }

            if (aiProvider.AiType == AiProviderType.OpenAI_Compatible)
            {
                OpenAIClientOptions clientOptions = new OpenAIClientOptions
                {
                    Endpoint = new Uri(aiProvider.ApiEndpoint)
                };

                OpenAIClient client = new(new ApiKeyCredential(aiProvider.ApiKey), clientOptions);

                builder.AddOpenAIChatCompletion(modelId: modelId, openAIClient: client);
            }

            return builder;
        }
    }
}
