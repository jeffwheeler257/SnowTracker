using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OpenQA.Selenium.DevTools.V142.Network;
using OpenQA.Selenium.DevTools.V143.DOM;

namespace SnowTracker
{
    public class SkiResortInfo
    {
        public string ResortName { get; set; }
        public int NewSnowfall { get; set; }
        public int TopSnowDepth  { get; set; }
        public int BottomSnowDepth  { get; set; }
        public int[] SnowForecast  { get; set; }
        public string ForecastOverview  { get; set; }

        public SkiResortInfo(string resort)
        {
            try {
                ResortName = resort;

                HtmlDocument htmlDoc = LoadHtml(resort);
                SnowForecast = GetSnowForecast(htmlDoc);

                int[] snowDepths = getSnowDepths(htmlDoc);
                TopSnowDepth = snowDepths[0];
                BottomSnowDepth = snowDepths[1];
                NewSnowfall = snowDepths[2];

                ForecastOverview = GetForecastOverview(htmlDoc);
            } catch (Exception e)
            {
                Logger.Log($"Issue creating resort info for {resort}\n" + e.Message);
                throw;
            }
        }

        public int[] GetSnowForecast(HtmlDocument htmlDoc)
        {
            int[] dailySnowForecast = new int[6];
            HtmlNodeCollection snowfallRow = htmlDoc.DocumentNode.SelectNodes(
                "//tr[@data-row='snow']/td"
                ) ?? throw new ArgumentException("Empty Snow Forecast Node Collection");;

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

        // returns a list of the snow depths. Index 0 = Top, Index 1 = Bottom, Index 2 = New
        public int[] getSnowDepths(HtmlDocument htmlDoc)
        {
            HtmlNodeCollection snowDepthNodes = htmlDoc.DocumentNode.SelectNodes(
                "//span[@class='snowht']"
                ) ?? throw new ArgumentException("Empty Snow Depth Node Collection.");
            int[] snowDepths = new int[3];
            snowDepths[0] = int.Parse(snowDepthNodes[0].InnerText.Trim());
            snowDepths[1] = int.Parse(snowDepthNodes[1].InnerText.Trim());
            if (snowDepthNodes.Count() == 3) // if there is no new snow, 3rd node class will not appear
                snowDepths[2] = int.Parse(snowDepthNodes[2].InnerText.Trim());
            return snowDepths;
        }

        public string GetForecastOverview(HtmlDocument htmlDoc)
        {
            HtmlNode overviewElement = htmlDoc.DocumentNode.SelectSingleNode(
                "//span[@class='truncated']"
                ) ?? throw new ArgumentException("Empty Overview Node.");;

            string overview = overviewElement.InnerText.Trim();
            return overview;
        }

        private static HtmlDocument LoadHtml(string resort)
        {
            try {
                HtmlDocument htmlDoc = new HtmlDocument();
                string url = "https://www.snow-forecast.com/resorts/" + resort + "/6day/mid";
                string pageSource = WebScraper.CreatePageSource(url);
                htmlDoc.LoadHtml(pageSource);
                return htmlDoc;
            } catch (Exception e)
            {
                Logger.Log("Issue loading html" + e.Message);
                throw;
            }
        }
    }
}