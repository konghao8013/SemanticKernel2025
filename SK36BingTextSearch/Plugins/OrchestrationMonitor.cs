using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public sealed class OrchestrationMonitor
    {
        public ChatHistory History { get; } = [];
        public ValueTask ResponseCallback(ChatMessageContent response)
        {
            History.Add(response);
            return ValueTask.CompletedTask;
        }
    }

