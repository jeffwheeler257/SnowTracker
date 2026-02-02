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
                "Sunshine"
            };
            foreach(string resort in skiResorts)
            {
                Console.WriteLine(resort);
                Console.WriteLine("Snowfall: " + SkiResortInfo.GetNewSnowfall(resort));
            }
        }
    }
}