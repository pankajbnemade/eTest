using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IFinancialYear : IRepository<Financialyear>
    {
        Task<int> CreateFinancialYear(FinancialYearModel financialYearModel);

        Task<bool> UpdateFinancialYear(FinancialYearModel financialYearModel);

        Task<bool> DeleteFinancialYear(int financialYearId);

        Task<FinancialYearModel> GetFinancialYearById(int financialYearId);

        Task<FinancialYearModel> GetFinancialYearByDateNCompanyId(int companyId, DateTime date);

        Task<DataTableResultModel<FinancialYearModel>> GetFinancialYearList();

        Task<IList<SelectListModel>> GetFinancialYearSelectList();
    }
}
