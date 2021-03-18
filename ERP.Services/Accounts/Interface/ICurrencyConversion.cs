using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ICurrencyConversion : IRepository<Currencyconversion>
    {
        Task<int> CreateCurrencyConversion(CurrencyConversionModel currencyConversionModel);
       
        Task<bool> UpdateCurrencyConversion(CurrencyConversionModel currencyConversionModel);
      
        Task<bool> DeleteCurrencyConversion(int currencyConversionId);
        
        Task<CurrencyConversionModel> GetCurrencyConversionById(int currencyConversionId);

        //Task<CurrencyConversionModel> GetCurrencyConversionByCurrencyId(int currencyId);
        
        Task<DataTableResultModel<CurrencyConversionModel>> GetCurrencyConversionList();
    }
}
