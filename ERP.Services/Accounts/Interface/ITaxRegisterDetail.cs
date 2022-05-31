using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ITaxRegisterDetail : IRepository<Taxregisterdetail>
    {
        Task<int> GenerateSrNo(int taxRegisterId);
        Task<int> CreateTaxRegisterDetail(TaxRegisterDetailModel taxRegisterDetailModel);

        Task<bool> UpdateTaxRegisterDetail(TaxRegisterDetailModel taxRegisterDetailModel);

        Task<bool> DeleteTaxRegisterDetail(int taxRegisterDetailId);

        Task<TaxRegisterDetailModel> GetTaxRegisterDetailById(int taxRegisterDetailId);

        Task<DataTableResultModel<TaxRegisterDetailModel>> GetTaxRegisterDetailByTaxRegisterId(int taxRegisterId);

        Task<IList<TaxRegisterDetailModel>> GetTaxRegisterDetailListByTaxRegisterId(int taxRegisterId);

        //Task<DataTableResultModel<TaxRegisterDetailModel>> GetTaxRegisterDetailList();
    }
}
