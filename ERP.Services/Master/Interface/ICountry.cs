using ERP.DataAccess.EntityModels;
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
        /// <summary>
        /// get all country list.
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        Task<IList<CountryModel>> GetAllCountry();
    }
}
