using ERP.Models.Accounts;
using ERP.Models.Common;
using System;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IReceivableStatement
    {
        Task<DataTableResultModel<ReceivableStatementModel>> GetReport(SearchFilterReceivableStatementModel searchFilterModel, DateTime fromDate_FY, DateTime toDate_FY);
    }
}
