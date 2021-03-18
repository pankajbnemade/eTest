using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ITaxRegisterDetail : IRepository<Taxregisterdetail>
    {
        Task<int> CreateTaxRegisterDetail(TaxRegisterDetailModel taxRegisterDetailModel);
       
        Task<bool> UpdateTaxRegisterDetail(TaxRegisterDetailModel taxRegisterDetailModel);
      
        Task<bool> DeleteTaxRegisterDetail(int taxRegisterDetailId);
        
        Task<TaxRegisterDetailModel> GetTaxRegisterDetailById(int taxRegisterDetailId);

        //Task<TaxRegisterDetailModel> GetTaxRegisterDetailByTaxRegisterId(int taxRegisterId);

        Task<DataTableResultModel<TaxRegisterDetailModel>> GetTaxRegisterDetailList();
    }
}
