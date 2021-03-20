using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class CurrencyService : Repository<Currency>, ICurrency
    {
        public CurrencyService(ErpDbContext dbContext) : base(dbContext) { }


        public async Task<int> CreateCurrency(CurrencyModel currencyModel)
        {
            int currencyId = 0;

            // assign values.
            Currency currency = new Currency();

            currency.CurrencyCode = currencyModel.CurrencyCode;
            currency.CurrencyName = currencyModel.CurrencyName;
            currency.Denomination = currencyModel.Denomination;

            currencyId = await Create(currency);

            return currencyId; // returns.
        }


        public async Task<bool> UpdateCurrency(CurrencyModel currencyModel)
        {
            bool isUpdated = false;

            // get record.
            Currency currency = await GetByIdAsync(w => w.CurrencyId == currencyModel.CurrencyId);

            if (null != currency)
            {
                // assign values.
                currency.CurrencyName = currencyModel.CurrencyName;
                currency.Denomination = currencyModel.Denomination;

                isUpdated = await Update(currency);
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeleteCurrency(int currencyId)
        {
            bool isDeleted = false;

            // get record.
            Currency currency = await GetByIdAsync(w => w.CurrencyId == currencyId);

            if (null != currency)
            {
                isDeleted = await Delete(currency);
            }

            return isDeleted; // returns.
        }


        public async Task<CurrencyModel> GetCurrencyById(int currencyId)
        {
            CurrencyModel currencyModel = null;

            IList<CurrencyModel> currencyModelList = await GetCurrencyList(currencyId);

            if (null != currencyModelList && currencyModelList.Any())
            {
                currencyModel = currencyModelList.FirstOrDefault();
            }

            return currencyModel; // returns.
        }

        public async Task<DataTableResultModel<CurrencyModel>> GetCurrencyList()
        {
            DataTableResultModel<CurrencyModel> resultModel = new DataTableResultModel<CurrencyModel>();

            IList<CurrencyModel> currencyModelList = await GetCurrencyList(0);

            if (null != currencyModelList && currencyModelList.Any())
            {
                resultModel = new DataTableResultModel<CurrencyModel>();
                resultModel.ResultList = currencyModelList;
                resultModel.TotalResultCount = currencyModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<CurrencyModel>> GetCurrencyList(int currencyId)
        {
            IList<CurrencyModel> currencyModelList = null;

            // create query.
            IQueryable<Currency> query = GetQueryByCondition(w => w.CurrencyId != 0);

            // apply filters.
            if (0 != currencyId)
                query = query.Where(w => w.CurrencyId == currencyId);

            // get records by query.
            List<Currency> currencyList = await query.ToListAsync();
            if (null != currencyList && currencyList.Count > 0)
            {
                currencyModelList = new List<CurrencyModel>();

                foreach (Currency currency in currencyList)
                {
                    currencyModelList.Add(await AssignValueToModel(currency));
                }
            }

            return currencyModelList; // returns.
        }

        private async Task<CurrencyModel> AssignValueToModel(Currency currency)
        {
            return await Task.Run(() =>
            {
                CurrencyModel currencyModel = new CurrencyModel();

                currencyModel.CurrencyId = currency.CurrencyId;
                currencyModel.CurrencyCode = currency.CurrencyCode;
                currencyModel.CurrencyName = currency.CurrencyName;
                currencyModel.Denomination = currency.Denomination;

                return currencyModel;
            });
        }


        public async Task<IList<SelectListModel>> GetCurrencySelectList()
        {
            IList<SelectListModel> resultModel = null;

            if (await Any(w => w.CurrencyId != 0))
            {
                IQueryable<Currency> query = GetQueryByCondition(w => w.CurrencyId != 0);

                resultModel = await query
                                    .Select(s => new SelectListModel
                                    {
                                        DisplayText = s.CurrencyName,
                                        Value = s.CurrencyId.ToString()
                                    })
                                    .ToListAsync();
            }

            return resultModel; // returns.
        }
    }
}
