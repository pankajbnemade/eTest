using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Helpers;
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

        public async Task<int> CreateState(StateModel stateModel)
        {
            int stateId = 0;

            // assign values.
            State state = new State();
            state.StateName = stateModel.StateName;
            state.CountryId = stateModel.CountryId;

            stateId = await Create(state);

            return stateId; // returns.
        }

        public async Task<bool> UpdateState(StateModel stateModel)
        {
            bool isUpdated = false;

            // get record.
            State state = await GetByIdAsync(w => w.StateId == stateModel.StateId);
            if (null != state)
            {
                // assign values.
                state.StateName = stateModel.StateName;
                state.CountryId = stateModel.CountryId;

                isUpdated = await Update(state);
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeleteState(int stateId)
        {
            bool isDeleted = false;

            // get record.
            State state = await GetByIdAsync(w => w.StateId == stateId);
            if (null != state)
            {
                isDeleted = await Delete(state);
            }

            return isDeleted; // returns.
        }


        public async Task<StateModel> GetStateById(int stateId)
        {
            StateModel stateModel = null;

            IList<StateModel> stateModelList = await GetStateList(stateId, 0);
            if (null != stateModelList && stateModelList.Any())
            {
                stateModel = stateModelList.FirstOrDefault();
            }

            return stateModel; // returns.
        }

        public async Task<DataTableResultModel<StateModel>> GetStateList()
        {
            DataTableResultModel<StateModel> resultModel = new DataTableResultModel<StateModel>();

            IList<StateModel> stateModelList = await GetStateList(0, 0);
            if (null != stateModelList && stateModelList.Any())
            {
                resultModel = new DataTableResultModel<StateModel>();
                resultModel.ResultList = stateModelList;
                resultModel.TotalResultCount = stateModelList.Count();
            }

            return resultModel; // returns.
        }

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
                    stateModelList.Add(await AssignValueToModel(state));
                }
            }

            return stateModelList; // returns.
        }

        private async Task<StateModel> AssignValueToModel(State state)
        {
            return await Task.Run(() =>
            {
                StateModel stateModel = new StateModel();

                stateModel.StateId = state.StateId;
                stateModel.StateName = state.StateName;
                stateModel.CountryId = Convert.ToInt32(state.CountryId);
                stateModel.CountryName = state.Country.CountryName;

                stateModel.PreparedByName = state.PreparedByUser.UserName;

                return stateModel;
            });
        }

        public async Task<IList<SelectListModel>> GetStateSelectList()
        {
            IList<SelectListModel> resultModel = null;

            if (await Any(w => w.StateId != 0))
            {
                IQueryable<State> query = GetQueryByCondition(w => w.StateId != 0);

                resultModel = await query
                                    .Select(s => new SelectListModel
                                    {
                                        DisplayText = s.StateName,
                                        Value = s.StateId.ToString()
                                    })
                                    .ToListAsync();
            }

            return resultModel; // returns.
        }

    }
}
