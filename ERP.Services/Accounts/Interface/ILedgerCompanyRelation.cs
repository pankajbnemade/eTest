using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ILedgerCompanyRelation : IRepository<Ledgercompanyrelation>
    {
        Task<int> CreateLedgerCompanyRelation(LedgerCompanyRelationModel ledgerCompanyRelationModel);

        Task<bool> DeleteLedgerCompanyRelation(int ledgerCompanyRelationId);

        Task<LedgerCompanyRelationModel> GetLedgerCompanyRelationById(int ledgerCompanyRelationId);

        Task<DataTableResultModel<LedgerCompanyRelationModel>> GetLedgerCompanyRelationByLedgerId(int finantialYearId);

        Task<DataTableResultModel<LedgerCompanyRelationModel>> GetLedgerCompanyRelationList();
    }
}
