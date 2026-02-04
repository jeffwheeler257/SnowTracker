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
            try
            {
                // .exe path
                string envPath = Path.Combine(AppContext.BaseDirectory, ".env");
                if (!File.Exists(envPath))
                {
                    // Dev path
                    envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
                }
                DotEnv.Load(new DotEnvOptions(envFilePaths: [envPath]));
                string senderEmail = Environment.GetEnvironmentVariable("SENDER_EMAIL") 
                    ?? throw new ArgumentException("No sender email given.");
                string senderEmailPassword = Environment.GetEnvironmentVariable("SENDER_PASSWORD") 
                    ?? throw new ArgumentException("No sender app password given.");

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderEmailPassword);
                smtpClient.EnableSsl = true;

                MailMessage email = new MailMessage();
                email.From = new MailAddress(senderEmail);
                email.Subject = "Daily Snow Reports";
                email.Body = GenerateEmailContent(resortInfoList);
                email.IsBodyHtml = true;

                string distributionList = Environment.GetEnvironmentVariable("MAIL_DISTRIBUTION_LIST") 
                    ?? throw new ArgumentException("No email distribution list");
                
                string[] recipients = distributionList.Split(
                    ',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
                );

                foreach (string recipient in recipients)
                {   
                    email.Bcc.Add(recipient);
                }

                smtpClient.Send(email);
                Logger.Log("Mail sent succesfully.");
            } catch (Exception e)
            {
                Logger.Log("Issue sending email " + e.Message);
                throw;
            }

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
                            sb.Append($"""<td style="text-align: center;">{forecast}</td>""");
                        }
                        
                    }else
                    {
                        sb.Append($"""<td style="text-align: center;">{value}</td>""");
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