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
            // Use Selenum driver to fully load page content (Create class)
            var options = new ChromeOptions();
            options.AddArgument("--headless"); // run without opening browser
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");

            using var driver = new ChromeDriver(options);
            String url = "https://www.snow-forecast.com/resorts/Lake-Louise/6day/mid";
            driver.Navigate().GoToUrl(url);
            System.Threading.Thread.Sleep(5000); // wait to load JS content
            string pageSource = driver.PageSource;

            // send get request to loaded page source
            
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(pageSource);

            // get the latest snowfall
            var newSnowfallElement = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class,'about-weather-summary__snow-information-value')]/span[not(@class)]");
            
            if (newSnowfallElement == null)
            {
                Console.WriteLine("Snowfall element NOT found.");
                return;
            }
            
            string newSnowfall = newSnowfallElement.InnerText.Trim();
            Console.WriteLine("New Snowfall: " + newSnowfall);

            // get the location


            driver.Quit();
        }
    }
}