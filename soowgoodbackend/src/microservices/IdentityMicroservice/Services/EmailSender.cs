using IdentityMicroservice.Interfaces;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace IdentityMicroservice.Services
{
    public class EmailSender : IEmailSender
    {
        public string ReceverEmail { get; set; }
        public string BaseUrl { get; set; }
        public string Domain { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecretKey { get; set; }
        public string EmailFrom { get; set; }
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
            BaseUrl = _configuration.GetSection("AppSettings").GetSection("mailjet.baseurl").Value;
            Domain = _configuration.GetSection("AppSettings").GetSection("mailjet.domain").Value;
            ApiKey = _configuration.GetSection("AppSettings").GetSection("mailjet.apikey").Value;
            ApiSecretKey = _configuration.GetSection("AppSettings").GetSection("mailjet.apiscrtkey").Value;
            EmailFrom = _configuration.GetSection("AppSettings").GetSection("mailjet.emailsender").Value;
        }


        public bool SendEmail(string subject, string emailBody, List<string> recievers, string type)
        {
            try
            {
                SendMailJetAsync(ApiKey, ApiSecretKey, EmailFrom, subject, emailBody, recievers, type).Wait();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        static async Task SendMailJetAsync(string ApiKey, string ApiSecretKey, string EmailFrom, string subject, string emailBody, List<string> recievers, string type)
        {
            emailBody = Regex.Replace(emailBody, @"\r\n?|\n", "<br />");
            var receiver = new List<JObject>();
            foreach (var address in recievers)
            {
                receiver.Add(new JObject {
                   {"Email", address.Trim()}
                   });
            }

            MailjetClient client = new MailjetClient(ApiKey, ApiSecretKey)
            {
                Version = ApiVersion.V3_1,
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
               .Property(Send.Messages, new JArray {
                new JObject {
                 {"From", new JObject {
                  {"Email", EmailFrom},
                  }},

                 {"To", new JArray {
                  receiver
                  }},

                 {"Subject", subject},

                 {"HTMLPart", emailBody}
                 }
                });

            MailjetResponse response = await client.PostAsync(request);
        }

        static async Task SensSMSMailJetAsync()
        {
            MailjetClient client = new MailjetClient("04b18141c61c436aba1a4784eb7522ae")
            {
                Version = ApiVersion.V4,
            };

            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
                .Property(Send.FromName, "MJPilot")
                .Property(Send.To, "+8801675964080")
                .Property(Send.Messages, "Have a nice SMS flight with Mailjet!");
            MailjetResponse response = await client.PostAsync(request);
        }
    }
}
