using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Web.Optimization;

namespace TICRM.Helper
{
    public static class Utility
    {
        public static void SendEmail(string strBody)
        {

            try
            {
                //MailAddress fromAddress = new MailAddress("mansoorsmtp@gmail.com", "TI CRM");
                //MailAddress toAddress = new MailAddress("aqil@techimplement.com", "aqil");
                //const string fromPassword = "@dmin1234";
                //const string subject = "A New Lead Arrived";
                //string body = "Test Body \n" + strBody;

                //SmtpClient smtp = new SmtpClient
                //{
                //    Host = "smtp.gmail.com",
                //    Port = 587,
                //    EnableSsl = true,
                //    DeliveryMethod = SmtpDeliveryMethod.Network,
                //    UseDefaultCredentials = false,
                //    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                //};
                //using (MailMessage message = new MailMessage(fromAddress, toAddress)
                //{
                //    Subject = subject,
                //    Body = body
                //})
                //{
                //    smtp.Send(message);
                //}
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public static void SendEmailToMultipleUser(string strBody, List<string> toEmails)
        {

            try
            {
                var fromAddress = new MailAddress("mansoorsmtp@gmail.com", "TI CRM");
                //var toAddress = new MailAddress("mansoor@techimplement.com", "Mansoor");
                const string fromPassword = "@dmin1234";
                //const string subject = "A New Lead Arrived";
                //string body = "Test Body \n" + strBody;
                //var smtp = new SmtpClient
                //{
                //    Host = "smtp.gmail.com",
                //    Port = 587,
                //    EnableSsl = true,
                //    DeliveryMethod = SmtpDeliveryMethod.Network,
                //    UseDefaultCredentials = false,
                //    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                //};
                //using (var message = new MailMessage(fromAddress, toAddress)
                //{
                //    Subject = subject,
                //    Body = body
                //})
                //{
                //    smtp.Send(message);
                //}





                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                message.From = new MailAddress("mansoorsmtp@gmail.com", "TI CRM");

                foreach (var item in toEmails)
                {
                    message.To.Add(new MailAddress(item));

                }
                message.Subject = "A New Lead Arrived";
                message.Body = strBody;

                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(fromAddress.Address, fromPassword);

                smtp.Send(message);
            }
            catch (Exception)
            {

                throw;
            }

        }


    }

    class NonOrderingBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }


    public static class BundleExtentions
    {
        public static Bundle NonOrdering(this Bundle bundle)
        {
            bundle.Orderer = new NonOrderingBundleOrderer();
            return bundle;
        }
    }
}