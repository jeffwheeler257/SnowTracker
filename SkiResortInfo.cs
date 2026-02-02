using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace SnowTracker
{
    public class SkiResortInfo
    {
        private List<string> skiResorts = new List<string>
        {
            "Lake-Louise",
            "Sunshine"
        };
        
        public static void GetNewSnowfall(string resort)
        {
            HtmlDocument htmlDoc = LoadHtml(resort);
            var newSnowfallElement = htmlDoc.DocumentNode.SelectSingleNode(
                "//div[contains(@class,'about-weather-summary__snow-information-value')]/span[not(@class)]"
            );

            if (newSnowfallElement == null)
            {
                Console.WriteLine("Snowfall element NOT found.");
                return;
            }

            string newSnowfall = newSnowfallElement.InnerText.Trim();
            Console.WriteLine("New Snowfall: " + newSnowfall);
        }

        private static HtmlDocument LoadHtml(string resort)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            string url = "https://www.snow-forecast.com/resorts/" + resort + "/6day/mid";
            string pageSource = WebScraper.CreatePageSource(url);
            htmlDoc.LoadHtml(pageSource);
            return htmlDoc;
        }
    }
}