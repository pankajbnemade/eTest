using ERP.Models.Common;
using ERP.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Common.Interface
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequestModel mailRequestModel);
        Task SendWelcomeEmailAsync(WelcomeRequestModel request);
    }
}
