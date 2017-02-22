using StorageBox.Contracts;
using StorageBox.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Implementations
{
    class EMailService : IEMailService
    {
        private SmtpClient _smtpClient;
        private NetworkCredential _basicCredential;
        private MailAddress _fromAddress;

        public EMailService()
        {
            _smtpClient = new SmtpClient();
            _basicCredential = new NetworkCredential(Properties.Settings.Default.ServiceEmail, Properties.Settings.Default.EmailPassword);
            _fromAddress = new MailAddress(Properties.Settings.Default.ServiceEmail);

            _smtpClient.Host = Properties.Settings.Default.SMTPServer;
            _smtpClient.UseDefaultCredentials = false;
            _smtpClient.Credentials = _basicCredential;

            //MailMessage message = new MailMessage();
        }

        public void sendAvailabilityWarning(List<ProductSKU> underThresholdSKU)
        {
            MailMessage message = new MailMessage();
            message.From = _fromAddress;
            message.Subject = "Powiadomienie o wyczerpywanych zasobach";
            //Set IsBodyHtml to true means you can send HTML email.
            message.IsBodyHtml = true;

            message.Body = "<h1>Powiadomienie</h1>";
            message.Body += "<ul>";
            foreach (ProductSKU productsku in underThresholdSKU)
            {
                message.Body += "<li>" + productsku.Product.ProductName + " [" + productsku.Sku + "] - Dostępnych: " + productsku.Boxes.Count + "</li>";
            }
            message.Body += "</ul>";

            message.To.Add(Properties.Settings.Default.RecipientEmail);

            Trace.WriteLine("Sending...");

            try
            {
                _smtpClient.Send(message);
                Trace.WriteLine("Sent.");
            }
            catch (Exception ex)
            {
                //Error, could not send the message
                Trace.Write(ex.Message);
            }
        }
    }
}
