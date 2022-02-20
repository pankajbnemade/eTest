using ERP.Models.Accounts;
using ERP.Models.Common;
using System;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IContraRegister
    {
        Task<DataTableResultModel<ContraRegisterModel>> GetReport(SearchFilterContraRegisterModel searchFilterModel, DateTime fromDate_FY, DateTime toDate_FY);
    }
}
