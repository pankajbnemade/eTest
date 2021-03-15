using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Master;
using ERP.Services.Master.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Master
{
    public class StateService : Repository<State>, IState
    {
        public StateService(ErpDbContext dbContext) : base(dbContext) { }

        /// <summary>
        /// get state list based on countryId
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<IList<StateModel>> GetStateByCountryId(int countryId)
        {
            return await GetStateList(0, countryId);
        }

        private async Task<IList<StateModel>> GetStateList(int stateId, int countryId)
        {
            IList<StateModel> stateModelList = null;

            // create query.
            IQueryable<State> query = GetQueryByCondition(w => w.StateId != 0).Include(ct => ct.Country);

            // apply filters.
            if (0 != stateId)
                query = query.Where(w => w.StateId == stateId);

            if (0 != countryId)
                query = query.Where(w => w.CountryId == countryId);

            // get records by query.
            List<State> stateList = await query.ToListAsync();
            if (null != stateList && stateList.Count > 0)
            {
                stateModelList = new List<StateModel>();
                foreach (State state in stateList)
                {
                    StateModel stateModel = new StateModel();
                    stateModel.StateId = state.StateId;
                    stateModel.StateName = state.StateName;
                    stateModel.CountryId = Convert.ToInt32(state.CountryId);
                    stateModel.CountryName = state.Country.CountryName;
                    stateModelList.Add(stateModel);
                }
            }

            return stateModelList; // returns.
        }
    }
}
