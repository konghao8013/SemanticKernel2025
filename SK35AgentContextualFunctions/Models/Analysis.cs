using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK27OpenAiAssistantAgent.Models
{
    public sealed class Analysis
    {
        public IList<string> Themes { get; set; } = [];
        public IList<string> Sentiments { get; set; } = [];
        public IList<string> Entities { get; set; } = [];
    }

}
