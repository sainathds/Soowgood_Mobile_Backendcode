using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Interfaces
{
    public interface ISmsSender
    {
        void SendSms(string commaSeparatedReceivers, string text);
    }
}
