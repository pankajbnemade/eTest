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

        /// <summary>
        /// get all country list.
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        Task<DataTableResultModel<CountryModel>> GetCountryList();
    }
}
