using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Master.Interface
{
    public interface I_ : IRepository<City>
    {
       
        Task<int> CreateCity(CityModel cityModel);

        Task<bool> UpdateCity(CityModel cityModel);

        Task<bool> DeleteCity(int cityId);
       
        Task<CityModel> GetCityById(int cityId);
        
        Task<IList<CityModel>> GetCityByStateId(int stateId);

        Task<DataTableResultModel<CityModel>> GetCityList();
    }
}
