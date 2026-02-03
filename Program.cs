using HtmlAgilityPack;
using System;
using System.Net.Http;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SnowTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> skiResorts = new List<string>
            {
                "Lake-Louise",
                "Sunshine",
                "Fernie"
            };
            List<SkiResortInfo> resortList = new List<SkiResortInfo>();
            foreach(string resort in skiResorts)
            {
                SkiResortInfo resortInfo = new SkiResortInfo(resort);
                resortList.Add(resortInfo);
                // Console.WriteLine(resortInfo.ResortName);
                // Console.WriteLine("Snowfall: " + resortInfo.NewSnowfall);
                // Console.WriteLine("Top depth: " + resortInfo.TopSnowDepth);
                // Console.WriteLine("Bottom depth: " + resortInfo.BottomSnowDepth);
                // Console.WriteLine("Weather Overview: " + resortInfo.ForecastOverview);
                // Console.WriteLine("Forecast raw data:");
                // int[] forecasts = resortInfo.SnowForecast;
                // foreach (int forecast in forecasts)
                // {
                //     Console.WriteLine(forecast + " cm");
                // }

            }
            EmailService.SendEmail(resortList);
        }
    }
}