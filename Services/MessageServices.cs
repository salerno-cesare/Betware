using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace Betware.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {

        private readonly string _smtpAddress;
        private readonly string _smtpPass;
        private readonly string _smtpSender;
        private readonly string _smtpUser;

        private readonly ILogger _logger;

        public AuthMessageSender(ILogger<AuthMessageSender> logger, IConfiguration config)
        {
            _smtpUser = config["MailManager:user"];
            _smtpPass = config["MailManager:password"];
            _smtpAddress = config["MailManager:address"];
            _smtpSender = config["MailManager:sender"];
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            try
            {
                _logger.LogInformation($"email: {email}, subj: { subject}, message: {message} ");
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = "<b>"+message+"</b>";
                //bodyBuilder.TextBody = message;
                var mes = new MimeMessage();
                //mes.From.Add(new MailboxAddress("Email Betware", _smtpSender));
                mes.From.Add(new MailboxAddress("Email Betware", "noreply@betware.com"));
                mes.To.Add(new MailboxAddress(email, email));
                mes.Subject = subject;
                //mes.Body = new TextPart(message);
                mes.Body = bodyBuilder.ToMessageBody();

                using (var smtpClient = new SmtpClient())
                {
                    //smtp.Connect(_smtpAddress, 587, false);
                    smtpClient.Connect("smtp.gmail.com", 587);
                    smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                    //smtpClient.Authenticate("_smtpUser", "_smtpPass");
                    smtpClient.Authenticate("betwarentt@gmail.com", "Betware123!");
                    smtpClient.Send(mes);
                    smtpClient.Disconnect(true);
                    ////if (recipient == null || recipient.Count < 1)
                    ////    throw new Exception("Please add at least one recipient");

                    ////Setting host and credential
                    //if (!string.IsNullOrEmpty(_smtpUser) && !string.IsNullOrEmpty(_smtpPass))
                    //{
                    //    var basicCredential = new NetworkCredential(_smtpUser, _smtpPass);
                    //    smtp.UseDefaultCredentials = false;
                    //    smtp.Credentials = basicCredential;


                    //}

                    //smtp.Host = _smtpAddress;

                    //var mes = new MailMessage
                    //{
                    //    From = new MailAddress(_smtpSender),
                    //    BodyEncoding = System.Text.Encoding.UTF8,
                    //    SubjectEncoding = System.Text.Encoding.UTF8
                    //};

                    ////foreach (var to in recipient)
                    //mes.To.Add(email);

                    ////if (carbonCopyRecipient != null)
                    ////    foreach (var to in carbonCopyRecipient) mes.CC.Add(to);

                    //mes.Body = message;
                    //mes.Subject = subject;
                    //mes.IsBodyHtml = true;
                    //await smtp.SendMailAsync(mes);

                    //_logger.LogInformation($"mail with subject {subject}, sended correctly");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot send mail: {e.Message}");
                var iex = e.InnerException;
                _logger.LogError($"InnerEx:  {iex }");
                while (iex != null)
                {
                    _logger.LogError($" ---> {iex.Message}");
                    iex = iex.InnerException;
                }
                _logger.LogError("=== SendMail - StackTrace ===");
                _logger.LogError(e.StackTrace);

            }
            //return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
