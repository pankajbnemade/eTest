using ERP.Models.Accounts;
using ERP.Models.Common;
using System;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IJournalRegister
    {
        Task<DataTableResultModel<JournalRegisterModel>> GetReport(SearchFilterJournalRegisterModel searchFilterModel, DateTime fromDate_FY, DateTime toDate_FY);
    }
}
