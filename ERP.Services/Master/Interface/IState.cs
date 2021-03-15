using ERP.DataAccess.EntityModels;
using ERP.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Master.Interface
{
    public interface IState : IRepository<State>
    {
        /// <summary>
        /// get state list based on countryId
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        Task<IList<StateModel>> GetStateByCountryId(int countryId);
    }
}
