using BL.IServices;
using DA.IRepository;
using Entity.CentralModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class EmailQueueService : IEmailQueueService
    {
        private readonly IGenericRepositoryCentral<EmailQueue> _Repository;
        private readonly IConfigSistemaService _configSistemaService;
        private readonly IConfiguration _configuration;

        public EmailQueueService(IGenericRepositoryCentral<EmailQueue> repository, IConfigSistemaService configSistemaService, IConfiguration configuration)
        {
            _Repository = repository;
            _configSistemaService = configSistemaService;
            _configuration = configuration;
        }

        public async Task SaveEmailQueue(string emailSubject, string emailBody, string emailTo, string createdBy, string replyTo)
        {

            EmailQueue emailQueue = new EmailQueue();

            emailQueue.SystemId = Convert.ToInt16(_configuration.GetSection("sistema_id").Value);
            emailQueue.EmailFrom = await _configSistemaService.getVariable("CPAL_DIAG_EMAIL_USER");
            emailQueue.EmailFromDisplay = await _configSistemaService.getVariable("CPAL_DIAG_EMAIL_USER_DISPLAY");
            emailQueue.EmailTo = emailTo; //separados por coma (,) si son varios
            emailQueue.ReplyTo = replyTo;
            if (!string.IsNullOrEmpty(_configuration.GetSection("emailBcc").Value)) emailQueue.EmailBcc = _configuration.GetSection("emailBcc").Value;
            emailQueue.EmailSubject = emailSubject;
            emailQueue.EmailBody = emailBody;
            emailQueue.CreatedBy = createdBy;
            emailQueue.CreatedDate = DateTime.Now;
            emailQueue.SmtpServer = await _configSistemaService.getVariable("CPAL_DIAG_EMAIL_SERVER");
            emailQueue.SmtpUser = await _configSistemaService.getVariable("CPAL_DIAG_EMAIL_USER");
            emailQueue.SmtpPwd = await _configSistemaService.getVariable("CPAL_DIAG_EMAIL_PWD");
            emailQueue.SmtpPort = await _configSistemaService.getVariable("CPAL_DIAG_EMAIL_PORT");
            emailQueue.SmtpSsl = await _configSistemaService.getVariable("CPAL_DIAG_EMAIL_SSL") == "1";
            emailQueue.QueueStatus = "PEN";

            await _Repository.Crear(emailQueue);
        }

        public async Task SaveEmailQueueDiag(string emailSubject, string emailBody, string emailTo, string createdBy, string replyTo)
        {

            EmailQueue emailQueue = new EmailQueue();

            emailQueue.SystemId = Convert.ToInt16(_configuration.GetSection("sistema_id").Value);
            emailQueue.EmailFrom = await _configSistemaService.getVariable("CPAL_DIAG_EMAIL_USER");
            emailQueue.EmailFromDisplay = await _configSistemaService.getVariable("CPAL_DIAG_EMAIL_USER_DISPLAY");
            emailQueue.EmailTo = emailTo; //separados por coma (,) si son varios
            emailQueue.ReplyTo = replyTo;
            if (!string.IsNullOrEmpty(_configuration.GetSection("emailBcc").Value)) emailQueue.EmailBcc = _configuration.GetSection("emailBcc").Value;
            emailQueue.EmailSubject = emailSubject;
            emailQueue.EmailBody = emailBody;
            emailQueue.CreatedBy = createdBy;
            emailQueue.CreatedDate = DateTime.Now;
            emailQueue.SmtpServer = await _configSistemaService.getVariable("CPAL_DIAG_EMAIL_SERVER");
            emailQueue.SmtpUser = await _configSistemaService.getVariable("CPAL_DIAG_EMAIL_USER");
            emailQueue.SmtpPwd = await _configSistemaService.getVariable("CPAL_DIAG_EMAIL_PWD");
            emailQueue.SmtpPort = await _configSistemaService.getVariable("CPAL_DIAG_EMAIL_PORT");
            emailQueue.SmtpSsl = await _configSistemaService.getVariable("CPAL_DIAG_EMAIL_SSL") == "1";
            emailQueue.QueueStatus = "PEN";

            await _Repository.Crear(emailQueue);
        }

        public async Task SaveEmailQueueTrat(string emailSubject, string emailBody, string emailTo, string createdBy, string replyTo)
        {

            EmailQueue emailQueue = new EmailQueue();

            emailQueue.SystemId = Convert.ToInt16(_configuration.GetSection("sistema_id").Value);
            emailQueue.EmailFrom = await _configSistemaService.getVariable("CPAL_TRAT_EMAIL_USER");
            emailQueue.EmailFromDisplay = await _configSistemaService.getVariable("CPAL_TRAT_EMAIL_USER_DISPLAY");
            emailQueue.EmailTo = emailTo; //separados por coma (,) si son varios
            emailQueue.ReplyTo = replyTo;
            if (!string.IsNullOrEmpty(_configuration.GetSection("emailBcc").Value)) emailQueue.EmailBcc = _configuration.GetSection("emailBcc").Value;
            emailQueue.EmailSubject = emailSubject;
            emailQueue.EmailBody = emailBody;
            emailQueue.CreatedBy = createdBy;
            emailQueue.CreatedDate = DateTime.Now;
            emailQueue.SmtpServer = await _configSistemaService.getVariable("CPAL_TRAT_EMAIL_SERVER");
            emailQueue.SmtpUser = await _configSistemaService.getVariable("CPAL_TRAT_EMAIL_USER");
            emailQueue.SmtpPwd = await _configSistemaService.getVariable("CPAL_TRAT_EMAIL_PWD");
            emailQueue.SmtpPort = await _configSistemaService.getVariable("CPAL_TRAT_EMAIL_PORT");
            emailQueue.SmtpSsl = await _configSistemaService.getVariable("CPAL_TRAT_EMAIL_SSL") == "1";
            emailQueue.QueueStatus = "PEN";

            await _Repository.Crear(emailQueue);
        }
    }
}
