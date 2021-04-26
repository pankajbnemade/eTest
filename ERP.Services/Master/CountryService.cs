using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using ERP.Services.Master.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Master
{
    public class CountryService : Repository<Country>, ICountry
    {
        public CountryService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> CreateCountry(CountryModel countryModel)
        {
            int countryId = 0;

            // assign values.
            Country country = new Country();
            country.CountryName = countryModel.CountryName;

            countryId = await Create(country);

            return countryId; // returns.
        }

        public async Task<bool> UpdateCountry(CountryModel countryModel)
        {
            bool isUpdated = false;

            // get record.
            Country country = await GetByIdAsync(w => w.CountryId == countryModel.CountryId);
            if (null != country)
            {
                // assign values.
                country.CountryName = countryModel.CountryName;

                isUpdated = await Update(country);
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeleteCountry(int countryId)
        {
            bool isDeleted = false;

            // get record.
            Country country = await GetByIdAsync(w => w.CountryId == countryId);
            if (null != country)
            {
                isDeleted = await Delete(country);
            }

            return isDeleted; // returns.
        }


        public async Task<CountryModel> GetCountryById(int countryId)
        {
            CountryModel countryModel = null;

            IList<CountryModel> countryModelList = await GetCountryList(countryId);
            if (null != countryModelList && countryModelList.Any())
            {
                countryModel = countryModelList.FirstOrDefault();
            }

            return countryModel; // returns.
        }

        public async Task<DataTableResultModel<CountryModel>> GetCountryList()
        {
            DataTableResultModel<CountryModel> resultModel = new DataTableResultModel<CountryModel>();

            IList<CountryModel> countryModelList = await GetCountryList(0);
            if (null != countryModelList && countryModelList.Any())
            {
                resultModel = new DataTableResultModel<CountryModel>();
                resultModel.ResultList = countryModelList;
                resultModel.TotalResultCount = countryModelList.Count();
            }

            return resultModel; // returns.
        }


        /// <summary>
        /// get all country list.
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<IList<CountryModel>> GetCountryList(int countryId)
        {
            IList<CountryModel> countryModelList = null;

            // get records by query.

            IQueryable<Country> query = GetQueryByCondition(w => w.CountryId != 0).Include(s => s.PreparedByUser);

            if (0 != countryId)
                query = query.Where(w => w.CountryId == countryId);

            IList<Country> countryList = await query.ToListAsync();

            if (null != countryList && countryList.Count > 0)
            {
                countryModelList = new List<CountryModel>();
                foreach (Country country in countryList)
                {
                    countryModelList.Add(await AssignValueToModel(country));
                }
            }

            return countryModelList; // returns.
        }

        private async Task<CountryModel> AssignValueToModel(Country country)
        {
            return await Task.Run(() =>
            {
                CountryModel countryModel = new CountryModel();

                countryModel.CountryId = country.CountryId;
                countryModel.CountryName = country.CountryName;

                if (null != country.PreparedByUser)
                {
                    countryModel.PreparedByName = country.PreparedByUser.UserName;
                }

                return countryModel;
            });
        }

        public async Task<IList<SelectListModel>> GetCountrySelectList()
        {
            IList<SelectListModel> resultModel = null;

            if (await Any(w => w.CountryId != 0))
            {
                IQueryable<Country> query = GetQueryByCondition(w => w.CountryId != 0);

                resultModel = await query
                                    .Select(s => new SelectListModel
                                    {
                                        DisplayText = s.CountryName,
                                        Value = s.CountryId.ToString()
                                    })
                                    .ToListAsync();
            }

            return resultModel; // returns.
        }

    }
}
