using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Master;
using ERP.Services.Master.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Master
{
    public class CountryService : Repository<Country>, ICountry
    {
        public CountryService(ErpDbContext dbContext) : base(dbContext) { }

        /// <summary>
        /// get all country list.
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<IList<CountryModel>> GetAllCountry()
        {
            IList<CountryModel> countryModelList = null;

            // get records by query.
            IList<Country> countryList = await GetListByConditionAsync(w => w.CountryId != 0);
            if (null != countryList && countryList.Count > 0)
            {
                countryModelList = new List<CountryModel>();
                foreach (Country country in countryList)
                {
                    CountryModel countryModel = new CountryModel();
                    countryModel.CountryId = country.CountryId;
                    countryModel.CountryName = country.CountryName;
                    countryModelList.Add(countryModel);
                }
            }

            return countryModelList; // returns.
        }
    }
}
