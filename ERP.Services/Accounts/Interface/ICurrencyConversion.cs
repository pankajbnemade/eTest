using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Master;
using System;
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

        Task<DataTableResultModel<CurrencyConversionModel>> GetCurrencyConversionByCurrencyId(int currencyId);

        Task<DataTableResultModel<CurrencyConversionModel>> GetCurrencyConversionList();

        Task<CurrencyConversionModel> GetExchangeRateByCurrencyId(int currencyId, DateTime invoiceDate);
    }
}
