using IdentityMicroservice.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMicroservice.Services
{
    public class SmsSender : ISmsSender
    {

        public void SendSms(string commaSeparatedReceivers, string text)
        {
            string result = "";
            WebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                String to = commaSeparatedReceivers; //Recipient Phone Number multiple number must be separated by comma
                String token = "1f9a87fb6d1eefd53b4b811ab93aec28"; //generate token from the control panel
                String message = System.Uri.EscapeUriString(text); //do not use single quotation (') in the message to avoid forbidden result
                String url = "http://api.greenweb.com.bd/api.php?token=" + token + "&to=" + to + "&message=" + message;
                request = WebRequest.Create(url);
                response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                Encoding ec = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader reader = new
                System.IO.StreamReader(stream, ec);
                result = reader.ReadToEnd();
                Console.WriteLine(result);
                reader.Close();
                stream.Close();
            }
            catch (Exception ex)
            {

            }
        }
    }
}