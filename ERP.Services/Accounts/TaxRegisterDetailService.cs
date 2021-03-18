using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Services.Accounts.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class TaxRegisterDetailService : Repository<Taxregisterdetail>, ITaxRegisterDetail
    {
        public TaxRegisterDetailService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> CreateTaxRegisterDetail(TaxRegisterDetailModel taxRegisterDetailModel)
        {
            int taxRegisterDetailId = 0;

            // assign values.
            Taxregisterdetail taxRegisterDetail = new Taxregisterdetail();

            //taxRegisterDetail.TaxRegisterDetailName = taxRegisterDetailModel.TaxRegisterDetailName;

            taxRegisterDetailId = await Create(taxRegisterDetail);

            return taxRegisterDetailId; // returns.
        }


        public async Task<bool> UpdateTaxRegisterDetail(TaxRegisterDetailModel taxRegisterDetailModel)
        {
            bool isUpdated = false;

            // get record.
            Taxregisterdetail taxRegisterDetail = await GetByIdAsync(w => w.TaxRegisterDetId == taxRegisterDetailModel.TaxRegisterDetId);

            if (null != taxRegisterDetail)
            {
                // assign values.
                //taxRegisterDetail.TaxRegisterDetailName = taxRegisterDetailModel.TaxRegisterDetailName;

                isUpdated = await Update(taxRegisterDetail);
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeleteTaxRegisterDetail(int taxRegisterDetailId)
        {
            bool isDeleted = false;

            // get record.
            Taxregisterdetail taxRegisterDetail = await GetByIdAsync(w => w.TaxRegisterDetId == taxRegisterDetailId);

            if (null != taxRegisterDetail)
            {
                isDeleted = await Delete(taxRegisterDetail);
            }

            return isDeleted; // returns.
        }


        public async Task<TaxRegisterDetailModel> GetTaxRegisterDetailById(int taxRegisterDetailId)
        {
            TaxRegisterDetailModel taxRegisterDetailModel = null;

            IList<TaxRegisterDetailModel> taxRegisterDetailModelList = await GetTaxRegisterDetailList(taxRegisterDetailId);

            if (null != taxRegisterDetailModelList && taxRegisterDetailModelList.Any())
            {
                taxRegisterDetailModel = taxRegisterDetailModelList.FirstOrDefault();
            }

            return taxRegisterDetailModel; // returns.
        }


        public async Task<IList<TaxRegisterDetailModel>> GetTaxRegisterDetailByStateId(int stateId)
        {
            return await GetTaxRegisterDetailList(0);
        }


        public async Task<DataTableResultModel<TaxRegisterDetailModel>> GetTaxRegisterDetailList()
        {
            DataTableResultModel<TaxRegisterDetailModel> resultModel = new DataTableResultModel<TaxRegisterDetailModel>();

            IList<TaxRegisterDetailModel> taxRegisterDetailModelList = await GetTaxRegisterDetailList(0);

            if (null != taxRegisterDetailModelList && taxRegisterDetailModelList.Any())
            {
                resultModel = new DataTableResultModel<TaxRegisterDetailModel>();
                resultModel.ResultList = taxRegisterDetailModelList;
                resultModel.TotalResultCount = taxRegisterDetailModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<TaxRegisterDetailModel>> GetTaxRegisterDetailList(int taxRegisterDetailId)
        {
            IList<TaxRegisterDetailModel> taxRegisterDetailModelList = null;

            // create query.
            IQueryable<Taxregisterdetail> query = GetQueryByCondition(w => w.TaxRegisterDetId != 0);

            // apply filters.
            if (0 != taxRegisterDetailId)
                query = query.Where(w => w.TaxRegisterDetId == taxRegisterDetailId);

          

            // get records by query.
            List<Taxregisterdetail> taxRegisterDetailList = await query.ToListAsync();
            if (null != taxRegisterDetailList && taxRegisterDetailList.Count > 0)
            {
                taxRegisterDetailModelList = new List<TaxRegisterDetailModel>();

                foreach (Taxregisterdetail taxRegisterDetail in taxRegisterDetailList)
                {
                    taxRegisterDetailModelList.Add(await AssignValueToModel(taxRegisterDetail));
                }
            }

            return taxRegisterDetailModelList; // returns.
        }

        private async Task<TaxRegisterDetailModel> AssignValueToModel(Taxregisterdetail taxRegisterDetail)
        {
            return await Task.Run(() =>
            {
                TaxRegisterDetailModel taxRegisterDetailModel = new TaxRegisterDetailModel();

                //taxRegisterDetailModel.TaxRegisterDetId = taxRegisterDetail.TaxRegisterDetId;
                //taxRegisterDetailModel.TaxRegisterDetailName = taxRegisterDetail.TaxRegisterDetailName;

                return taxRegisterDetailModel;
            });
        }
    }
}
