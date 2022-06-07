using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IProfitAndLossReport
    {
        Task<DataTableResultModel<ProfitAndLossReportModel>> GetReport(SearchFilterProfitAndLossReportModel searchFilterModel, DateTime fromDate_FY, DateTime toDate_FY);
    }
}
