using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface I : IRepository<Currency>
    {
        Task<int> CreateCurrency(CurrencyModel currencyModel);
       
        Task<bool> UpdateCurrency(CurrencyModel currencyModel);
      
        Task<bool> DeleteCurrency(int currencyId);
        
        Task<CurrencyModel> GetCurrencyById(int currencyId);

        Task<CurrencyModel> GetCurrencyBysId(int sId);
        Task<DataTableResultModel<CurrencyModel>> GetCurrencyList();
    }
}
