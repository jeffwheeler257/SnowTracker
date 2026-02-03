using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.DevTools.V142.CSS;
using OpenQA.Selenium.DevTools.V142.Storage;

namespace SnowTracker
{
    public class EmailService
    {
        private List<string> emailList = new List<string>
        {
            "jeffwheeler257@gmail.com"
        };

        // public void SendDailyEmail()
        // {
            
        // }

        public static string GenerateEmailContent(List<SkiResortInfo> resortInfoList)
        {
            StringBuilder sb = new StringBuilder();
            var props = typeof(SkiResortInfo).GetProperties();

            sb.Append("""
                <html>
                <body style="font-family: Arial, sans-serif;">
                <h3>Daily Snow Report</h3>
                <table border="1" cellpadding="6" cellspacing="0" style="border-collapse:collapse;">
                    <tr style="background:#f2f2f2;">
                        <th>Resort</th>
                        <th>New Snow (cm)</th>
                        <th>Top Depth (cm)</th>
                        <th>Bottom Depth (cm)</th>
                        <th>+1 day Forecast (cm)</th>
                        <th>+2 day Forecast (cm)</th>
                        <th>+3 day Forecast (cm)</th>
                        <th>+4 day Forecast (cm)</th>
                        <th>+5 day Forecast (cm)</th>
                        <th>+6 day Forecast (cm)</th>
                        <th>Weather Overview</th>
                    </tr>
            """);

            foreach(SkiResortInfo resort in resortInfoList)
            {
                sb.Append("<tr>");
                foreach(var prop in props)
                {
                    var value = prop.GetValue(resort);
                    if (value is int[] forecasts)
                    {
                        foreach (int forecast in forecasts)
                        {
                            sb.Append($"<td>{forecast}</td>");
                        }
                        
                    }else
                    {
                        sb.Append($"<td>{value}</td>");
                    }
                    
                }
                sb.Append("</tr>");
            }

            sb.Append("""
                </table>
                </body>
                </html>
            """);
            
            return sb.ToString();
        }
    }
}