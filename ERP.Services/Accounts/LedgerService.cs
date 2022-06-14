using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using ERP.Services.Common.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class LedgerService : Repository<Ledger>, ILedger
    {
        private readonly ICommon _common;
        private readonly ErpDbContext _dbContext;


        public LedgerService(ErpDbContext dbContext, ICommon common) : base(dbContext)
        {
            _common = common;
            _dbContext = dbContext;
        }

        public async Task<GenerateNoModel> GenerateLedgerCode()
        {
            int voucherSetupId = 1;

            int? maxNo = await GetQueryByCondition(w => w.LedgerId != 0).MaxAsync(m => (int?)m.MaxNo);

            maxNo = maxNo == null ? 0 : maxNo;

            GenerateNoModel generateNoModel = await _common.GenerateVoucherNo((int)maxNo, voucherSetupId, 0, 0);

            return generateNoModel; // returns.
        }

        public async Task<int> CreateLedger(LedgerModel ledgerModel)
        {
            int ledgerId = 0;

            // assign values.
            Ledger ledger = new Ledger();

            ledger.LedgerCode = ledgerModel.LedgerCode;
            ledger.LedgerName = ledgerModel.LedgerName;
            ledger.IsGroup = Convert.ToSByte(ledgerModel.IsGroup);
            ledger.IsMasterGroup = Convert.ToSByte(ledgerModel.IsMasterGroup);
            ledger.ParentGroupId = ledgerModel.ParentGroupId;
            ledger.IsDeActive = ledgerModel.IsDeActive;
            ledger.TaxRegisteredNo = ledgerModel.TaxRegisteredNo;
            ledger.Description = ledgerModel.Description;
            ledger.MaxNo = ledgerModel.MaxNo;

            await Create(ledger);

            ledgerId = ledger.LedgerId;


            IList<Company> companyList = _dbContext.Companies.ToList();

            Ledgercompanyrelation ledgerCompanyRelation;

            foreach (Company company in companyList)
            {
                ledgerCompanyRelation = new Ledgercompanyrelation()
                {
                    CompanyId = company.CompanyId,
                    LedgerId = ledgerId,
                };

                _dbContext.Ledgercompanyrelations.Add(ledgerCompanyRelation);
            }

            await _dbContext.SaveChangesAsync();

            return ledgerId; // returns.
        }

        public async Task<bool> UpdateLedger(LedgerModel ledgerModel)
        {
            bool isUpdated = false;

            // get record.
            Ledger ledger = await GetByIdAsync(w => w.LedgerId == ledgerModel.LedgerId);

            if (null != ledger)
            {
                // assign values.
                ledger.LedgerCode = ledgerModel.LedgerCode;
                ledger.LedgerName = ledgerModel.LedgerName;
                ledger.IsGroup = Convert.ToSByte(ledgerModel.IsGroup);

                ledger.ParentGroupId = ledgerModel.ParentGroupId;
                ledger.IsDeActive = ledgerModel.IsDeActive;
                ledger.TaxRegisteredNo = ledgerModel.TaxRegisteredNo;
                ledger.Description = ledgerModel.Description;

                isUpdated = await Update(ledger);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteLedger(int ledgerId)
        {
            bool isDeleted = false;

            // get record.
            Ledger ledger = await GetByIdAsync(w => w.LedgerId == ledgerId);
            if (null != ledger)
            {
                isDeleted = await Delete(ledger);
            }

            return isDeleted; // returns.
        }

        public async Task<LedgerModel> GetLedgerById(int ledgerId)
        {
            LedgerModel ledgerModel = null;

            IList<LedgerModel> ledgerModelList = await GetLedgerList(ledgerId, 0);

            if (null != ledgerModelList && ledgerModelList.Any())
            {
                ledgerModel = ledgerModelList.FirstOrDefault();
            }

            return ledgerModel; // returns.
        }

        public async Task<DataTableResultModel<LedgerModel>> GetLedgerListByParentGroupId(int parentGroupId)
        {
            DataTableResultModel<LedgerModel> resultModel = new DataTableResultModel<LedgerModel>();

            IList<LedgerModel> ledgerModelList = await GetLedgerList(0, parentGroupId);

            if (null != ledgerModelList && ledgerModelList.Any())
            {
                resultModel = new DataTableResultModel<LedgerModel>();
                resultModel.ResultList = ledgerModelList;
                resultModel.TotalResultCount = ledgerModelList.Count();
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<LedgerModel>> GetLedgerList()
        {
            DataTableResultModel<LedgerModel> resultModel = new DataTableResultModel<LedgerModel>();

            IList<LedgerModel> ledgerModelList = await GetLedgerList(0, 0);

            if (null != ledgerModelList && ledgerModelList.Any())
            {
                resultModel = new DataTableResultModel<LedgerModel>();
                resultModel.ResultList = ledgerModelList;
                resultModel.TotalResultCount = ledgerModelList.Count();
            }

            return resultModel; // returns.
        }

        public async Task<LedgerModel> GetClosingBalanceByAccountLedgerId(int ledgerId, DateTime voucherDate)
        {
            LedgerModel ledgerModel = null;

            // create query.
            if (await Any(w => w.LedgerId == ledgerId))
            {
                Ledger ledger =
                    await GetQueryByCondition(w => w.LedgerId == ledgerId)
                    .FirstOrDefaultAsync();

                ledgerModel = await AssignValueToModel(ledger);
            }

            return ledgerModel; // returns.
        }

        private async Task<IList<LedgerModel>> GetLedgerList(int ledgerId, int parentGroupId)
        {
            IList<LedgerModel> ledgerModelList = null;

            // create query.
            IQueryable<Ledger> query = GetQueryByCondition(w => w.LedgerId != 0)
                                            .Include(w => w.ParentGroup)
                                            .Include(w => w.PreparedByUser).ThenInclude(w => w.Employee)
                                            .Include(w => w.Ledgeraddresses)
                                            .Include(w => w.Ledgercompanyrelations).ThenInclude(w => w.Company);

            // apply filters.
            if (0 != ledgerId)
                query = query.Where(w => w.LedgerId == ledgerId);

            // apply filters.
            if (0 != parentGroupId)
                query = query.Where(w => w.ParentGroupId == parentGroupId);

            // get records by query.
            List<Ledger> ledgerList = await query.ToListAsync();

            if (null != ledgerList && ledgerList.Count > 0)
            {
                ledgerModelList = new List<LedgerModel>();

                foreach (Ledger ledger in ledgerList)
                {
                    ledgerModelList.Add(await AssignValueToModel(ledger));
                }
            }

            return ledgerModelList; // returns.
        }

        public async Task<DataTableResultModel<LedgerModel>> GetLedgerList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterLedgerModel searchFilterModel)
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
            DataTableResultModel<LedgerModel> resultModel = await GetDataFromDbase(searchFilterModel, searchBy, take, skip, sortBy, sortDir);

            return resultModel; // returns.
        }

        private async Task<DataTableResultModel<LedgerModel>> GetDataFromDbase(SearchFilterLedgerModel searchFilterModel, string searchBy, int take, int skip, string sortBy, string sortDir)
        {
            DataTableResultModel<LedgerModel> resultModel = new DataTableResultModel<LedgerModel>();

            IQueryable<Ledger> query = GetQueryByCondition(w => w.LedgerId != 0)
                                    .Where(w => w.IsMasterGroup==0)
                                    .Where(w => w.ParentGroupId!=null)
                                    .Where(w => w.IsDeActive==0)
                                    .Include(w => w.ParentGroup)
                                    .Include(w => w.PreparedByUser).ThenInclude(w => w.Employee);


            //sortBy
            if (string.IsNullOrEmpty(sortBy) || sortBy == "0")
            {
                sortBy = "LedgerName";
            }

            //sortDir
            if (string.IsNullOrEmpty(sortDir) || sortDir == "")
            {
                sortDir = "asc";
            }

            if (!string.IsNullOrEmpty(searchFilterModel.LedgerName))
            {
                query = query.Where(w => w.LedgerName.Contains(searchFilterModel.LedgerName));
            }

            if (null != searchFilterModel.ParentGroupId)
            {
                query = query.Where(w => w.ParentGroupId == searchFilterModel.ParentGroupId);
            }

            if (null != searchFilterModel.FromDate)
            {
                query = query.Where(w => w.PreparedDateTime >= searchFilterModel.FromDate);
            }

            if (null != searchFilterModel.ToDate)
            {
                query = query.Where(w => w.PreparedDateTime <= searchFilterModel.ToDate);
            }

            query = query.Where(w => w.IsGroup == Convert.ToSByte(searchFilterModel.IsGroup));

            // get total count.
            resultModel.TotalResultCount = await query.CountAsync();

            // datatable search
            if (!string.IsNullOrEmpty(searchBy))
            {
                query = query.Where(w => w.LedgerName.ToLower().Contains(searchBy.ToLower()));
            }

            // get records based on pagesize.
            query = query.Skip(skip).Take(take);

            resultModel.ResultList = await query.Select(s => new LedgerModel
            {
                LedgerId = s.LedgerId,
                LedgerCode = s.LedgerCode,
                LedgerName = s.LedgerName,
                ParentGroupName = s.ParentGroup.LedgerName,
                PreparedByName = s.PreparedByUser.Employee.FirstName + " " + s.PreparedByUser.Employee.LastName,
            }).OrderBy($"{sortBy} {sortDir}").ToListAsync();


            // get filter record count.
            resultModel.FilterResultCount = await query.CountAsync();

            return resultModel; // returns.
        }

        private async Task<LedgerModel> AssignValueToModel(Ledger ledger)
        {
            return await Task.Run(() =>
            {
                LedgerModel ledgerModel = new LedgerModel();

                ledgerModel.LedgerId = ledger.LedgerId;
                ledgerModel.LedgerCode = ledger.LedgerCode;
                ledgerModel.LedgerName = ledger.LedgerName;
                ledgerModel.IsGroup = Convert.ToBoolean(ledger.IsGroup);
                ledgerModel.IsMasterGroup = Convert.ToBoolean(ledger.IsMasterGroup);
                ledgerModel.ParentGroupId = ledger.ParentGroupId;
                ledgerModel.IsDeActive = ledger.IsDeActive;
                ledgerModel.TaxRegisteredNo = ledger.TaxRegisteredNo;
                ledgerModel.Description = ledger.Description;
                //######
                ledgerModel.ParentGroupName = null != ledger.ParentGroup ? ledger.ParentGroup.LedgerName : null;
                ledgerModel.PreparedByName = null != ledger.PreparedByUser ? ledger.PreparedByUser.Employee.FirstName + " " + ledger.PreparedByUser.Employee.LastName : null;
                //ledgerModel.ClosingBalance = 500;

                if (null != ledger.Ledgercompanyrelations)
                {
                    ledgerModel.CompanyList = string.Join(",", ledger.Ledgercompanyrelations
                                                    .Select(p => p.Company.CompanyName.ToString()));
                }

                return ledgerModel;
            });
        }

        public async Task<IList<SelectListModel>> GetGroupSelectList(int parentGroupId)
        {
            IList<SelectListModel> resultModel = null;

            if (await Any(w => w.LedgerId != 0))
            {
                IQueryable<Ledger> query = GetQueryByCondition(w => w.LedgerId != 0);

                query = query.Where(w => w.IsGroup == 1);
                query = query.Where(w => w.ParentGroupId != null);

                // apply filters.
                if (0 != parentGroupId)
                    query = query.Where(w => w.ParentGroupId == parentGroupId);

                resultModel = await query.Select(s => new SelectListModel
                {
                    DisplayText = s.LedgerName,
                    Value = s.LedgerId.ToString()
                }).OrderBy(w => w.DisplayText).ToListAsync();
            }

            return resultModel; // returns.
        }

        public async Task<IList<SelectListModel>> GetLedgerSelectList(int parentGroupId, int companyId, Boolean IsLegderOnly)
        {
            IList<SelectListModel> resultModel = null;

            if (await Any(w => w.LedgerId != 0))
            {
                IQueryable<Ledger> query = GetQueryByCondition(w => w.LedgerId != 0)
                                            .Include(w=>w.Ledgercompanyrelations);

                if (companyId != 0)
                {
                    query = query.Where(w => w.Ledgercompanyrelations.Any(c => c.CompanyId == companyId));
                }

                if (IsLegderOnly == true)
                {
                    query = query.Where(w => w.IsGroup == 0);
                }

                if (IsLegderOnly == true)
                {
                    query = query.Where(w => w.IsGroup == 0);
                }


                // apply filters.
                if (0 != parentGroupId)
                    query = query.Where(w => w.ParentGroupId == parentGroupId);

                resultModel = await query.Select(s => new SelectListModel
                {
                    DisplayText = s.LedgerName,
                    Value = s.LedgerId.ToString()
                }).OrderBy(w => w.DisplayText).ToListAsync();
            }

            return resultModel; // returns.
        }
    }
}
