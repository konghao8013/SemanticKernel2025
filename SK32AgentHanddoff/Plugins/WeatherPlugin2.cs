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
    /// A plugin that provides the current weather data and specifies the return type schema in the function <see cref="DescriptionAttribute"/>.
    /// </summary>
    public sealed class WeatherPlugin2
    {
        [KernelFunction]
        [Description("""Returns current weather: {"type":"object","properties":{"Data1":{"description":"Temperature (°C)","type":"number"},"Data2":{"description":"Humidity(%)","type":"number"}, "Data3":{"description":"Dew point (°C)","type":"number"},"Data4":{"description":"Wind speed (km/h)","type":"number"}}}""")]
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
