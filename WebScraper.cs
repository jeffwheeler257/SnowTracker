using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SnowTracker
{
    public class WebScraper
    {
        public string CreatePageSource(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL cannot be null or empty", nameof(url));
            using var driver = InitializeDriver();
            driver.Navigate().GoToUrl(url);
            
            // Wait for browser to report that page load has completed or time out after 10 seconds.
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(pageIsLoaded => 
                ((IJavaScriptExecutor) pageIsLoaded)
                    .ExecuteScript("return document.readyState") is string state
                    && state == "complete");

            return driver.PageSource;
        }

        private ChromeDriver InitializeDriver()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments(
                "--headless=new", // no GUI
                "--window-size=1920,1080", // avoids potential layout issues
                "--disable-extensions",
                "--disable-infobars",
                "--disable-notifications",
                "--disable-popup-blocking"
                // "--no-sandbox", should be implemented if running in Docker
            );

            return new ChromeDriver(options);
        }
    }
}