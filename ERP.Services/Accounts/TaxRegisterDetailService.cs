using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Services.Accounts.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class TaxRegisterDetailService : Repository<Taxregisterdetail>, ITaxRegisterDetail
    {
        public TaxRegisterDetailService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> GenerateSrNo(int taxRegisterId)
        {
            int srNo = 0;

            if (await Any(w => w.TaxRegisterDetId != 0 && w.TaxRegisterId == taxRegisterId))
            {
                srNo = await GetQueryByCondition(w => w.TaxRegisterDetId != 0 && w.TaxRegisterId == taxRegisterId).MaxAsync(m => Convert.ToInt32(m.SrNo));
            }

            return (srNo + 1);
        }

        public async Task<int> CreateTaxRegisterDetail(TaxRegisterDetailModel taxRegisterDetailModel)
        {
            int taxRegisterDetailId = 0;

            // assign values.
            Taxregisterdetail taxRegisterDetail = new Taxregisterdetail();
            taxRegisterDetail.TaxRegisterId = taxRegisterDetailModel.TaxRegisterId;
            taxRegisterDetail.SrNo = taxRegisterDetailModel.SrNo;
            taxRegisterDetail.TaxLedgerId = taxRegisterDetailModel.TaxLedgerId;
            taxRegisterDetail.TaxPercentageOrAmount = taxRegisterDetailModel.TaxPercentageOrAmount;
            taxRegisterDetail.Rate = taxRegisterDetailModel.Rate;
            taxRegisterDetail.TaxAddOrDeduct = taxRegisterDetailModel.TaxAddOrDeduct;

            await Create(taxRegisterDetail);

            taxRegisterDetailId = taxRegisterDetail.TaxRegisterDetId;

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
                taxRegisterDetail.TaxRegisterId = taxRegisterDetailModel.TaxRegisterId;
                taxRegisterDetail.SrNo = taxRegisterDetailModel.SrNo;
                taxRegisterDetail.TaxLedgerId = taxRegisterDetailModel.TaxLedgerId;
                taxRegisterDetail.TaxPercentageOrAmount = taxRegisterDetailModel.TaxPercentageOrAmount;
                taxRegisterDetail.Rate = taxRegisterDetailModel.Rate;
                taxRegisterDetail.TaxAddOrDeduct = taxRegisterDetailModel.TaxAddOrDeduct;

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

            IList<TaxRegisterDetailModel> taxRegisterDetailModelList = await GetTaxRegisterDetailList(taxRegisterDetailId, 0);

            if (null != taxRegisterDetailModelList && taxRegisterDetailModelList.Any())
            {
                taxRegisterDetailModel = taxRegisterDetailModelList.FirstOrDefault();
            }

            return taxRegisterDetailModel; // returns.
        }

        public async Task<DataTableResultModel<TaxRegisterDetailModel>> GetTaxRegisterDetailByTaxRegisterId(int taxRegisterId)
        {
            DataTableResultModel<TaxRegisterDetailModel> resultModel = new DataTableResultModel<TaxRegisterDetailModel>();

            IList<TaxRegisterDetailModel> taxRegisterDetailModelList = await GetTaxRegisterDetailList(0, taxRegisterId);

            if (null != taxRegisterDetailModelList && taxRegisterDetailModelList.Any())
            {
                resultModel = new DataTableResultModel<TaxRegisterDetailModel>();
                resultModel.ResultList = taxRegisterDetailModelList;
                resultModel.TotalResultCount = taxRegisterDetailModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<TaxRegisterDetailModel>();
                resultModel.ResultList = new List<TaxRegisterDetailModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        public async Task<IList<TaxRegisterDetailModel>> GetTaxRegisterDetailListByTaxRegisterId(int taxRegisterId)
        {
            IList<TaxRegisterDetailModel> taxRegisterDetailModelList = await GetTaxRegisterDetailList(0, taxRegisterId);

            return taxRegisterDetailModelList; // returns.
        }

        //public async Task<DataTableResultModel<TaxRegisterDetailModel>> GetTaxRegisterDetailList()
        //{

        //    DataTableResultModel<TaxRegisterDetailModel> resultModel = new DataTableResultModel<TaxRegisterDetailModel>();

        //    try
        //    {
        //        IList<TaxRegisterDetailModel> taxRegisterDetailModelList = await GetTaxRegisterDetailList(0, 0);

        //        if (null != taxRegisterDetailModelList && taxRegisterDetailModelList.Any())
        //        {
        //            resultModel = new DataTableResultModel<TaxRegisterDetailModel>();
        //            resultModel.ResultList = taxRegisterDetailModelList;
        //            resultModel.TotalResultCount = taxRegisterDetailModelList.Count();
        //        }
        //        else
        //        {
        //            resultModel = new DataTableResultModel<TaxRegisterDetailModel>();
        //            resultModel.ResultList = new List<TaxRegisterDetailModel>();
        //            resultModel.TotalResultCount = 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.Message.ToString());
        //    }

        //    return resultModel; // returns.
        //}

        private async Task<IList<TaxRegisterDetailModel>> GetTaxRegisterDetailList(int taxRegisterDetailId, int taxRegisterId)
        {
            IList<TaxRegisterDetailModel> taxRegisterDetailModelList = null;

            // create query.
            IQueryable<Taxregisterdetail> query = GetQueryByCondition(w => w.TaxRegisterDetId != 0)
                                                    .Include(w => w.TaxLedger);

            // apply filters.
            if (0 != taxRegisterDetailId)
                query = query.Where(w => w.TaxRegisterDetId == taxRegisterDetailId);

            // apply filters.
            if (0 != taxRegisterId)
                query = query.Where(w => w.TaxRegisterId == taxRegisterId);

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
                taxRegisterDetailModel.TaxRegisterDetId = taxRegisterDetail.TaxRegisterDetId;
                taxRegisterDetailModel.TaxRegisterId = taxRegisterDetail.TaxRegisterId;
                taxRegisterDetailModel.SrNo = taxRegisterDetail.SrNo;
                taxRegisterDetailModel.TaxLedgerId = taxRegisterDetail.TaxLedgerId;
                taxRegisterDetailModel.TaxPercentageOrAmount = taxRegisterDetail.TaxPercentageOrAmount;
                taxRegisterDetailModel.Rate = taxRegisterDetail.Rate;
                taxRegisterDetailModel.TaxAddOrDeduct = taxRegisterDetail.TaxAddOrDeduct;
                taxRegisterDetailModel.TaxLedgerName = null != taxRegisterDetail.TaxLedger ? taxRegisterDetail.TaxLedger.LedgerName : null;

                return taxRegisterDetailModel;
            });
        }
    }
}
