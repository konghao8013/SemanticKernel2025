using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK18FunctionCallingReturn.Plugins
{
    /// <summary>
    /// A plugin that provides the current weather data and provides descriptions for the return type properties.
    /// </summary>
    public sealed class WeatherPlugin3
    {
        [KernelFunction]
        public WeatherData GetWeatherData()
        {
            return new WeatherData()
            {
                Data1 = 35.0,  // Temperature in degrees Celsius  
                Data2 = 20.0,  // Humidity in percentage  
                Data3 = 10.0,  // Dew point in degrees Celsius  
                Data4 = 15.0   // Wind speed in kilometers per hour
            };
        }

        public sealed class WeatherData
        {
            [Description("Temp (°C)")]
            public double Data1 { get; set; }

            [Description("Humidity (%)")]
            public double Data2 { get; set; }

            [Description("Dew point (°C)")]
            public double Data3 { get; set; }

            [Description("Wind speed (km/h)")]
            public double Data4 { get; set; }
        }
    }
}
