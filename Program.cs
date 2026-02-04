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
                "Fernie",
                "Revelstoke",
                "Panorama",
                "Kicking-Horse"
            };
            List<SkiResortInfo> resortList = new List<SkiResortInfo>();
            foreach(string resort in skiResorts)
            {
                SkiResortInfo resortInfo = new SkiResortInfo(resort);
                resortList.Add(resortInfo);

            }
            EmailService.SendEmail(resortList);
        }
    }
}