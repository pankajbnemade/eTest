using ERP.Models.Accounts;
using ERP.Models.Common;
using System;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IPDCReport
    {
        Task<DataTableResultModel<PDCReportModel>> GetReport(SearchFilterPDCReportModel searchFilterModel, DateTime fromDate_FY, DateTime toDate_FY);
    }
}
