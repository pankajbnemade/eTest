using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Services.Accounts.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class CurrencyConversionService : Repository<Currencyconversion>, ICurrencyConversion
    {
        public CurrencyConversionService(ErpDbContext dbContext) : base(dbContext) { }


        public async Task<int> CreateCurrencyConversion(CurrencyConversionModel currencyConversionModel)
        {
            int currencyConversionId = 0;

            // assign values.
            Currencyconversion currencyConversion = new Currencyconversion();

            currencyConversion.ConversionId = currencyConversionModel.ConversionId;
            currencyConversion.CompanyId = currencyConversionModel.CompanyId;
            currencyConversion.CurrencyId = currencyConversionModel.CurrencyId;
            currencyConversion.EffectiveDateTime = currencyConversionModel.EffectiveDateTime;
            currencyConversion.ExchangeRate = currencyConversionModel.ExchangeRate;

            currencyConversionId = await Create(currencyConversion);

            return currencyConversionId; // returns.
        }


        public async Task<bool> UpdateCurrencyConversion(CurrencyConversionModel currencyConversionModel)
        {
            bool isUpdated = false;

            // get record.
            Currencyconversion currencyConversion = await GetByIdAsync(w => w.ConversionId == currencyConversionModel.ConversionId);

            if (null != currencyConversion)
            {
                // assign values.
                currencyConversion.EffectiveDateTime = currencyConversionModel.EffectiveDateTime;
                currencyConversion.ExchangeRate = currencyConversionModel.ExchangeRate;

                isUpdated = await Update(currencyConversion);
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeleteCurrencyConversion(int currencyConversionId)
        {
            bool isDeleted = false;

            // get record.
            Currencyconversion currencyConversion = await GetByIdAsync(w => w.ConversionId == currencyConversionId);

            if (null != currencyConversion)
            {
                isDeleted = await Delete(currencyConversion);
            }

            return isDeleted; // returns.
        }


        public async Task<CurrencyConversionModel> GetCurrencyConversionById(int currencyConversionId)
        {
            CurrencyConversionModel currencyConversionModel = null;

            IList<CurrencyConversionModel> currencyConversionModelList = await GetCurrencyConversionList(currencyConversionId, 0);

            if (null != currencyConversionModelList && currencyConversionModelList.Any())
            {
                currencyConversionModel = currencyConversionModelList.FirstOrDefault();
            }

            return currencyConversionModel; // returns.
        }


        public async Task<CurrencyConversionModel> GetExchangeRateByCurrencyId(int currencyId, DateTime invoiceDate)
        {
            CurrencyConversionModel currencyConversionModel = null;

            // create query.
            if (await Any(w => w.CurrencyId == currencyId && w.EffectiveDateTime <= invoiceDate))
            {
                Currencyconversion currencyConversion = 
                    await GetQueryByCondition(w => w.CurrencyId == currencyId && w.EffectiveDateTime <= invoiceDate)
                    .Include(w => w.Currency).Include(w => w.Company).Include(w => w.PreparedByUser)                                     
                    .OrderByDescending(w => w.EffectiveDateTime).FirstOrDefaultAsync();

                currencyConversionModel = await AssignValueToModel(currencyConversion);
            }

            return currencyConversionModel; // returns.
        }


        public async Task<DataTableResultModel<CurrencyConversionModel>> GetCurrencyConversionByCurrencyId(int currencyId)
        {
            DataTableResultModel<CurrencyConversionModel> resultModel = new DataTableResultModel<CurrencyConversionModel>();

            IList<CurrencyConversionModel> currencyConversionModelList = await GetCurrencyConversionList(0, currencyId);

            if (null != currencyConversionModelList && currencyConversionModelList.Any())
            {
                resultModel = new DataTableResultModel<CurrencyConversionModel>();
                resultModel.ResultList = currencyConversionModelList;
                resultModel.TotalResultCount = currencyConversionModelList.Count();
            }

            return resultModel; // returns.
        }


        public async Task<DataTableResultModel<CurrencyConversionModel>> GetCurrencyConversionList()
        {
            DataTableResultModel<CurrencyConversionModel> resultModel = new DataTableResultModel<CurrencyConversionModel>();

            IList<CurrencyConversionModel> currencyConversionModelList = await GetCurrencyConversionList(0, 0);

            if (null != currencyConversionModelList && currencyConversionModelList.Any())
            {
                resultModel = new DataTableResultModel<CurrencyConversionModel>();
                resultModel.ResultList = currencyConversionModelList;
                resultModel.TotalResultCount = currencyConversionModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<CurrencyConversionModel>> GetCurrencyConversionList(int currencyConversionId, int currencyId)
        {
            IList<CurrencyConversionModel> currencyConversionModelList = null;

            // create query.
            IQueryable<Currencyconversion> query = GetQueryByCondition(w => w.ConversionId != 0)
                                                        .Include(w => w.Currency)
                                                        .Include(w => w.Company).Include(w => w.PreparedByUser);

            // apply filters.
            if (0 != currencyConversionId)
                query = query.Where(w => w.ConversionId == currencyConversionId);

            if (0 != currencyId)
                query = query.Where(w => w.CurrencyId == currencyId);

            // get records by query.
            List<Currencyconversion> currencyConversionList = await query.ToListAsync();
            if (null != currencyConversionList && currencyConversionList.Count > 0)
            {
                currencyConversionModelList = new List<CurrencyConversionModel>();

                foreach (Currencyconversion currencyConversion in currencyConversionList)
                {
                    currencyConversionModelList.Add(await AssignValueToModel(currencyConversion));
                }
            }

            return currencyConversionModelList; // returns.
        }

        private async Task<CurrencyConversionModel> AssignValueToModel(Currencyconversion currencyConversion)
        {
            return await Task.Run(() =>
            {
                CurrencyConversionModel currencyConversionModel = new CurrencyConversionModel();

                currencyConversionModel.ConversionId = currencyConversion.ConversionId;
                currencyConversionModel.CompanyId = currencyConversion.CompanyId;
                currencyConversionModel.CurrencyId = currencyConversion.CurrencyId;
                currencyConversionModel.EffectiveDateTime = currencyConversion.EffectiveDateTime;
                currencyConversionModel.ExchangeRate = currencyConversion.ExchangeRate;
                
                currencyConversionModel.CompanyName = currencyConversion.Company.CompanyName;
                currencyConversionModel.CurrencyName = currencyConversion.Currency.CurrencyName;
                currencyConversionModel.PreparedByName = currencyConversion.PreparedByUser.UserName;

                return currencyConversionModel;
            });
        }
    }
}
