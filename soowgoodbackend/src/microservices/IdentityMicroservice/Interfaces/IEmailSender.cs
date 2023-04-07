using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Interfaces
{
    public interface IEmailSender
    {
        bool SendEmail(string subject, string emailBody, List<string> recievers, string type);
    }
}
