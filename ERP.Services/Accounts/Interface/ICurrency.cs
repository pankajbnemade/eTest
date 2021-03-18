using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ICurrency : IRepository<Currency>
    {
        Task<int> CreateCurrency(CurrencyModel currencyModel);
       
        Task<bool> UpdateCurrency(CurrencyModel currencyModel);
      
        Task<bool> DeleteCurrency(int currencyId);
        
        Task<CurrencyModel> GetCurrencyById(int currencyId);
        
        Task<DataTableResultModel<CurrencyModel>> GetCurrencyList();
    }
}
