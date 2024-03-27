using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace TopLearn.Core.Senders
{
    public class SendEmail
    {
        public static void Send(string to, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("sepehrhashemi.f2024@gmail.com", "تاپ لرن");
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            //System.Net.Mail.Attachment attachment;
            // attachment = new System.Net.Mail.Attachment("c:/textfile.txt");
            // mail.Attachments.Add(attachment);

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("sepehrhashemi.f2024@gmail.com", "*******");
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);

        }
    } 

    //public class SendEmail2
    //{

    //    var mailerSend = new MailerSend("api_key");
    //    var recipients = new List<Recipient>
    //    {
    //    new Recipient("recipient@email.com", "Recipient")
    //    };

    //    var emailParams = new EmailParams
    //    {
    //        From = new From("info@trial-7dnvo4de27nl5r86.mlsender.net", "Your Name"),
    //        Recipients = recipients,
    //        Subject = "Subject",
    //        Html = "This is the HTML content",
    //        Text = "This is the text content"
    //    };
    //    mailerSend.Email.Send(emailParams);
    //}
}