using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.DevTools.V142.CSS;
using OpenQA.Selenium.DevTools.V142.Storage;
using System.Net;
using System.Net.Mail;
using dotenv.net;


namespace SnowTracker
{
    public class EmailService
    {
        public static void SendEmail(List<SkiResortInfo> resortInfoList)
        {
            DotEnv.Load();
            string? senderEmail = Environment.GetEnvironmentVariable("SENDER_EMAIL");
            string? senderEmailPassword = Environment.GetEnvironmentVariable("SENDER_PASSWORD");
            
            if (senderEmail == null || senderEmailPassword == null)
            {
                Console.WriteLine("No sender email given");
                return;
            }

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderEmailPassword);
            smtpClient.EnableSsl = true;

            MailMessage email = new MailMessage();
            email.From = new MailAddress(senderEmail);
            email.Subject = "Daily Snow Reports";
            email.Body = GenerateEmailContent(resortInfoList);
            email.IsBodyHtml = true;

            string? distributionList = Environment.GetEnvironmentVariable("MAIL_DISTRIBUTION_LIST");

            if (distributionList == null)
            {
                Console.WriteLine("No email distribution list");
                return;
            }

            string[] recipients = distributionList.Split(
                ',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
            );

            foreach (string recipient in recipients)
            {
                email.To.Add(recipient);
            }

            try
            {
                smtpClient.Send(email);
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            Console.WriteLine("Mail sent succesfully.");

        }

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