using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RailswareTestTask.Interfaces
{
    public interface IMailService
    {
        bool SendMail(MailData mailData);
    }
}
