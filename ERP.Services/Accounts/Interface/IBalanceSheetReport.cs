using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IBalanceSheetReport
    {
        Task<DataTableResultModel<BalanceSheetReportModel>> GetReport(SearchFilterBalanceSheetReportModel searchFilterModel, DateTime fromDate_FY, DateTime toDate_FY);
    }
}
