using Microsoft.SemanticKernel.Agents.Orchestration.GroupChat;
using Microsoft.SemanticKernel.ChatCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SKEXP0110 // RoundRobinGroupChatManager 类尚处实验阶段（1.55.0-preview）
#pragma warning disable SKEXP0001 // AuthorName 类尚处实验阶段（1.55.0-preview）
namespace SK33AgentGroupChat.Plugins
{
    public sealed class HumanInTheLoopGroupChatManager : RoundRobinGroupChatManager
    {
        public override ValueTask<GroupChatManagerResult<bool>> ShouldRequestUserInput(ChatHistory history, CancellationToken cancellationToken = default)
        {
            string? lastAgent = history.LastOrDefault()?.AuthorName;
            if (lastAgent is null)
            {
                return ValueTask.FromResult(new GroupChatManagerResult<bool>(false) { Reason = "No agents have spoken yet." });
            }
            if (lastAgent == "reviewer")
            {
                Console.WriteLine("需要用户输入以继续审查过程。请提供您的反馈或修改建议：");
                return ValueTask.FromResult(new GroupChatManagerResult<bool>(true) { Reason = "User input is needed after the reviewer's message." });
            }
            return ValueTask.FromResult(new GroupChatManagerResult<bool>(false) { Reason = "User input is not needed until the reviewer's message." });
        }
    }
}
