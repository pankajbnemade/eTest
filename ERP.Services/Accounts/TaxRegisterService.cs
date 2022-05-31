using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using ERP.Services.Common.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class TaxRegisterService : Repository<Taxregister>, ITaxRegister
    {
        public TaxRegisterService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> CreateTaxRegister(TaxRegisterModel taxRegisterModel)
        {
            int taxRegisterId = 0;

            // assign values.
            Taxregister taxRegister = new Taxregister();

            taxRegister.TaxRegisterName = taxRegisterModel.TaxRegisterName;

            await Create(taxRegister);

            taxRegisterId = taxRegister.TaxRegisterId;

            return taxRegisterId; // returns.
        }

        public async Task<bool> UpdateTaxRegister(TaxRegisterModel taxRegisterModel)
        {
            bool isUpdated = false;

            // get record.
            Taxregister taxRegister = await GetByIdAsync(w => w.TaxRegisterId == taxRegisterModel.TaxRegisterId);

            if (null != taxRegister)
            {
                // assign values.
                taxRegister.TaxRegisterName = taxRegisterModel.TaxRegisterName;

                isUpdated = await Update(taxRegister);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteTaxRegister(int taxRegisterId)
        {
            bool isDeleted = false;

            // get record.
            Taxregister taxRegister = await GetByIdAsync(w => w.TaxRegisterId == taxRegisterId);

            if (null != taxRegister)
            {
                isDeleted = await Delete(taxRegister);
            }

            return isDeleted; // returns.
        }

        public async Task<TaxRegisterModel> GetTaxRegisterById(int taxRegisterId)
        {
            TaxRegisterModel taxRegisterModel = null;

            IList<TaxRegisterModel> taxRegisterModelList = await GetTaxRegisterList(taxRegisterId);

            if (null != taxRegisterModelList && taxRegisterModelList.Any())
            {
                taxRegisterModel = taxRegisterModelList.FirstOrDefault();
            }

            return taxRegisterModel; // returns.
        }

        public async Task<DataTableResultModel<TaxRegisterModel>> GetTaxRegisterList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterTaxRegisterModel searchFilterModel)
        {
            string searchBy = dataTableAjaxPostModel.search?.value;
            int take = dataTableAjaxPostModel.length;
            int skip = dataTableAjaxPostModel.start;

            string sortBy = string.Empty;
            string sortDir = string.Empty;

            if (dataTableAjaxPostModel.order != null)
            {
                sortBy = dataTableAjaxPostModel.columns[dataTableAjaxPostModel.order[0].column].data;
                sortDir = dataTableAjaxPostModel.order[0].dir.ToLower();
            }

            // search the dbase taking into consideration table sorting and paging
            DataTableResultModel<TaxRegisterModel> resultModel = await GetDataFromDbase(searchFilterModel, searchBy, take, skip, sortBy, sortDir);

            return resultModel; // returns.
        }

        private async Task<DataTableResultModel<TaxRegisterModel>> GetDataFromDbase(SearchFilterTaxRegisterModel searchFilterModel, string searchBy, int take, int skip, string sortBy, string sortDir)
        {
            DataTableResultModel<TaxRegisterModel> resultModel = new DataTableResultModel<TaxRegisterModel>();

            IQueryable<Taxregister> query = GetQueryByCondition(w => w.TaxRegisterId != 0)
                                             .Include(w => w.PreparedByUser);

            //sortBy
            if (string.IsNullOrEmpty(sortBy) || sortBy == "0")
            {
                sortBy = "TaxRegisterName";
            }

            //sortDir
            if (string.IsNullOrEmpty(sortDir) || sortDir == "")
            {
                sortDir = "asc";
            }

            if (!string.IsNullOrEmpty(searchFilterModel.TaxRegisterName))
            {
                query = query.Where(w => w.TaxRegisterName.Contains(searchFilterModel.TaxRegisterName));
            }

            // get total count.
            resultModel.TotalResultCount = await query.CountAsync();

            // datatable search
            if (!string.IsNullOrEmpty(searchBy))
            {
                query = query.Where(w => w.TaxRegisterName.ToLower().Contains(searchBy.ToLower()));
            }

            // get records based on pagesize.
            query = query.Skip(skip).Take(take);

            resultModel.ResultList = await query.Select(s => new TaxRegisterModel
            {
                TaxRegisterId = s.TaxRegisterId,
                TaxRegisterName = s.TaxRegisterName,
                PreparedByName = s.PreparedByUser.UserName,
            }).OrderBy($"{sortBy} {sortDir}").ToListAsync();

            // get filter record count.
            resultModel.FilterResultCount = await query.CountAsync();

            return resultModel; // returns.
        }

        private async Task<IList<TaxRegisterModel>> GetTaxRegisterList(int taxRegisterId)
        {
            IList<TaxRegisterModel> taxRegisterModelList = null;

            // create query.
            IQueryable<Taxregister> query = GetQueryByCondition(w => w.TaxRegisterId != 0);

            // apply filters.
            if (0 != taxRegisterId)
                query = query.Where(w => w.TaxRegisterId == taxRegisterId);

            // get records by query.
            List<Taxregister> taxRegisterList = await query.ToListAsync();

            if (null != taxRegisterList && taxRegisterList.Count > 0)
            {
                taxRegisterModelList = new List<TaxRegisterModel>();
                foreach (Taxregister taxRegister in taxRegisterList)
                {
                    taxRegisterModelList.Add(await AssignValueToModel(taxRegister));
                }
            }

            return taxRegisterModelList; // returns.
        }

        private async Task<TaxRegisterModel> AssignValueToModel(Taxregister taxRegister)
        {
            return await Task.Run(() =>
            {
                TaxRegisterModel taxRegisterModel = new TaxRegisterModel();
                taxRegisterModel.TaxRegisterId = taxRegister.TaxRegisterId;
                taxRegisterModel.TaxRegisterName = taxRegister.TaxRegisterName;
                taxRegisterModel.PreparedByName = null != taxRegister.PreparedByUser ? taxRegister.PreparedByUser.UserName : null;

                return taxRegisterModel;
            });
        }

        public async Task<IList<SelectListModel>> GetTaxRegisterSelectList()
        {
            IList<SelectListModel> resultModel = null;

            if (await Any(w => w.TaxRegisterId != 0))
            {
                IQueryable<Taxregister> query = GetQueryByCondition(w => w.TaxRegisterId != 0);
                resultModel = await query.Select(s => new SelectListModel
                {
                    DisplayText = s.TaxRegisterName,
                    Value = s.TaxRegisterId.ToString()
                }).OrderBy(w => w.DisplayText).ToListAsync();
            }

            return resultModel; // returns.
        }
    }
}
