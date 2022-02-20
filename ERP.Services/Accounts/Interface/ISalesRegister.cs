using ERP.Models.Accounts;
using ERP.Models.Common;
using System;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ISalesRegister
    {
        Task<DataTableResultModel<SalesRegisterModel>> GetReport(SearchFilterSalesRegisterModel searchFilterModel, DateTime fromDate_FY, DateTime toDate_FY);
    }
}
