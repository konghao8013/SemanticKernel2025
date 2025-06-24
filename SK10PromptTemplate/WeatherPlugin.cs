using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WeatherPlugin
{
    [KernelFunction("GetForecast")]
    [Description("获取给定位置的天气预报")]
    [return: Description("指定位置的天气预报")]
    public static string GetForecast(string location)
    {
        return $"Sunny, 23℃";
    }
}
