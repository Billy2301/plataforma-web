using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface IEmailQueueService
    {
        Task SaveEmailQueue(string emailSubject, string emailBody, string emailTo, string createdBy, string replyTo);
        Task SaveEmailQueueDiag(string emailSubject, string emailBody, string emailTo, string createdBy, string replyTo);
        Task SaveEmailQueueTrat(string emailSubject, string emailBody, string emailTo, string createdBy, string replyTo);
    }
}
