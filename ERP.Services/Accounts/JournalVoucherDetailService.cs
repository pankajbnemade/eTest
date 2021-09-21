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
    public class JournalVoucherDetailService : Repository<Journalvoucherdetail>, IJournalVoucherDetail
    {
        IJournalVoucher journalVoucher;

        public JournalVoucherDetailService(ErpDbContext dbContext, IJournalVoucher _journalVoucher) : base(dbContext)
        {
            journalVoucher = _journalVoucher;
        }

        public async Task<int> CreateJournalVoucherDetail(JournalVoucherDetailModel journalVoucherDetailModel)
        {
            int journalVoucherDetailId = 0;

            // assign values.
            Journalvoucherdetail journalVoucherDetail = new Journalvoucherdetail();

            journalVoucherDetail.JournalVoucherId = journalVoucherDetailModel.JournalVoucherId;
            journalVoucherDetail.ParticularLedgerId = journalVoucherDetailModel.ParticularLedgerId;
            journalVoucherDetail.TransactionTypeId = journalVoucherDetailModel.TransactionTypeId;
            journalVoucherDetail.DebitAmountFc = journalVoucherDetailModel.DebitAmountFc;
            journalVoucherDetail.DebitAmount = 0;
            journalVoucherDetail.CreditAmountFc = journalVoucherDetailModel.CreditAmountFc;
            journalVoucherDetail.CreditAmount = 0;
            journalVoucherDetail.Narration = journalVoucherDetailModel.Narration;
            journalVoucherDetail.SalesInvoiceId = journalVoucherDetailModel.SalesInvoiceId;
            journalVoucherDetail.PurchaseInvoiceId = journalVoucherDetailModel.PurchaseInvoiceId;
            journalVoucherDetail.DebitNoteId = journalVoucherDetailModel.DebitNoteId;
            journalVoucherDetail.CreditNoteId = journalVoucherDetailModel.CreditNoteId;

            if (journalVoucherDetailId != 0)
            {
                await UpdateJournalVoucherDetailAmount(journalVoucherDetailId);
            }

            await Create(journalVoucherDetail);

            journalVoucherDetailId = journalVoucherDetail.JournalVoucherDetId;

            return journalVoucherDetailId; // returns.
        }

        public async Task<bool> UpdateJournalVoucherDetail(JournalVoucherDetailModel journalVoucherDetailModel)
        {
            bool isUpdated = false;

            // get record.
            Journalvoucherdetail journalVoucherDetail = await GetByIdAsync(w => w.JournalVoucherDetId == journalVoucherDetailModel.JournalVoucherDetId);

            if (null != journalVoucherDetail)
            {

                // assign values.
                journalVoucherDetail.ParticularLedgerId = journalVoucherDetailModel.ParticularLedgerId;
                journalVoucherDetail.TransactionTypeId = journalVoucherDetailModel.TransactionTypeId;
                journalVoucherDetail.DebitAmountFc = journalVoucherDetailModel.DebitAmountFc;
                journalVoucherDetail.DebitAmount = 0;
                journalVoucherDetail.CreditAmountFc = journalVoucherDetailModel.CreditAmountFc;
                journalVoucherDetail.CreditAmount = 0;
                journalVoucherDetail.Narration = journalVoucherDetailModel.Narration;
                journalVoucherDetail.SalesInvoiceId = journalVoucherDetailModel.SalesInvoiceId;
                journalVoucherDetail.PurchaseInvoiceId = journalVoucherDetailModel.PurchaseInvoiceId;
                journalVoucherDetail.DebitNoteId = journalVoucherDetailModel.DebitNoteId;
                journalVoucherDetail.CreditNoteId = journalVoucherDetailModel.CreditNoteId;

                isUpdated = await Update(journalVoucherDetail);
            }

            if (isUpdated != false)
            {
                await UpdateJournalVoucherDetailAmount(journalVoucherDetailModel.JournalVoucherDetId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateJournalVoucherDetailAmount(int? journalVoucherDetailId)
        {
            bool isUpdated = false;

            // get record.
            Journalvoucherdetail journalVoucherDetail = await GetQueryByCondition(w => w.JournalVoucherDetId == journalVoucherDetailId)
                                                                 .Include(w => w.JournalVoucher).FirstOrDefaultAsync();

            if (null != journalVoucherDetail)
            {
                journalVoucherDetail.CreditAmount = journalVoucherDetail.CreditAmountFc * journalVoucherDetail.JournalVoucher.ExchangeRate;
                journalVoucherDetail.DebitAmount = journalVoucherDetail.DebitAmountFc * journalVoucherDetail.JournalVoucher.ExchangeRate;

                isUpdated = await Update(journalVoucherDetail);
            }

            if (isUpdated != false)
            {
                await journalVoucher.UpdateJournalVoucherMasterAmount(journalVoucherDetail.JournalVoucherId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteJournalVoucherDetail(int journalVoucherDetailId)
        {
            bool isDeleted = false;

            // get record.
            Journalvoucherdetail journalVoucherDetail = await GetByIdAsync(w => w.JournalVoucherDetId == journalVoucherDetailId);

            if (null != journalVoucherDetail)
            {
                isDeleted = await Delete(journalVoucherDetail);
            }

            if (isDeleted != false)
            {
                await journalVoucher.UpdateJournalVoucherMasterAmount(journalVoucherDetail.JournalVoucherId);
            }

            return isDeleted; // returns.
        }

        public async Task<JournalVoucherDetailModel> GetJournalVoucherDetailById(int journalVoucherDetailId)
        {
            JournalVoucherDetailModel journalVoucherDetailModel = null;

            IList<JournalVoucherDetailModel> journalVoucherModelDetailList = await GetJournalVoucherDetailList(journalVoucherDetailId, 0);

            if (null != journalVoucherModelDetailList && journalVoucherModelDetailList.Any())
            {
                journalVoucherDetailModel = journalVoucherModelDetailList.FirstOrDefault();
            }

            return journalVoucherDetailModel; // returns.
        }

        public async Task<DataTableResultModel<JournalVoucherDetailModel>> GetJournalVoucherDetailByJournalVoucherId(int journalVoucherId)
        {
            DataTableResultModel<JournalVoucherDetailModel> resultModel = new DataTableResultModel<JournalVoucherDetailModel>();

            IList<JournalVoucherDetailModel> journalVoucherDetailModelList = await GetJournalVoucherDetailList(0, journalVoucherId);

            if (null != journalVoucherDetailModelList && journalVoucherDetailModelList.Any())
            {
                resultModel = new DataTableResultModel<JournalVoucherDetailModel>();
                resultModel.ResultList = journalVoucherDetailModelList;
                resultModel.TotalResultCount = journalVoucherDetailModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<JournalVoucherDetailModel>();
                resultModel.ResultList = new List<JournalVoucherDetailModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<JournalVoucherDetailModel>> GetJournalVoucherDetailList()
        {
            DataTableResultModel<JournalVoucherDetailModel> resultModel = new DataTableResultModel<JournalVoucherDetailModel>();

            IList<JournalVoucherDetailModel> journalVoucherDetailModelList = await GetJournalVoucherDetailList(0, 0);

            if (null != journalVoucherDetailModelList && journalVoucherDetailModelList.Any())
            {
                resultModel = new DataTableResultModel<JournalVoucherDetailModel>();
                resultModel.ResultList = journalVoucherDetailModelList;
                resultModel.TotalResultCount = journalVoucherDetailModelList.Count();
            }

            return resultModel; // returns.
        }

        /// <summary>
        /// get journal voucher List based on particularLedgerId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<IList<JournalVoucherDetailModel>> GetInvoiceListByParticularLedgerId(int particularLedgerId)
        {
            IList<JournalVoucherDetailModel> journalVoucherDetailModelList = null;

            // create query.
            IQueryable<Journalvoucherdetail> query = GetQueryByCondition(w => w.JournalVoucherDetId != 0);

            // apply filters.
            if (0 != particularLedgerId)
                query = query.Where(w => w.ParticularLedgerId == particularLedgerId);

            // get records by query.
            List<Journalvoucherdetail> journalVoucherDetailList = await query.ToListAsync();

             journalVoucherDetailModelList = new List<JournalVoucherDetailModel>();

            if (null != journalVoucherDetailList && journalVoucherDetailList.Count > 0)
            {
                foreach (Journalvoucherdetail journalVoucherDetail in journalVoucherDetailList)
                {
                    journalVoucherDetailModelList.Add(await AssignValueToModel(journalVoucherDetail));
                }
            }

            return journalVoucherDetailModelList; // returns.
        }

        private async Task<IList<JournalVoucherDetailModel>> GetJournalVoucherDetailList(int journalVoucherDetailId, int journalVoucherId)
        {
            IList<JournalVoucherDetailModel> journalVoucherDetailModelList = null;

            // create query.
            IQueryable<Journalvoucherdetail> query = GetQueryByCondition(w => w.JournalVoucherDetId != 0);

            // apply filters.
            if (0 != journalVoucherDetailId)
                query = query.Where(w => w.JournalVoucherDetId == journalVoucherDetailId);

            if (0 != journalVoucherId)
                query = query.Where(w => w.JournalVoucherId == journalVoucherId);

            // get records by query.
            List<Journalvoucherdetail> journalVoucherDetailList = await query.ToListAsync();

            if (null != journalVoucherDetailList && journalVoucherDetailList.Count > 0)
            {
                journalVoucherDetailModelList = new List<JournalVoucherDetailModel>();

                foreach (Journalvoucherdetail journalVoucherDetail in journalVoucherDetailList)
                {
                    journalVoucherDetailModelList.Add(await AssignValueToModel(journalVoucherDetail));
                }
            }

            return journalVoucherDetailModelList; // returns.
        }

        private async Task<JournalVoucherDetailModel> AssignValueToModel(Journalvoucherdetail journalVoucherDetail)
        {
            return await Task.Run(() =>
            {
                JournalVoucherDetailModel journalVoucherDetailModel = new JournalVoucherDetailModel();

                journalVoucherDetailModel.JournalVoucherDetId = journalVoucherDetail.JournalVoucherDetId;
                journalVoucherDetailModel.JournalVoucherId = journalVoucherDetail.JournalVoucherId;
                journalVoucherDetailModel.ParticularLedgerId = journalVoucherDetail.ParticularLedgerId;
                journalVoucherDetailModel.TransactionTypeId = journalVoucherDetail.TransactionTypeId;
                journalVoucherDetailModel.DebitAmountFc = journalVoucherDetail.DebitAmountFc;
                journalVoucherDetailModel.DebitAmount = journalVoucherDetail.DebitAmount;
                journalVoucherDetailModel.CreditAmountFc = journalVoucherDetail.CreditAmountFc;
                journalVoucherDetailModel.CreditAmount = journalVoucherDetail.CreditAmount;
                journalVoucherDetailModel.Narration = journalVoucherDetail.Narration;

                journalVoucherDetailModel.SalesInvoiceId = journalVoucherDetail.SalesInvoiceId;
                journalVoucherDetailModel.PurchaseInvoiceId = journalVoucherDetail.PurchaseInvoiceId;
                journalVoucherDetailModel.CreditNoteId = journalVoucherDetail.CreditNoteId;
                journalVoucherDetailModel.DebitNoteId = journalVoucherDetail.DebitNoteId;

                //--####
                //journalVoucherDetailModel.TransactionTypeName = null != journalVoucherDetail.UnitOfMeasurement ? journalVoucherDetail.UnitOfMeasurement.UnitOfMeasurementName : null;

                return journalVoucherDetailModel;
            });
        }

    }
}
