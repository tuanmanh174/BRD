using BRD.Common.Infrastructure;
using BRD.DataModel.Email;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;

namespace BRD.Service
{
    public interface IMailService
    {
        bool SendMail(string to, string[] bcc, string subject, string body, Stream attachment = null,
            string fileName = null, string fileType = null);

        bool SendDevelopMail(string from, string to, string subject, string body);

        void SendDeviceConnectionStatusMail(string email, string subject, string contents, CultureInfo culture);

        void SendQRMail(string email, string subject, string contents, CultureInfo culture, Stream attachment, string fileName, string fileType);

        string GetSupportMailAddress();
        string GetPathToTemplateFile(string fileName);
        string GetPathToImageFile(string fileName);
        string ConvertImageToBase64(string pathToImageFile);
        string GetFrontEndURL();
    }
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private IHostingEnvironment _env;

        public MailService(IConfiguration configuration,
            ILogger<MailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _env = ApplicationVariables.env;
        }

        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="to"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="attachment"></param>
        /// <param name="fileName"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public bool SendMail(string to, string[] bcc, string subject, string body
            , Stream attachment = null, string fileName = null, string fileType = null)
        {
            try
            {
                var mailSettings = _configuration.GetSection(Constants.Settings.MailSettings).Get<MailSettings>();

                using (var client = new SmtpClient(mailSettings.Host))
                {
                    client.Host = mailSettings.Host;
                    if (!string.IsNullOrEmpty(mailSettings.Port))
                    {
                        client.Port = int.Parse(mailSettings.Port);
                    }

                    client.EnableSsl = Convert.ToBoolean(mailSettings.EnableSsl);
                    client.UseDefaultCredentials = Convert.ToBoolean(mailSettings.DefaultCredentials);
                    client.Credentials = new System.Net.NetworkCredential(mailSettings.UserName, mailSettings.Password);

                    Attachment att = null;
                    if (attachment != null)
                    {
                        att = new Attachment(attachment, fileName, fileType);
                    }

                    using (var msg = new MailMessage())
                    {
                        var view = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8,
                            "text/html");
                        msg.From = new MailAddress(mailSettings.UserName, mailSettings.From);
                        msg.Sender = new MailAddress(mailSettings.UserName, mailSettings.From);
                        msg.AlternateViews.Add(view);
                        msg.IsBodyHtml = true;
                        msg.SubjectEncoding = Encoding.UTF8;
                        msg.BodyEncoding = Encoding.UTF8;
                        msg.Subject = subject;
                        msg.Body = body;

                        if (att != null)
                            msg.Attachments.Add(att);


                        if (!String.IsNullOrEmpty(to))
                        {
                            try
                            {
                                msg.To.Add(to);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
                                _logger.LogError("Mail address : " + to);

                                return false;
                            }

                            try
                            {
                                if (bcc != null && bcc.Any())
                                {
                                    foreach (var email in bcc)
                                    {
                                        msg.Bcc.Add(email);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
                            }

                            client.Send(msg);
                        }
                        else
                        {
                            _logger.LogWarning("Email with subject:\"" + subject + "\" missing To address.");
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to send E-mail.");
                _logger.LogError($"{ex.Message}:{Environment.NewLine} {ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// Send mail
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public bool SendDevelopMail(string from, string to, string subject, string body)
        {
            try
            {
                var mailSettings = _configuration.GetSection(Constants.Settings.MailSettings)
                    .Get<MailSettings>();
                using (var client = new SmtpClient(mailSettings.Host))
                {
                    client.Host = mailSettings.Host;
                    if (!string.IsNullOrEmpty(mailSettings.Port))
                    {
                        client.Port = int.Parse(mailSettings.Port);
                    }
                    client.EnableSsl = Convert.ToBoolean(mailSettings.EnableSsl);
                    client.UseDefaultCredentials = Convert.ToBoolean(mailSettings.DefaultCredentials);
                    client.Credentials = new System.Net.NetworkCredential(mailSettings.UserName, mailSettings.Password);

                    using (var msg = new MailMessage())
                    {
                        var view = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8,
                            "text/html");
                        msg.From = new MailAddress(from);
                        msg.Sender = new MailAddress(from);
                        msg.AlternateViews.Add(view);
                        msg.IsBodyHtml = true;
                        msg.SubjectEncoding = Encoding.UTF8;
                        msg.BodyEncoding = Encoding.UTF8;
                        msg.Subject = subject;
                        msg.Body = body;
                        msg.To.Add(to);

                        client.Send(msg);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}:{Environment.NewLine} {ex.StackTrace}");
                return false;
            }
        }


        /// <summary>
        /// Get Email address for customer support
        /// </summary>
        /// <returns> support Email address </returns>
        public string GetSupportMailAddress()
        {
            var mailDevelopSettings = _configuration.GetSection(Constants.Settings.MailDevelopSettings).Get<MailDevelopSettings>();

            var supportMail = mailDevelopSettings.To;

            return supportMail;
        }

        /// <summary>
        /// get path of template file
        /// </summary>
        /// <param name="fileName"> name of template file </param>
        /// <returns> template file path </returns>
        public string GetPathToTemplateFile(string fileName)
        {
            var webRoot = _env.WebRootPath;

            var pathToFile = webRoot
                    + Path.DirectorySeparatorChar.ToString()
                    + "Templates"
                    + Path.DirectorySeparatorChar.ToString()
                    + "EmailTemplate"
                    + Path.DirectorySeparatorChar.ToString()
                    + fileName;

            return pathToFile;
        }

        /// <summary>
        /// get path of image file
        /// </summary>
        /// <param name="fileName"> name of image file </param>
        /// <returns> image file path </returns>
        public string GetPathToImageFile(string fileName)
        {
            var webRoot = _env.WebRootPath;

            var pathToFile = webRoot
                            + Path.DirectorySeparatorChar.ToString()
                            + "Templates"
                            + Path.DirectorySeparatorChar.ToString()
                            + "EmailTemplate"
                            + Path.DirectorySeparatorChar.ToString()
                            + "images"
                            + Path.DirectorySeparatorChar.ToString()
                            + fileName;

            return pathToFile;
        }

        /// <summary>
        /// Convert Image file path to Base64
        /// </summary>
        /// <param name="pathToImageFile"></param>
        /// <returns> converted image to Base64 </returns>
        public string ConvertImageToBase64(string pathToImageFile)
        {
            var bytes = File.ReadAllBytes(pathToImageFile);
            var b64String = Convert.ToBase64String(bytes);

            return b64String;
        }

        /// <summary>
        /// get front-end URL
        /// </summary>
        /// <returns> front-end URL </returns>
        public string GetFrontEndURL()
        {
            var frontendURL = _configuration.GetSection("WebApp:Host").Value;

            if (frontendURL.Equals("localhost"))
                frontendURL = "http://" + frontendURL;

            return frontendURL;
        }

        /// <summary>
        /// Send email when device's connection status is changed.
        /// </summary>
        /// <param name="email"> email address </param>
        public void SendDeviceConnectionStatusMail(string email, string subject, string contents, CultureInfo culture)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var supportMail = GetSupportMailAddress();

                var frontendURL = GetFrontEndURL();

                var thread = new Thread(delegate ()
                {
                    var pathToTemplateFile = GetPathToTemplateFile("Plain_Email.html");

                    var pathToImage = GetPathToImageFile("logo.png");
                    var b64String = ConvertImageToBase64(pathToImage);
                    var imageUrl = "data:image/png;base64," + b64String;

                    BodyBuilder builder = new BodyBuilder();

                    try
                    {
                        using (StreamReader SourceReader = File.OpenText(pathToTemplateFile))
                        {
                            builder.HtmlBody = SourceReader.ReadToEnd();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("The process failed: {0}", e.ToString());
                    }

                    //string customerSupport = String.Format(MailContentResource.ResourceManager.GetString("BodyCustomerSupport", culture),
                    //                                    MailContentResource.ResourceManager.GetString("BodyWorkingTimeInfo", culture), supportMail);
                    //string replyMessage = String.Format(MailContentResource.ResourceManager.GetString("BodyReplyMessage", culture), supportMail);

                    //string mailBody = string.Format(builder.HtmlBody,
                    //                                contents,
                    //                                customerSupport,
                    //                                replyMessage,
                    //                                imageUrl);

                    string mailBody = string.Format(builder.HtmlBody,
                                                   contents,
                                                   "",
                                                   "",
                                                   imageUrl);

                    SendMail(email, null, subject, mailBody);
                });
                thread.Start();
            }
        }


        /// <summary>
        /// Send email include QR image to user.
        /// </summary>
        /// <param name="email"> email address </param>
        public void SendQRMail(string email, string subject, string contents, CultureInfo culture, Stream attachment, string fileName, string fileType)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var supportMail = GetSupportMailAddress();

                var frontendURL = GetFrontEndURL();

                var thread = new Thread(delegate ()
                {
                    var pathToTemplateFile = GetPathToTemplateFile("Plain_Email.html");

                    var pathToImage = GetPathToImageFile("logo.png");
                    var b64String = ConvertImageToBase64(pathToImage);
                    var imageUrl = "data:image/png;base64," + b64String;

                    BodyBuilder builder = new BodyBuilder();

                    try
                    {
                        using (StreamReader SourceReader = File.OpenText(pathToTemplateFile))
                        {
                            builder.HtmlBody = SourceReader.ReadToEnd();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("The process failed: {0}", e.ToString());
                    }

                    //string customerSupport = String.Format(MailContentResource.ResourceManager.GetString("BodyCustomerSupport", culture),
                    //                                    MailContentResource.ResourceManager.GetString("BodyWorkingTimeInfo", culture), supportMail);
                    //string replyMessage = String.Format(MailContentResource.ResourceManager.GetString("BodyReplyMessage", culture), supportMail);

                    //string mailBody = string.Format(builder.HtmlBody,
                    //                                contents,
                    //                                customerSupport,
                    //                                replyMessage,
                    //                                imageUrl);
                    string mailBody = string.Format(builder.HtmlBody,
                                                    contents,
                                                    "",
                                                    "",
                                                    imageUrl);

                    SendMail(email, null, subject, mailBody, attachment, fileName, fileType);
                });
                thread.Start();
            }
        }
    }
}
