using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IFinancialYearCompanyRelation : IRepository<Financialyearcompanyrelation>
    {
        Task<int> CreateFinancialYearCompanyRelation(FinancialYearCompanyRelationModel financialYearCompanyRelationModel);

        Task<bool> UpdateFinancialYearCompanyRelation(FinancialYearCompanyRelationModel financialYearCompanyRelationModel);

        Task<bool> DeleteFinancialYearCompanyRelation(int financialYearCompanyRelationId);

        Task<FinancialYearCompanyRelationModel> GetFinancialYearCompanyRelationById(int financialYearCompanyRelationId);

        //Task<FinancialYearCompanyRelationModel> GetFinancialYearCompanyRelationByFinantialYearId(int finantialYearId);

        Task<DataTableResultModel<FinancialYearCompanyRelationModel>> GetFinancialYearCompanyRelationList();
    }
}
