using System;
using System.Threading.Tasks;
using BRD.Common.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using BRD.DataModel.Api;
using BRD.DataModel.Email;
using BRD.Service;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using StatusCodes = Microsoft.AspNetCore.Http.StatusCodes;

namespace BRD.API.Infrastructure
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IOptions<MvcNewtonsoftJsonOptions> _jsonOptions;
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="jsonOptions"></param>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="mailService"></param>
        public ErrorHandlerMiddleware(RequestDelegate next, IOptions<MvcNewtonsoftJsonOptions> jsonOptions,
            ILogger<ErrorHandlerMiddleware> logger, IConfiguration configuration, IMailService mailService)
        {
            _next = next;
            _jsonOptions = jsonOptions;
            _logger = logger;
            _configuration = configuration;
            _mailService = mailService;
        }

        /// <summary>
        /// Invoke
        /// </summary>
        /// <param name="context"></param>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                SendMailError(exception, context);
                var errorMessage = $"{exception.Message}{Environment.NewLine}{exception.StackTrace}";
                _logger.LogError(errorMessage);
                await HandleErrorAsync(context, "SystemError");
            }
        }

        private Task HandleErrorAsync(HttpContext context, string errorMessage)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            var response = new ApiErrorResultModel(context.Response.StatusCode, errorMessage);
            var payload = JsonConvert.SerializeObject(response, _jsonOptions.Value.SerializerSettings);
            return context.Response.WriteAsync(payload);
        }

        private void SendMailError(Exception exception, HttpContext context)
        {
            var mailSettings = _configuration.GetSection(Constants.Settings.MailDevelopSettings)
                .Get<MailDevelopSettings>();
            if (mailSettings != null)
            {
                //var subject = string.Format(MailContentResource.SubjectInternalServerError, exception.Message);
                //var body = string.Format(MailContentResource.BodyInternalServerError, context.User.Identity.Name,
                //    context.Request.GetDisplayUrl(), $"{exception.Message}{Environment.NewLine}{exception.StackTrace}",
                //    $"{exception.InnerException?.Message}{Environment.NewLine}{exception.InnerException?.StackTrace}");
                //_mailService.SendDevelopMail(mailSettings.From, mailSettings.To, subject, body);
            }
        }
    }
}
