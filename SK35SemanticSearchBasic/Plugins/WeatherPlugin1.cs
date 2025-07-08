using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK18FunctionCallingReturn.Plugins
{
    public sealed class WeatherPlugin1
    {
        [KernelFunction]
        [Description("Returns current weather: Data1 - Temperature (°C), Data2 - Humidity (%), Data3 - Dew Point (°C), Data4 - Wind Speed (km/h)")]
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
            public double Data1 { get; set; }
            public double Data2 { get; set; }
            public double Data3 { get; set; }
            public double Data4 { get; set; }
        }
    }
}
