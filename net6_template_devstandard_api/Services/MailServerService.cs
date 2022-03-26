using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace net6_template_devstandard_api.Services
{
    public interface IMailServerService
    {
        Task<bool> SendMail(string msg);
    }
    public class MailServerService : IMailServerService
    {
        private readonly IConfiguration configuration;
        public MailServerService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<bool> SendMail(string msg)
        {
            try
            {
                var SmtpEmail_From = configuration.GetSection("MailServerConfiguration:MailFrom").Value;
                var SmtpEmail_To = configuration.GetSection("MailServerConfiguration:MailTo").Value.Split(',').ToList();
                var mailServer = Boolean.Parse(configuration.GetSection("MailServerConfiguration:IsStatus").Value);
                var SmtpClient = configuration.GetSection("MailServerConfiguration:Host").Value;
                var SmtpServerPort = configuration.GetSection("MailServerConfiguration:Port").Value;
                var Email_Subject = "Test mail alert";

                var content = msg;

                var myMail = new MailMessage();
                myMail.From = new MailAddress(SmtpEmail_From.ToString());
                myMail.Subject = Email_Subject;
                foreach (var rec in SmtpEmail_To)
                {
                    myMail.To.Add(rec);
                }
                myMail.IsBodyHtml = true;
                myMail.BodyEncoding = System.Text.Encoding.UTF8;
                myMail.Body = content;

                var smtpClient = new SmtpClient();
                smtpClient.Port = Convert.ToInt16(SmtpServerPort);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Host = SmtpClient;

                if (!mailServer)
                {
                    var mailTempPath = configuration.GetSection("MailServerConfiguration:MailTemp").Value;
                    if (!Directory.Exists(mailTempPath))
                    {
                        Directory.CreateDirectory(mailTempPath);
                    }
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = mailTempPath;
                    await smtpClient.SendMailAsync(myMail);
                    smtpClient.Dispose();
                    myMail.Dispose();
                }
                else
                {
                        await smtpClient.SendMailAsync(myMail);
                        smtpClient.Dispose();
                        myMail.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }
    }
}
