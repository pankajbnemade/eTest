using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Master.Interface
{
    public interface IState : IRepository<State>
    {

        Task<int> CreateState(StateModel stateModel);
        Task<bool> UpdateState(StateModel stateModel);
        Task<bool> DeleteState(int stateId);
        Task<StateModel> GetStateById(int stateId);

        /// <summary>
        /// get state list based on countryId
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        Task<IList<StateModel>> GetStateByCountryId(int countryId);

        Task<DataTableResultModel<StateModel>> GetStateList();

        Task<IList<SelectListModel>> GetStateSelectListByCountryId(int countryId);

        Task<IList<SelectListModel>> GetStateSelectList();

    }
}
