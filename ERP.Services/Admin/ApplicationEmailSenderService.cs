using ERP.DataAccess.Entity;
using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Admin;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using ERP.Services.Admin.Interface;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Admin
{
    public class ApplicationEmailSenderService : IApplicationIEmailSender<IEmailSender>
    {
        public ApplicationEmailSenderService()  { }

    }
}
