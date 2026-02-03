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
        public static int[] GetSnowForecast(string resort)
        {
            int[] dailySnowForecast = new int[6];
            HtmlDocument htmlDoc = LoadHtml(resort);
            HtmlNodeCollection snowfallRow = htmlDoc.DocumentNode.SelectNodes(
                // "//tr[@class='forecast-table-row' and @data-row='snow']/td"
                "//tr[@data-row='snow']/td"
            );
            
            if (snowfallRow == null)
            {          
                Console.WriteLine("Empty Node Collection");      
                return dailySnowForecast;
            }

            int[] incrementalSnowfall = new int[18];
            int index = 0;
            
            foreach (HtmlNode td in snowfallRow)
            {
                int value = 0;
                HtmlNodeCollection spanNodes = td.SelectNodes(".//span");
                if (spanNodes.Count == 2)
                {
                    string firstSpanText = spanNodes[0].InnerText.Trim();
                    value = int.Parse(firstSpanText);
                }
                incrementalSnowfall[index] = value;
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