using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK05MultipleAi
{
    public class AiOptions
    {
        public string DefaultProvider { get; set; }
        public List<AiProvider> Providers { get; set; }

        public AiProvider? GetProvider(string providerCode) =>
            Providers.FirstOrDefault(x => x.Code == providerCode);
    }
}
