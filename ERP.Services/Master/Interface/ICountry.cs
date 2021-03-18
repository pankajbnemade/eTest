using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Master.Interface
{
    public interface ICountry : IRepository<Country>
    {

        Task<int> CreateCountry(CountryModel countryModel);
        Task<bool> UpdateCountry(CountryModel countryModel);
        Task<bool> DeleteCountry(int countryId);
        Task<CountryModel> GetCountryById(int countryId);
        Task<DataTableResultModel<CountryModel>> GetCountryList();
    }
}
