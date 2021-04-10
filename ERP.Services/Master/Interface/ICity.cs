using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Master.Interface
{
    public interface ICity : IRepository<City>
    {
        /// <summary>
        /// create new city.
        /// </summary>
        /// <param name="cityModel"></param>
        /// <returns>
        /// return inserted cityId.
        /// </returns>
        Task<int> CreateCity(CityModel cityModel);

        /// <summary>
        /// update city.
        /// </summary>
        /// <param name="cityModel"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        Task<bool> UpdateCity(CityModel cityModel);

        /// <summary>
        /// update city.
        /// </summary>
        /// <param name="cityModel"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        Task<bool> DeleteCity(int cityId);

        /// <summary>
        /// get city based on cityId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        Task<CityModel> GetCityById(int cityId);

        /// <summary>
        /// get city based on stateId
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        Task<IList<CityModel>> GetCityByStateId(int stateId);

        /// <summary>
        /// get all city list
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        Task<DataTableResultModel<CityModel>> GetCityList();
    }
}
