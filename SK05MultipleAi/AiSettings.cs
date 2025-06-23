using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK05MultipleAi
{
    public static class AiSettings
    {
        public static AiOptions LoadAiProvidersFromFile()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var aiOptions = configuration.GetSection("AI").Get<AiOptions>();
            return aiOptions;
        }
    }
}
