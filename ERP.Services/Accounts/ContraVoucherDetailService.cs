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
    public class ContraVoucherDetailService : Repository<Contravoucherdetail>, IContraVoucherDetail
    {
        IContraVoucher contraVoucher;

        public ContraVoucherDetailService(ErpDbContext dbContext, IContraVoucher _contraVoucher) : base(dbContext)
        {
            contraVoucher = _contraVoucher;
        }

        public async Task<int> CreateContraVoucherDetail(ContraVoucherDetailModel contraVoucherDetailModel)
        {
            int contraVoucherDetailId = 0;

            // assign values.
            Contravoucherdetail contraVoucherDetail = new Contravoucherdetail();

            contraVoucherDetail.ContraVoucherId = contraVoucherDetailModel.ContraVoucherId;
            contraVoucherDetail.ParticularLedgerId = contraVoucherDetailModel.ParticularLedgerId;
            contraVoucherDetail.DebitAmountFc = contraVoucherDetailModel.DebitAmountFc;
            contraVoucherDetail.DebitAmount = 0;
            contraVoucherDetail.CreditAmountFc = contraVoucherDetailModel.CreditAmountFc;
            contraVoucherDetail.CreditAmount = 0;
            contraVoucherDetail.Narration = contraVoucherDetailModel.Narration;

            if (contraVoucherDetailId != 0)
            {
                await UpdateContraVoucherDetailAmount(contraVoucherDetailId);
            }

            await Create(contraVoucherDetail);

            contraVoucherDetailId = contraVoucherDetail.ContraVoucherDetId;

            return contraVoucherDetailId; // returns.
        }

        public async Task<bool> UpdateContraVoucherDetail(ContraVoucherDetailModel contraVoucherDetailModel)
        {
            bool isUpdated = false;

            // get record.
            Contravoucherdetail contraVoucherDetail = await GetByIdAsync(w => w.ContraVoucherDetId == contraVoucherDetailModel.ContraVoucherDetId);

            if (null != contraVoucherDetail)
            {

                // assign values.
                contraVoucherDetail.ParticularLedgerId = contraVoucherDetailModel.ParticularLedgerId;
                contraVoucherDetail.DebitAmountFc = contraVoucherDetailModel.DebitAmountFc;
                contraVoucherDetail.DebitAmount = 0;
                contraVoucherDetail.CreditAmountFc = contraVoucherDetailModel.CreditAmountFc;
                contraVoucherDetail.CreditAmount = 0;
                contraVoucherDetail.Narration = contraVoucherDetailModel.Narration;

                isUpdated = await Update(contraVoucherDetail);
            }

            if (isUpdated != false)
            {
                await UpdateContraVoucherDetailAmount(contraVoucherDetailModel.ContraVoucherDetId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateContraVoucherDetailAmount(int? contraVoucherDetailId)
        {
            bool isUpdated = false;

            // get record.
            Contravoucherdetail contraVoucherDetail = await GetQueryByCondition(w => w.ContraVoucherDetId == contraVoucherDetailId)
                                                                 .Include(w => w.ContraVoucher).FirstOrDefaultAsync();

            if (null != contraVoucherDetail)
            {
                contraVoucherDetail.CreditAmount = contraVoucherDetail.CreditAmountFc * contraVoucherDetail.ContraVoucher.ExchangeRate;
                contraVoucherDetail.DebitAmount = contraVoucherDetail.DebitAmountFc * contraVoucherDetail.ContraVoucher.ExchangeRate;

                isUpdated = await Update(contraVoucherDetail);
            }

            if (isUpdated != false)
            {
                await contraVoucher.UpdateContraVoucherMasterAmount(contraVoucherDetail.ContraVoucherId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteContraVoucherDetail(int contraVoucherDetailId)
        {
            bool isDeleted = false;

            // get record.
            Contravoucherdetail contraVoucherDetail = await GetByIdAsync(w => w.ContraVoucherDetId == contraVoucherDetailId);

            if (null != contraVoucherDetail)
            {
                isDeleted = await Delete(contraVoucherDetail);
            }

            if (isDeleted != false)
            {
                await contraVoucher.UpdateContraVoucherMasterAmount(contraVoucherDetail.ContraVoucherId);
            }

            return isDeleted; // returns.
        }

        public async Task<ContraVoucherDetailModel> GetContraVoucherDetailById(int contraVoucherDetailId)
        {
            ContraVoucherDetailModel contraVoucherDetailModel = null;

            IList<ContraVoucherDetailModel> contraVoucherModelDetailList = await GetContraVoucherDetailList(contraVoucherDetailId, 0);

            if (null != contraVoucherModelDetailList && contraVoucherModelDetailList.Any())
            {
                contraVoucherDetailModel = contraVoucherModelDetailList.FirstOrDefault();
            }

            return contraVoucherDetailModel; // returns.
        }

        public async Task<DataTableResultModel<ContraVoucherDetailModel>> GetContraVoucherDetailByContraVoucherId(int contraVoucherId)
        {
            DataTableResultModel<ContraVoucherDetailModel> resultModel = new DataTableResultModel<ContraVoucherDetailModel>();

            IList<ContraVoucherDetailModel> contraVoucherDetailModelList = await GetContraVoucherDetailList(0, contraVoucherId);

            if (null != contraVoucherDetailModelList && contraVoucherDetailModelList.Any())
            {
                resultModel = new DataTableResultModel<ContraVoucherDetailModel>();
                resultModel.ResultList = contraVoucherDetailModelList;
                resultModel.TotalResultCount = contraVoucherDetailModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<ContraVoucherDetailModel>();
                resultModel.ResultList = new List<ContraVoucherDetailModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<ContraVoucherDetailModel>> GetContraVoucherDetailList()
        {
            DataTableResultModel<ContraVoucherDetailModel> resultModel = new DataTableResultModel<ContraVoucherDetailModel>();

            IList<ContraVoucherDetailModel> contraVoucherDetailModelList = await GetContraVoucherDetailList(0, 0);

            if (null != contraVoucherDetailModelList && contraVoucherDetailModelList.Any())
            {
                resultModel = new DataTableResultModel<ContraVoucherDetailModel>();
                resultModel.ResultList = contraVoucherDetailModelList;
                resultModel.TotalResultCount = contraVoucherDetailModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<ContraVoucherDetailModel>> GetContraVoucherDetailList(int contraVoucherDetailId, int contraVoucherId)
        {
            IList<ContraVoucherDetailModel> contraVoucherDetailModelList = null;

            // create query.
            IQueryable<Contravoucherdetail> query = GetQueryByCondition(w => w.ContraVoucherDetId != 0);

            // apply filters.
            if (0 != contraVoucherDetailId)
                query = query.Where(w => w.ContraVoucherDetId == contraVoucherDetailId);

            if (0 != contraVoucherId)
                query = query.Where(w => w.ContraVoucherId == contraVoucherId);

            // get records by query.
            List<Contravoucherdetail> contraVoucherDetailList = await query.ToListAsync();

            if (null != contraVoucherDetailList && contraVoucherDetailList.Count > 0)
            {
                contraVoucherDetailModelList = new List<ContraVoucherDetailModel>();

                foreach (Contravoucherdetail contraVoucherDetail in contraVoucherDetailList)
                {
                    contraVoucherDetailModelList.Add(await AssignValueToModel(contraVoucherDetail));
                }
            }

            return contraVoucherDetailModelList; // returns.
        }

        private async Task<ContraVoucherDetailModel> AssignValueToModel(Contravoucherdetail contraVoucherDetail)
        {
            return await Task.Run(() =>
            {
                ContraVoucherDetailModel contraVoucherDetailModel = new ContraVoucherDetailModel();

                contraVoucherDetailModel.ContraVoucherDetId = contraVoucherDetail.ContraVoucherDetId;
                contraVoucherDetailModel.ContraVoucherId = contraVoucherDetail.ContraVoucherId;
                contraVoucherDetailModel.ParticularLedgerId = contraVoucherDetail.ParticularLedgerId;
                contraVoucherDetailModel.DebitAmountFc = contraVoucherDetail.DebitAmountFc;
                contraVoucherDetailModel.DebitAmount = contraVoucherDetail.DebitAmount;
                contraVoucherDetailModel.CreditAmountFc = contraVoucherDetail.CreditAmountFc;
                contraVoucherDetailModel.CreditAmount = contraVoucherDetail.CreditAmount;
                contraVoucherDetailModel.Narration = contraVoucherDetail.Narration;

                //--####
                //contraVoucherDetailModel.TransactionTypeName = null != contraVoucherDetail.UnitOfMeasurement ? contraVoucherDetail.UnitOfMeasurement.UnitOfMeasurementName : null;

                return contraVoucherDetailModel;
            });
        }

    }
}
