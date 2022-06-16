using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Models.Helpers;
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
        private readonly IJournalVoucher _journalVoucher;
        private readonly ILedger _ledger;

        public JournalVoucherDetailService(ErpDbContext dbContext, IJournalVoucher journalVoucher, ILedger ledger) : base(dbContext)
        {
            _journalVoucher = journalVoucher;
            _ledger = ledger;
        }

        public async Task<int> CreateJournalVoucherDetail(JournalVoucherDetailModel journalVoucherDetailModel)
        {
            int journalVoucherDetailId = 0;

            Journalvoucherdetail journalVoucherDetail = new Journalvoucherdetail();

            journalVoucherDetail.JournalVoucherId = journalVoucherDetailModel.JournalVoucherId;
            journalVoucherDetail.ParticularLedgerId = journalVoucherDetailModel.ParticularLedgerId;
            journalVoucherDetail.TransactionTypeId = journalVoucherDetailModel.TransactionTypeId;
            journalVoucherDetail.CreditAmountFc = journalVoucherDetailModel.CreditAmountFc;
            journalVoucherDetail.CreditAmount = 0;
            journalVoucherDetail.DebitAmountFc = journalVoucherDetailModel.DebitAmountFc;
            journalVoucherDetail.DebitAmount = 0;

            journalVoucherDetail.Narration = journalVoucherDetailModel.Narration;
            journalVoucherDetail.SalesInvoiceId = journalVoucherDetailModel.SalesInvoiceId;
            journalVoucherDetail.CreditNoteId = journalVoucherDetailModel.CreditNoteId;
            journalVoucherDetail.PurchaseInvoiceId = journalVoucherDetailModel.PurchaseInvoiceId;
            journalVoucherDetail.DebitNoteId = journalVoucherDetailModel.DebitNoteId;

            await Create(journalVoucherDetail);

            journalVoucherDetailId = journalVoucherDetail.JournalVoucherDetId;

            if (journalVoucherDetailId != 0)
            {
                await UpdateJournalVoucherDetailAmount(journalVoucherDetailId);
            }

            return journalVoucherDetailId; // returns.
        }

        public async Task<bool> UpdateJournalVoucherDetail(JournalVoucherDetailModel journalVoucherDetailModel)
        {
            bool isUpdated = false;

            // get record.
            Journalvoucherdetail journalVoucherDetail = await GetByIdAsync(w => w.JournalVoucherDetId == journalVoucherDetailModel.JournalVoucherDetId);

            if (null != journalVoucherDetail)
            {
                journalVoucherDetail.ParticularLedgerId = journalVoucherDetailModel.ParticularLedgerId;
                journalVoucherDetail.TransactionTypeId = journalVoucherDetailModel.TransactionTypeId;
                journalVoucherDetail.CreditAmountFc = journalVoucherDetailModel.CreditAmountFc;
                journalVoucherDetail.CreditAmount = 0;
                journalVoucherDetail.DebitAmountFc = journalVoucherDetailModel.DebitAmountFc;
                journalVoucherDetail.DebitAmount = 0;

                journalVoucherDetail.Narration = journalVoucherDetailModel.Narration;
                //journalVoucherDetail.SalesInvoiceId = journalVoucherDetailModel.SalesInvoiceId;
                //journalVoucherDetail.CreditNoteId = journalVoucherDetailModel.CreditNoteId;
                //journalVoucherDetail.PurchaseInvoiceId = journalVoucherDetailModel.PurchaseInvoiceId;
                //journalVoucherDetail.DebitNoteId = journalVoucherDetailModel.DebitNoteId;

                isUpdated = await Update(journalVoucherDetail);
            }

            if (isUpdated != false)
            {
                await UpdateJournalVoucherDetailAmount(journalVoucherDetailModel.JournalVoucherDetId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateJournalVoucherDetailAmount(int journalVoucherDetailId)
        {
            bool isUpdated = false;

            // get record.
            Journalvoucherdetail journalVoucherDetail = await GetQueryByCondition(w => w.JournalVoucherDetId == journalVoucherDetailId)
                                                                 .Include(w => w.JournalVoucher).FirstOrDefaultAsync();

            if (null != journalVoucherDetail)
            {
                journalVoucherDetail.CreditAmount = journalVoucherDetail.CreditAmountFc / journalVoucherDetail.JournalVoucher.ExchangeRate;
                journalVoucherDetail.DebitAmount = journalVoucherDetail.DebitAmountFc / journalVoucherDetail.JournalVoucher.ExchangeRate;

                isUpdated = await Update(journalVoucherDetail);
            }

            //if (isUpdated != false)
            //{
            //    await journalVoucher.UpdateJournalVoucherMasterAmount(journalVoucherDetail.JournalVoucherId);
            //}

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
                await _journalVoucher.UpdateJournalVoucherMasterAmount(journalVoucherDetail.JournalVoucherId);
            }

            return isDeleted; // returns.
        }

        public async Task<JournalVoucherDetailModel> GetJournalVoucherDetailById(int journalVoucherDetailId, int journalVoucherId)
        {
            JournalVoucherDetailModel journalVoucherDetailModel = null;

            IList<JournalVoucherDetailModel> journalVoucherModelDetailList = await GetJournalVoucherDetailList(journalVoucherDetailId, journalVoucherId);

            if (null != journalVoucherModelDetailList && journalVoucherModelDetailList.Any())
            {
                journalVoucherDetailModel = journalVoucherModelDetailList.FirstOrDefault();
            }

            return journalVoucherDetailModel; // returns.
        }

        public async Task<DataTableResultModel<JournalVoucherDetailModel>> GetJournalVoucherDetailByJournalVoucherId(int journalVoucherId, int addRow_Blank)
        {
            DataTableResultModel<JournalVoucherDetailModel> resultModel = new DataTableResultModel<JournalVoucherDetailModel>();

            IList<JournalVoucherDetailModel> journalVoucherDetailModelList = await GetJournalVoucherDetailList(0, journalVoucherId);

            if (null != journalVoucherDetailModelList && journalVoucherDetailModelList.Any())
            {
                if (addRow_Blank == 1)
                {
                    journalVoucherDetailModelList.Add(await AddRow_Blank(journalVoucherId));
                }

                resultModel = new DataTableResultModel<JournalVoucherDetailModel>();
                resultModel.ResultList = journalVoucherDetailModelList;
                resultModel.TotalResultCount = journalVoucherDetailModelList.Count();
            }
            else
            {
                journalVoucherDetailModelList = new List<JournalVoucherDetailModel>();

                if (addRow_Blank == 1)
                {
                    journalVoucherDetailModelList.Add(await AddRow_Blank(journalVoucherId));
                }

                resultModel = new DataTableResultModel<JournalVoucherDetailModel>();
                resultModel.ResultList = journalVoucherDetailModelList;
                resultModel.TotalResultCount = journalVoucherDetailModelList.Count();
            }


            return resultModel; // returns.
        }

        public async Task<IList<JournalVoucherDetailModel>> GetJournalVoucherDetailByVoucherId(int journalVoucherId, int addRow_Blank)
        {
            IList<JournalVoucherDetailModel> journalVoucherDetailModelList = await GetJournalVoucherDetailList(0, journalVoucherId);

            if (null != journalVoucherDetailModelList && journalVoucherDetailModelList.Any())
            {
                if (addRow_Blank == 1)
                {
                    journalVoucherDetailModelList.Add(await AddRow_Blank(journalVoucherId));
                }
            }
            else
            {
                journalVoucherDetailModelList = new List<JournalVoucherDetailModel>();

                if (addRow_Blank == 1)
                {
                    journalVoucherDetailModelList.Add(await AddRow_Blank(journalVoucherId));
                }

            }

            return journalVoucherDetailModelList; // returns.
        }

        private async Task<JournalVoucherDetailModel> AddRow_Blank(int journalVoucherId)
        {
            JournalVoucherDetailModel journalVoucherDetailModel = new JournalVoucherDetailModel();

            return await Task.Run(() =>
            {
                journalVoucherDetailModel.JournalVoucherId = journalVoucherId;
                journalVoucherDetailModel.ParticularLedgerId = 0;
                journalVoucherDetailModel.TransactionTypeId = 0;
                journalVoucherDetailModel.CreditAmountFc = 0;
                journalVoucherDetailModel.CreditAmount = 0;
                journalVoucherDetailModel.DebitAmountFc = 0;
                journalVoucherDetailModel.DebitAmount = 0;
                journalVoucherDetailModel.Narration = "";
                journalVoucherDetailModel.SalesInvoiceId = null;
                journalVoucherDetailModel.PurchaseInvoiceId = null;
                journalVoucherDetailModel.CreditNoteId = null;
                journalVoucherDetailModel.DebitNoteId = null;
                journalVoucherDetailModel.InvoiceNo = "";
                journalVoucherDetailModel.InvoiceType = "";
                journalVoucherDetailModel.ParticularLedgerName = "";
                journalVoucherDetailModel.TransactionTypeName = "";

                return journalVoucherDetailModel;
            });
        }

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
            IQueryable<Journalvoucherdetail> query = GetQueryByCondition(w => w.JournalVoucherDetId != 0)
                                                    .Include(w => w.ParticularLedger)
                                                    .Include(w => w.SalesInvoice).Include(w => w.CreditNote)
                                                    .Include(w => w.PurchaseInvoice).Include(w => w.DebitNote);

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
                journalVoucherDetailModel.TransactionTypeName = EnumHelper.GetEnumDescription<TransactionType_JV>(((TransactionType_JV)journalVoucherDetail.TransactionTypeId).ToString());
                journalVoucherDetailModel.ParticularLedgerName = null != journalVoucherDetail.ParticularLedger ? journalVoucherDetail.ParticularLedger.LedgerName : null;

                if (journalVoucherDetailModel.SalesInvoiceId != 0 && journalVoucherDetailModel.SalesInvoiceId != null)
                {
                    journalVoucherDetailModel.InvoiceType = "Sales Invoice";
                    journalVoucherDetailModel.InvoiceNo = null != journalVoucherDetail.SalesInvoice ? journalVoucherDetail.SalesInvoice.InvoiceNo : null;
                }
                else if (journalVoucherDetailModel.PurchaseInvoiceId != 0 && journalVoucherDetailModel.PurchaseInvoiceId != null)
                {
                    journalVoucherDetailModel.InvoiceType = "Purchase Invoice";
                    journalVoucherDetailModel.InvoiceNo = null != journalVoucherDetail.PurchaseInvoice ? journalVoucherDetail.PurchaseInvoice.InvoiceNo : null;
                }
                else if (journalVoucherDetailModel.CreditNoteId != 0 && journalVoucherDetailModel.CreditNoteId != null)
                {
                    journalVoucherDetailModel.InvoiceType = "Credit Note";
                    journalVoucherDetailModel.InvoiceNo = null != journalVoucherDetail.CreditNote ? journalVoucherDetail.CreditNote.CreditNoteNo : null;
                }
                else if (journalVoucherDetailModel.DebitNoteId != 0 && journalVoucherDetailModel.DebitNoteId != null)
                {
                    journalVoucherDetailModel.InvoiceType = "Debit Note";
                    journalVoucherDetailModel.InvoiceNo = null != journalVoucherDetail.DebitNote ? journalVoucherDetail.DebitNote.DebitNoteNo : null;
                }

                return journalVoucherDetailModel;
            });
        }

        public async Task<IList<GeneralLedgerModel>> GetTransactionList(int ledgerId, DateTime fromDate, DateTime toDate, int yearId, int companyId)
        {
            IList<GeneralLedgerModel> generalLedgerModelList = null;

            // create query.
            IQueryable<Journalvoucherdetail> query = GetQueryByCondition(w => w.JournalVoucherDetId != 0)
                                                .Include(i => i.JournalVoucher).ThenInclude(i => i.Currency)
                                                .Where((w => w.JournalVoucher.StatusId == (int)DocumentStatus.Approved && w.JournalVoucher.FinancialYearId == yearId && w.JournalVoucher.CompanyId == companyId));

            query = query.Where(w => w.ParticularLedgerId == ledgerId);

            query = query.Where(w => w.JournalVoucher.VoucherDate >= fromDate && w.JournalVoucher.VoucherDate <= toDate);

            // get records by query.
            List<Journalvoucherdetail> journalVoucherDetailList = await query.ToListAsync();

            generalLedgerModelList = new List<GeneralLedgerModel>();

            if (null != journalVoucherDetailList && journalVoucherDetailList.Count > 0)
            {
                foreach (Journalvoucherdetail journalVoucherDetail in journalVoucherDetailList)
                {
                    generalLedgerModelList.Add(new GeneralLedgerModel()
                    {
                        DocumentId = journalVoucherDetail.JournalVoucher.JournalVoucherId,
                        DocumentType = "Journal Voucher",
                        DocumentNo = journalVoucherDetail.JournalVoucher.VoucherNo,
                        DocumentDate = journalVoucherDetail.JournalVoucher.VoucherDate,
                        Amount_FC = journalVoucherDetail.DebitAmountFc == 0 ? journalVoucherDetail.CreditAmountFc : journalVoucherDetail.DebitAmountFc,
                        Amount = journalVoucherDetail.DebitAmount == 0 ? journalVoucherDetail.CreditAmount : journalVoucherDetail.DebitAmount,
                        DebitAmount_FC = journalVoucherDetail.DebitAmountFc,
                        DebitAmount = journalVoucherDetail.DebitAmount,
                        CreditAmount_FC = journalVoucherDetail.CreditAmountFc,
                        CreditAmount = journalVoucherDetail.CreditAmount,
                        JournalVoucherId = journalVoucherDetail.JournalVoucher.JournalVoucherId,
                        CurrencyId = journalVoucherDetail.JournalVoucher.CurrencyId,
                        CurrencyCode = journalVoucherDetail.JournalVoucher.Currency.CurrencyCode,
                        ExchangeRate = journalVoucherDetail.JournalVoucher.ExchangeRate,
                    });
                }
            }

            return generalLedgerModelList; // returns.
        }

    }
}
