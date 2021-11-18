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
    public class ContraVoucherDetailService : Repository<Contravoucherdetail>, IContraVoucherDetail
    {
        IContraVoucher contraVoucher;
        ILedger ledger;

        public ContraVoucherDetailService(ErpDbContext dbContext, IContraVoucher _contraVoucher, ILedger _ledger) : base(dbContext)
        {
            contraVoucher = _contraVoucher;
            ledger = _ledger;
        }

        public async Task<int> CreateContraVoucherDetail(ContraVoucherDetailModel contraVoucherDetailModel)
        {
            int contraVoucherDetailId = 0;

            Contravoucherdetail contraVoucherDetail = new Contravoucherdetail();

            contraVoucherDetail.ContraVoucherId = contraVoucherDetailModel.ContraVoucherId;
            contraVoucherDetail.ParticularLedgerId = contraVoucherDetailModel.ParticularLedgerId;
            contraVoucherDetail.CreditAmountFc = contraVoucherDetailModel.CreditAmountFc;
            contraVoucherDetail.CreditAmount = 0;
            contraVoucherDetail.DebitAmountFc = contraVoucherDetailModel.DebitAmountFc;
            contraVoucherDetail.DebitAmount = 0;

            contraVoucherDetail.Narration = contraVoucherDetailModel.Narration;

            await Create(contraVoucherDetail);

            contraVoucherDetailId = contraVoucherDetail.ContraVoucherDetId;

            if (contraVoucherDetailId != 0)
            {
                await UpdateContraVoucherDetailAmount(contraVoucherDetailId);
            }

            return contraVoucherDetailId; // returns.
        }

        public async Task<bool> UpdateContraVoucherDetail(ContraVoucherDetailModel contraVoucherDetailModel)
        {
            bool isUpdated = false;

            // get record.
            Contravoucherdetail contraVoucherDetail = await GetByIdAsync(w => w.ContraVoucherDetId == contraVoucherDetailModel.ContraVoucherDetId);

            if (null != contraVoucherDetail)
            {
                contraVoucherDetail.ParticularLedgerId = contraVoucherDetailModel.ParticularLedgerId;
                contraVoucherDetail.CreditAmountFc = contraVoucherDetailModel.CreditAmountFc;
                contraVoucherDetail.CreditAmount = 0;
                contraVoucherDetail.DebitAmountFc = contraVoucherDetailModel.DebitAmountFc;
                contraVoucherDetail.DebitAmount = 0;

                contraVoucherDetail.Narration = contraVoucherDetailModel.Narration;

                isUpdated = await Update(contraVoucherDetail);
            }

            if (isUpdated != false)
            {
                await UpdateContraVoucherDetailAmount(contraVoucherDetailModel.ContraVoucherDetId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateContraVoucherDetailAmount(int contraVoucherDetailId)
        {
            bool isUpdated = false;

            // get record.
            Contravoucherdetail contraVoucherDetail = await GetQueryByCondition(w => w.ContraVoucherDetId == contraVoucherDetailId)
                                                                 .Include(w => w.ContraVoucher).FirstOrDefaultAsync();

            if (null != contraVoucherDetail)
            {
                contraVoucherDetail.CreditAmount = contraVoucherDetail.CreditAmountFc / contraVoucherDetail.ContraVoucher.ExchangeRate;
                contraVoucherDetail.DebitAmount = contraVoucherDetail.DebitAmountFc / contraVoucherDetail.ContraVoucher.ExchangeRate;

                isUpdated = await Update(contraVoucherDetail);
            }

            //if (isUpdated != false)
            //{
            //    await contraVoucher.UpdateContraVoucherMasterAmount(contraVoucherDetail.ContraVoucherId);
            //}

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

        public async Task<ContraVoucherDetailModel> GetContraVoucherDetailById(int contraVoucherDetailId, int contraVoucherId)
        {
            ContraVoucherDetailModel contraVoucherDetailModel = null;

            IList<ContraVoucherDetailModel> contraVoucherModelDetailList = await GetContraVoucherDetailList(contraVoucherDetailId, contraVoucherId);

            if (null != contraVoucherModelDetailList && contraVoucherModelDetailList.Any())
            {
                contraVoucherDetailModel = contraVoucherModelDetailList.FirstOrDefault();
            }

            return contraVoucherDetailModel; // returns.
        }

        public async Task<DataTableResultModel<ContraVoucherDetailModel>> GetContraVoucherDetailByContraVoucherId(int contraVoucherId, int addRow_Blank)
        {
            DataTableResultModel<ContraVoucherDetailModel> resultModel = new DataTableResultModel<ContraVoucherDetailModel>();

            IList<ContraVoucherDetailModel> contraVoucherDetailModelList = await GetContraVoucherDetailList(0, contraVoucherId);

            if (null != contraVoucherDetailModelList && contraVoucherDetailModelList.Any())
            {
                if (addRow_Blank == 1)
                {
                    contraVoucherDetailModelList.Add(await AddRow_Blank(contraVoucherId));
                }

                resultModel = new DataTableResultModel<ContraVoucherDetailModel>();
                resultModel.ResultList = contraVoucherDetailModelList;
                resultModel.TotalResultCount = contraVoucherDetailModelList.Count();
            }
            else
            {
                contraVoucherDetailModelList = new List<ContraVoucherDetailModel>();

                if (addRow_Blank == 1)
                {
                    contraVoucherDetailModelList.Add(await AddRow_Blank(contraVoucherId));
                }

                resultModel = new DataTableResultModel<ContraVoucherDetailModel>();
                resultModel.ResultList = contraVoucherDetailModelList;
                resultModel.TotalResultCount = contraVoucherDetailModelList.Count();
            }


            return resultModel; // returns.
        }

        public async Task<IList<ContraVoucherDetailModel>> GetContraVoucherDetailByVoucherId(int contraVoucherId, int addRow_Blank)
        {
            IList<ContraVoucherDetailModel> contraVoucherDetailModelList = await GetContraVoucherDetailList(0, contraVoucherId);

            if (null != contraVoucherDetailModelList && contraVoucherDetailModelList.Any())
            {
                if (addRow_Blank == 1)
                {
                    contraVoucherDetailModelList.Add(await AddRow_Blank(contraVoucherId));
                }
            }
            else
            {
                contraVoucherDetailModelList = new List<ContraVoucherDetailModel>();

                if (addRow_Blank == 1)
                {
                    contraVoucherDetailModelList.Add(await AddRow_Blank(contraVoucherId));
                }

            }

            return contraVoucherDetailModelList; // returns.
        }

        private async Task<ContraVoucherDetailModel> AddRow_Blank(int contraVoucherId)
        {
            ContraVoucherDetailModel contraVoucherDetailModel = new ContraVoucherDetailModel();

            return await Task.Run(() =>
            {
                contraVoucherDetailModel.ContraVoucherId = contraVoucherId;
                contraVoucherDetailModel.ParticularLedgerId = 0;
                contraVoucherDetailModel.CreditAmountFc = 0;
                contraVoucherDetailModel.CreditAmount = 0;
                contraVoucherDetailModel.DebitAmountFc = 0;
                contraVoucherDetailModel.DebitAmount = 0;
                contraVoucherDetailModel.Narration = "";
                contraVoucherDetailModel.ParticularLedgerName = "";

                return contraVoucherDetailModel;
            });
        }

        public async Task<IList<ContraVoucherDetailModel>> GetInvoiceListByParticularLedgerId(int particularLedgerId)
        {
            IList<ContraVoucherDetailModel> contraVoucherDetailModelList = null;

            // create query.
            IQueryable<Contravoucherdetail> query = GetQueryByCondition(w => w.ContraVoucherDetId != 0);

            // apply filters.
            if (0 != particularLedgerId)
                query = query.Where(w => w.ParticularLedgerId == particularLedgerId);

            // get records by query.
            List<Contravoucherdetail> contraVoucherDetailList = await query.ToListAsync();

            contraVoucherDetailModelList = new List<ContraVoucherDetailModel>();

            if (null != contraVoucherDetailList && contraVoucherDetailList.Count > 0)
            {
                foreach (Contravoucherdetail contraVoucherDetail in contraVoucherDetailList)
                {
                    contraVoucherDetailModelList.Add(await AssignValueToModel(contraVoucherDetail));
                }
            }

            return contraVoucherDetailModelList; // returns.
        }

        private async Task<IList<ContraVoucherDetailModel>> GetContraVoucherDetailList(int contraVoucherDetailId, int contraVoucherId)
        {
            IList<ContraVoucherDetailModel> contraVoucherDetailModelList = null;

            // create query.
            IQueryable<Contravoucherdetail> query = GetQueryByCondition(w => w.ContraVoucherDetId != 0)
                                                    .Include(w => w.ParticularLedger);

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
                contraVoucherDetailModel.ParticularLedgerName = null != contraVoucherDetail.ParticularLedger ? contraVoucherDetail.ParticularLedger.LedgerName : null;

                return contraVoucherDetailModel;
            });
        }

        public async Task<IList<GeneralLedgerModel>> GetTransactionList(int ledgerId, DateTime fromDate, DateTime toDate, int yearId, int companyId)
        {
            IList<GeneralLedgerModel> generalLedgerModelList = null;

            // create query.
            IQueryable<Contravoucherdetail> query = GetQueryByCondition(w => w.ContraVoucherDetId != 0)
                                                .Include(i => i.ContraVoucher).ThenInclude(i => i.Currency)
                                                .Where((w => w.ContraVoucher.StatusId == (int)DocumentStatus.Approved && w.ContraVoucher.FinancialYearId == yearId && w.ContraVoucher.CompanyId == companyId));

            query = query.Where(w => w.ParticularLedgerId == ledgerId);

            query = query.Where(w => w.ContraVoucher.VoucherDate >= fromDate && w.ContraVoucher.VoucherDate <= toDate);

            // get records by query.
            List<Contravoucherdetail> contraVoucherDetailList = await query.ToListAsync();

            generalLedgerModelList = new List<GeneralLedgerModel>();

            if (null != contraVoucherDetailList && contraVoucherDetailList.Count > 0)
            {
                foreach (Contravoucherdetail contraVoucherDetail in contraVoucherDetailList)
                {
                    generalLedgerModelList.Add(new GeneralLedgerModel()
                    {
                        DocumentId = contraVoucherDetail.ContraVoucher.ContraVoucherId,
                        DocumentType = "Contra Voucher",
                        DocumentNo = contraVoucherDetail.ContraVoucher.VoucherNo,
                        DocumentDate = contraVoucherDetail.ContraVoucher.VoucherDate,
                        Amount_FC = contraVoucherDetail.DebitAmountFc == 0 ? contraVoucherDetail.CreditAmountFc : contraVoucherDetail.DebitAmountFc,
                        Amount = contraVoucherDetail.DebitAmount == 0 ? contraVoucherDetail.CreditAmount : contraVoucherDetail.DebitAmount,
                        DebitAmount_FC = contraVoucherDetail.DebitAmountFc,
                        DebitAmount = contraVoucherDetail.DebitAmount,
                        CreditAmount_FC = contraVoucherDetail.CreditAmountFc,
                        CreditAmount = contraVoucherDetail.CreditAmount,
                        ContraVoucherId = contraVoucherDetail.ContraVoucher.ContraVoucherId,
                        CurrencyId = contraVoucherDetail.ContraVoucher.CurrencyId,
                        CurrencyCode = contraVoucherDetail.ContraVoucher.Currency.CurrencyCode,
                        ExchangeRate = contraVoucherDetail.ContraVoucher.ExchangeRate,
                        //PartyReferenceNo = contraVoucherDetail.ContraVoucher.ChequeNo,
                    });
                }
            }

            return generalLedgerModelList; // returns.
        }

    }
}
