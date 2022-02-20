using ERP.Models.Accounts;
using ERP.Models.Common;
using System;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IReceiptRegister
    {
        Task<DataTableResultModel<ReceiptRegisterModel>> GetReport(SearchFilterReceiptRegisterModel searchFilterModel, DateTime fromDate_FY, DateTime toDate_FY);
    }
}
