using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Master;
using ERP.Services.Master.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Master
{
    public class _Service : Repository<City>, I_
    {
        public _Service(ErpDbContext dbContext) : base(dbContext) { }

        
        public async Task<int> CreateCity(CityModel cityModel)
        {
            int cityId = 0;

            // assign values.
            City city = new City();
            city.CityName = cityModel.CityName;
            city.StateId = cityModel.StateId;
            cityId = await Create(city);

            return cityId; // returns.
        }

        
        public async Task<bool> UpdateCity(CityModel cityModel)
        {
            bool isUpdated = false;

            // get record.
            City city = await GetByIdAsync(w => w.CityId == cityModel.CityId);
            if (null != city)
            {
                // assign values.
                city.CityName = cityModel.CityName;
                city.StateId = cityModel.StateId;
                isUpdated = await Update(city);
            }

            return isUpdated; // returns.
        }

     
        public async Task<bool> DeleteCity(int cityId)
        {
            bool isDeleted = false;

            // get record.
            City city = await GetByIdAsync(w => w.CityId == cityId);
            if (null != city)
            {
                isDeleted = await Delete(city);
            }

            return isDeleted; // returns.
        }

      
        public async Task<CityModel> GetCityById(int cityId)
        {
            CityModel cityModel = null;

            IList<CityModel> cityModelList = await GetCityList(cityId, 0, 0);
            if (null != cityModelList && cityModelList.Any())
            {
                cityModel = cityModelList.FirstOrDefault();
            }

            return cityModel; // returns.
        }

       
        public async Task<IList<CityModel>> GetCityByStateId(int stateId)
        {
            return await GetCityList(0, stateId, 0);
        }

        
        public async Task<DataTableResultModel<CityModel>> GetCityList()
        {
            DataTableResultModel<CityModel> resultModel = new DataTableResultModel<CityModel>();

            IList<CityModel> cityModelList = await GetCityList(0, 0, 0);
            if (null != cityModelList && cityModelList.Any())
            {
                resultModel = new DataTableResultModel<CityModel>();
                resultModel.ResultList = cityModelList;
                resultModel.TotalResultCount = cityModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<CityModel>> GetCityList(int cityId, int stateId, int countryId)
        {
            IList<CityModel> cityModelList = null;

            // create query.
            IQueryable<City> query = GetQueryByCondition(w => w.CityId != 0).Include(s => s.State).ThenInclude(ct => ct.Country);

            // apply filters.
            if (0 != cityId)
                query = query.Where(w => w.CityId == cityId);

            if (0 != stateId)
                query = query.Where(w => w.StateId == stateId);

            if (0 != countryId)
                query = query.Where(w => w.State.CountryId == countryId);

            // get records by query.
            List<City> cityList = await query.ToListAsync();
            if (null != cityList && cityList.Count > 0)
            {
                cityModelList = new List<CityModel>();
                foreach (City city in cityList)
                {
                    cityModelList.Add(await AssignValueToModel(city));
                }
            }

            return cityModelList; // returns.
        }

        private async Task<CityModel> AssignValueToModel(City city)
        {
            return await Task.Run(() =>
            {
                CityModel cityModel = new CityModel();

                cityModel.CityId = city.CityId;
                cityModel.CityName = city.CityName;
                cityModel.StateId = city.State.StateId;
                cityModel.StateName = city.State.StateName;
                cityModel.CountryId = city.State.Country.CountryId;
                cityModel.CountryName = city.State.Country.CountryName;

                return cityModel;
            });
        }
    }
}
