using HtmlAgilityPack;
using System;
using System.Net.Http;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;

namespace SnowTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Log("Program started.");

            try{
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
            } catch (Exception e)
            {
                Logger.Log(e.Message);
                Logger.Log("Program terminated");
                return;
            }
            Logger.Log("Program executed.");
        }
    }
}