using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OpenQA.Selenium.DevTools.V142.Network;

namespace SnowTracker
{
    public class SkiResortInfo
    {
        
        public static string GetNewSnowfall(string resort)
        {
            HtmlDocument htmlDoc = LoadHtml(resort);
            var newSnowfallElement = htmlDoc.DocumentNode.SelectSingleNode(
                "//div[contains(@class,'about-weather-summary__snow-information-value')]/span[not(@class)]"
            );

            if (newSnowfallElement == null)
            {                
                return "Snowfall not found.";
            }

            string newSnowfall = newSnowfallElement.InnerText.Trim();
            return newSnowfall;
        }
        public static int[] getSnowForecast(string resort)
        {
            int[] dailySnowForecast = new int[6];
            HtmlDocument htmlDoc = LoadHtml(resort);
            var snowfallRow = htmlDoc.DocumentNode.SelectNodes(
                "//tr[@class='forecast-table-row' and @data-row='snow']/td"
            );

            if (snowfallRow == null)
            {                
                return dailySnowForecast;
            }

            int[] incrementalSnowfall = new int[18];
            int index = 0;

            // ISSUES RETRIEVING ACTUAL SNOW DATA IN HERE SOMEWHERE
            foreach (var td in snowfallRow)
            {
                int value = 0;
                var spanNodes = td.SelectNodes(".//span");
                if (spanNodes != null && spanNodes.Count > 0)
                {
                    var firstSpanText = spanNodes[0].InnerText.Trim();

                    if (!string.IsNullOrEmpty(firstSpanText) && firstSpanText != "—")
                    {
                        if (!int.TryParse(firstSpanText, out value))
                            value = 0;
                    }
                }
                index++;
            }

            for(int i = 0; i < 6; i++)
            {
                dailySnowForecast[i] = incrementalSnowfall[3 * i] + incrementalSnowfall[3 * i + 1] + incrementalSnowfall[3 * i + 2];
            }

            return dailySnowForecast;
        }


        /*
        Methods to add:
        Top & bottom snow depth
        Temperature
        */

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