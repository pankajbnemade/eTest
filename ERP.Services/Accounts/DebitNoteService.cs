using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Models.Master;
using ERP.Services.Accounts.Interface;
using ERP.Services.Common.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace ERP.Services.Accounts
{
    public class DebitNoteService : Repository<Debitnote>, IDebitNote
    {
        ICommon common;
        public DebitNoteService(ErpDbContext dbContext, ICommon _common) : base(dbContext)
        {
            common = _common;
        }

        /// <summary>
        /// generate invoice no.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="financialYearId"></param>
        /// <returns>
        /// return invoice no.
        /// </returns>
        public async Task<GenerateNoModel> GenerateDebitNoteNo(int companyId, int financialYearId)
        {
            int voucherSetupId = 5;
            // get maxno.
            int? maxNo = await GetQueryByCondition(w => w.CompanyId == companyId && w.FinancialYearId == financialYearId).MaxAsync(m => m.MaxNo);

            GenerateNoModel generateNoModel = await common.GenerateVoucherNo(Convert.ToInt32(maxNo), voucherSetupId, companyId, financialYearId);

            return generateNoModel; // returns.
        }

        /// <summary>
        /// create new purchase invoice.
        /// </summary>
        /// <param name="debitNoteModel"></param>
        /// <returns>
        /// return id.
        /// </returns>
        public async Task<int> CreateDebitNote(DebitNoteModel debitNoteModel)
        {
            int debitNoteId = 0;

            GenerateNoModel generateNoModel = await GenerateDebitNoteNo(debitNoteModel.CompanyId, debitNoteModel.FinancialYearId);

            // assign values.
            Debitnote debitNote = new Debitnote();

            debitNote.DebitNoteNo = generateNoModel.VoucherNo;
            debitNote.MaxNo = generateNoModel.MaxNo;
            debitNote.VoucherStyleId = generateNoModel.VoucherStyleId;

            debitNote.DebitNoteDate = debitNoteModel.DebitNoteDate;
            debitNote.PartyLedgerId = debitNoteModel.PartyLedgerId;
            debitNote.BillToAddressId = debitNoteModel.BillToAddressId;
            debitNote.AccountLedgerId = debitNoteModel.AccountLedgerId;
            debitNote.PartyReferenceNo = debitNoteModel.PartyReferenceNo;
            debitNote.PartyReferenceDate = debitNoteModel.PartyReferenceDate;
            debitNote.OurReferenceNo = debitNoteModel.OurReferenceNo;
            debitNote.OurReferenceDate = debitNoteModel.OurReferenceDate;
            debitNote.CreditLimitDays = debitNoteModel.CreditLimitDays;
            debitNote.PaymentTerm = debitNoteModel.PaymentTerm;
            debitNote.Remark = debitNoteModel.Remark;
            debitNote.TaxModelType = debitNoteModel.TaxModelType;
            debitNote.TaxRegisterId = debitNoteModel.TaxRegisterId;
            debitNote.CurrencyId = debitNoteModel.CurrencyId;
            debitNote.ExchangeRate = debitNoteModel.ExchangeRate;
            debitNote.TotalLineItemAmountFc = 0;
            debitNote.TotalLineItemAmount = 0;
            debitNote.GrossAmountFc = 0;
            debitNote.GrossAmount = 0;
            debitNote.NetAmountFc = 0;
            debitNote.NetAmount = 0;
            debitNote.NetAmountFcinWord = "";
            debitNote.TaxAmountFc = 0;
            debitNote.TaxAmount = 0;

            debitNote.DiscountPercentageOrAmount = debitNoteModel.DiscountPercentageOrAmount;
            debitNote.DiscountPerOrAmountFc = debitNoteModel.DiscountPerOrAmountFc;
            debitNote.DiscountAmountFc = 0;
            debitNote.DiscountAmount = 0;

            debitNote.StatusId = 1;
            debitNote.CompanyId = debitNoteModel.CompanyId;
            debitNote.FinancialYearId = debitNoteModel.FinancialYearId;

            await Create(debitNote);
            debitNoteId = debitNote.DebitNoteId;

            if (debitNoteId != 0)
            {
                await UpdateDebitNoteMasterAmount(debitNoteId);
            }

            return debitNoteId; // returns.
        }

        /// <summary>
        /// update purchase invoice.
        /// </summary>
        /// <param name="debitNoteModel"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> UpdateDebitNote(DebitNoteModel debitNoteModel)
        {
            bool isUpdated = false;

            // get record.
            Debitnote debitNote = await GetByIdAsync(w => w.DebitNoteId == debitNoteModel.DebitNoteId);

            if (null != debitNote)
            {
                // assign values.

                debitNote.DebitNoteDate = debitNoteModel.DebitNoteDate;
                debitNote.PartyLedgerId = debitNoteModel.PartyLedgerId;
                debitNote.BillToAddressId = debitNoteModel.BillToAddressId;
                debitNote.AccountLedgerId = debitNoteModel.AccountLedgerId;
                debitNote.PartyReferenceNo = debitNoteModel.PartyReferenceNo;
                debitNote.PartyReferenceDate = debitNoteModel.PartyReferenceDate;
                debitNote.OurReferenceNo = debitNoteModel.OurReferenceNo;
                debitNote.OurReferenceDate = debitNoteModel.OurReferenceDate;
                debitNote.CreditLimitDays = debitNoteModel.CreditLimitDays;
                debitNote.PaymentTerm = debitNoteModel.PaymentTerm;
                debitNote.Remark = debitNoteModel.Remark;
                debitNote.TaxModelType = debitNoteModel.TaxModelType;
                debitNote.TaxRegisterId = debitNoteModel.TaxRegisterId;
                debitNote.CurrencyId = debitNoteModel.CurrencyId;
                debitNote.ExchangeRate = debitNoteModel.ExchangeRate;

                debitNote.TotalLineItemAmountFc = 0;
                debitNote.TotalLineItemAmount = 0;
                debitNote.GrossAmountFc = 0;
                debitNote.GrossAmount = 0;
                debitNote.NetAmountFc = 0;
                debitNote.NetAmount = 0;
                debitNote.NetAmountFcinWord = "";
                debitNote.TaxAmountFc = 0;
                debitNote.TaxAmount = 0;

                debitNote.DiscountPercentageOrAmount = debitNoteModel.DiscountPercentageOrAmount;
                debitNote.DiscountPerOrAmountFc = debitNoteModel.DiscountPerOrAmountFc;

                debitNote.DiscountAmountFc = 0;
                debitNote.DiscountAmount = 0;

                isUpdated = await Update(debitNote);
            }

            if (isUpdated != false)
            {
                await UpdateDebitNoteMasterAmount(debitNote.DebitNoteId);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// delete purchase invoice.
        /// </summary>
        /// <param name="salesDebitNoteId"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> DeleteDebitNote(int salesDebitNoteId)
        {
            bool isDeleted = false;

            // get record.
            Debitnote debitNote = await GetByIdAsync(w => w.DebitNoteId == salesDebitNoteId);
            if (null != debitNote)
            {
                isDeleted = await Delete(debitNote);
            }

            return isDeleted; // returns.
        }

        public async Task<bool> UpdateDebitNoteMasterAmount(int? debitNoteId)
        {
            bool isUpdated = false;

            // get record.
            Debitnote debitNote = await GetQueryByCondition(w => w.DebitNoteId == debitNoteId)
                                                    .Include(w => w.Debitnotedetails).Include(w => w.Debitnotetaxes)
                                                    .Include(w => w.Currency).FirstOrDefaultAsync();

            if (null != debitNote)
            {
                debitNote.TotalLineItemAmountFc = debitNote.Debitnotedetails.Sum(w => w.GrossAmountFc);
                debitNote.TotalLineItemAmount = debitNote.TotalLineItemAmountFc * debitNote.ExchangeRate;

                if (DiscountType.Percentage.ToString() == debitNote.DiscountPercentageOrAmount)
                {
                    debitNote.DiscountAmountFc = (debitNote.TotalLineItemAmountFc * debitNote.DiscountPerOrAmountFc) / 100;
                }
                else
                {
                    debitNote.DiscountAmountFc = debitNote.DiscountPerOrAmountFc;
                }

                debitNote.DiscountAmount = debitNote.DiscountAmountFc * debitNote.ExchangeRate;

                debitNote.GrossAmountFc = debitNote.TotalLineItemAmountFc + debitNote.DiscountAmountFc;
                debitNote.GrossAmount = debitNote.GrossAmountFc * debitNote.ExchangeRate;

                if (TaxModelType.LineWise.ToString() == debitNote.TaxModelType)
                {
                    debitNote.TaxAmountFc = debitNote.Debitnotedetails.Sum(w => w.TaxAmountFc);
                }
                else
                {
                    debitNote.TaxAmountFc = debitNote.Debitnotetaxes.Sum(w => w.TaxAmountFc);
                }

                debitNote.TaxAmount = debitNote.TaxAmountFc * debitNote.ExchangeRate;

                debitNote.NetAmountFc = debitNote.GrossAmountFc + debitNote.DiscountAmountFc;
                debitNote.NetAmount = debitNote.NetAmountFc * debitNote.ExchangeRate;

                debitNote.NetAmountFcinWord = await common.AmountInWord_Million(debitNote.NetAmountFc.ToString(), debitNote.Currency.CurrencyCode, debitNote.Currency.Denomination);

                isUpdated = await Update(debitNote);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// get purchase invoice based on salesDebitNoteId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<DebitNoteModel> GetDebitNoteById(int debitNoteId)
        {
            DebitNoteModel debitNoteModel = null;

            IList<DebitNoteModel> debitNoteModelList = await GetDebitNoteList(debitNoteId);

            if (null != debitNoteModelList && debitNoteModelList.Any())
            {
                debitNoteModel = debitNoteModelList.FirstOrDefault();
            }

            return debitNoteModel; // returns.
        }

        /// <summary>
        /// get search purchase invoice result list.
        /// </summary>
        /// <param name="dataTableAjaxPostModel"></param>
        /// <param name="searchFilterModel"></param>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<DataTableResultModel<DebitNoteModel>> GetDebitNoteList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterDebitNoteModel searchFilterModel)
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
            DataTableResultModel<DebitNoteModel> resultModel = await GetDataFromDbase(searchFilterModel, searchBy, take, skip, sortBy, sortDir);

            return resultModel; // returns.
        }

        /// <summary>
        /// get debit note List based on partyLedgerId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<IList<OutstandingInvoiceModel>> GetDebitNoteListByPartyLedgerId(int partyLedgerId)
        {
            IList<OutstandingInvoiceModel> outstandingInvoiceModelList = null;

            // create query.
            IQueryable<Debitnote> query = GetQueryByCondition(w => w.DebitNoteId != 0);

            // apply filters.
            if (0 != partyLedgerId)
                query = query.Where(w => w.PartyLedgerId == partyLedgerId);

            // get records by query.
            List<Debitnote> debitNoteList = await query.ToListAsync();
            
            outstandingInvoiceModelList = new List<OutstandingInvoiceModel>();

            if (null != debitNoteList && debitNoteList.Count > 0)
            {
                
                foreach (Debitnote debitNote in debitNoteList)
                {
                    outstandingInvoiceModelList.Add(new OutstandingInvoiceModel()
                    {
                        InvoiceId = debitNote.DebitNoteId,
                        InvoiceType = "Debit Note",
                        InvoiceNo = debitNote.DebitNoteNo,
                        InvoiceDate = debitNote.DebitNoteDate,
                        InvoiceAmount = debitNote.NetAmount,
                        DebitNoteId = debitNote.DebitNoteId,
                        SalesInvoiceId = 0,
                        PurchaseInvoiceId = 0,
                        CreditNoteId = 0
                    });
                }
            }

            return outstandingInvoiceModelList; // returns.
        }

        #region Private Methods

        /// <summary>
        /// get records from database.
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortDir"></param>
        /// <returns></returns>
        private async Task<DataTableResultModel<DebitNoteModel>> GetDataFromDbase(SearchFilterDebitNoteModel searchFilterModel, string searchBy, int take, int skip, string sortBy, string sortDir)
        {
            DataTableResultModel<DebitNoteModel> resultModel = new DataTableResultModel<DebitNoteModel>();

            IQueryable<Debitnote> query = GetQueryByCondition(w => w.DebitNoteId != 0);

            if (!string.IsNullOrEmpty(searchFilterModel.DebitNoteNo))
            {
                query = query.Where(w => w.DebitNoteNo.Contains(searchFilterModel.DebitNoteNo));
            }

            if (null != searchFilterModel.PartyLedgerId)
            {
                query = query.Where(w => w.PartyLedgerId == searchFilterModel.PartyLedgerId);
            }

            if (null != searchFilterModel.FromDate)
            {
                query = query.Where(w => w.DebitNoteDate >= searchFilterModel.FromDate);
            }

            if (null != searchFilterModel.ToDate)
            {
                query = query.Where(w => w.DebitNoteDate <= searchFilterModel.ToDate);
            }

            // get total count.
            resultModel.TotalResultCount = await query.CountAsync();

            //sorting
            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(sortDir))
            {
                query = query.OrderBy($"{sortBy} {sortDir}");
            }

            // datatable search
            if (!string.IsNullOrEmpty(searchBy))
            {
                query = query.Where(w => w.DebitNoteNo.ToLower().Contains(searchBy.ToLower()));
            }

            // get records based on pagesize.
            query = query.Skip(skip).Take(take);

            resultModel.ResultList = await query.Select(s => new DebitNoteModel
            {
                DebitNoteId = s.DebitNoteId,
                DebitNoteNo = s.DebitNoteNo,
                DebitNoteDate = s.DebitNoteDate,
                NetAmount = s.NetAmount,
            }).ToListAsync();

            // get filter record count.
            resultModel.FilterResultCount = await query.CountAsync();

            return resultModel; // returns.
        }

        private async Task<IList<DebitNoteModel>> GetDebitNoteList(int debitNoteId)
        {
            IList<DebitNoteModel> debitNoteModelList = null;

            // create query.
            IQueryable<Debitnote> query = GetQueryByCondition(w => w.DebitNoteId != 0)
                                            .Include(w => w.PartyLedger).Include(w => w.BillToAddress)
                                            .Include(w => w.AccountLedger)
                                            .Include(w => w.TaxRegister).Include(w => w.Currency)
                                            .Include(w => w.Status).Include(w => w.PreparedByUser);



            // apply filters.
            if (0 != debitNoteId)
                query = query.Where(w => w.DebitNoteId == debitNoteId);

            // get records by query.
            List<Debitnote> debitNoteList = await query.ToListAsync();

            if (null != debitNoteList && debitNoteList.Count > 0)
            {
                debitNoteModelList = new List<DebitNoteModel>();

                foreach (Debitnote debitNote in debitNoteList)
                {
                    debitNoteModelList.Add(await AssignValueToModel(debitNote));
                }
            }

            return debitNoteModelList; // returns.
        }

        private async Task<DebitNoteModel> AssignValueToModel(Debitnote debitNote)
        {
            return await Task.Run(() =>
            {
                DebitNoteModel debitNoteModel = new DebitNoteModel();

                debitNoteModel.DebitNoteId = debitNote.DebitNoteId;
                debitNoteModel.DebitNoteNo = debitNote.DebitNoteNo;
                debitNoteModel.DebitNoteDate = debitNote.DebitNoteDate;
                debitNoteModel.PartyLedgerId = debitNote.PartyLedgerId;
                debitNoteModel.BillToAddressId = debitNote.BillToAddressId;
                debitNoteModel.AccountLedgerId = debitNote.AccountLedgerId;
                debitNoteModel.PartyReferenceNo = debitNote.PartyReferenceNo;
                debitNoteModel.PartyReferenceDate = debitNote.PartyReferenceDate;
                debitNoteModel.OurReferenceNo = debitNote.OurReferenceNo;
                debitNoteModel.OurReferenceDate = debitNote.OurReferenceDate;
                debitNoteModel.CreditLimitDays = debitNote.CreditLimitDays;
                debitNoteModel.PaymentTerm = debitNote.PaymentTerm;
                debitNoteModel.Remark = debitNote.Remark;
                debitNoteModel.TaxModelType = debitNote.TaxModelType;
                debitNoteModel.TaxRegisterId = debitNote.TaxRegisterId;
                debitNoteModel.CurrencyId = debitNote.CurrencyId;
                debitNoteModel.ExchangeRate = debitNote.ExchangeRate;
                debitNoteModel.TotalLineItemAmountFc = debitNote.TotalLineItemAmountFc;
                debitNoteModel.TotalLineItemAmount = debitNote.TotalLineItemAmount;
                debitNoteModel.GrossAmountFc = debitNote.GrossAmountFc;
                debitNoteModel.GrossAmount = debitNote.GrossAmount;
                debitNoteModel.NetAmountFc = debitNote.NetAmountFc;
                debitNoteModel.NetAmount = debitNote.NetAmount;
                debitNoteModel.NetAmountFcinWord = debitNote.NetAmountFcinWord;
                debitNoteModel.TaxAmountFc = debitNote.TaxAmountFc;
                debitNoteModel.TaxAmount = debitNote.TaxAmount;
                debitNoteModel.DiscountPercentageOrAmount = debitNote.DiscountPercentageOrAmount;
                debitNoteModel.DiscountPerOrAmountFc = debitNote.DiscountPerOrAmountFc;
                debitNoteModel.DiscountAmountFc = debitNote.DiscountAmountFc;
                debitNoteModel.DiscountAmount = debitNote.DiscountAmount;
                debitNoteModel.StatusId = debitNote.StatusId;
                debitNoteModel.CompanyId = Convert.ToInt32(debitNote.CompanyId);
                debitNoteModel.FinancialYearId = Convert.ToInt32(debitNote.FinancialYearId);
                debitNoteModel.MaxNo = debitNote.MaxNo;
                debitNoteModel.VoucherStyleId = debitNote.VoucherStyleId;
                //debitNoteModel.PreparedByUserId = debitNote.PreparedByUserId;
                //debitNoteModel.UpdatedByUserId = debitNote.UpdatedByUserId;
                //debitNoteModel.PreparedDateTime = debitNote.PreparedDateTime;
                //debitNoteModel.UpdatedDateTime = debitNote.UpdatedDateTime;

                // ###
                debitNoteModel.PartyLedgerName = null != debitNote.PartyLedger ? debitNote.PartyLedger.LedgerName : null;
                debitNoteModel.BillToAddress = null != debitNote.BillToAddress ? debitNote.BillToAddress.AddressDescription : null;
                debitNoteModel.AccountLedgerName = null != debitNote.AccountLedger ? debitNote.AccountLedger.LedgerName : null;
                debitNoteModel.TaxRegisterName = null != debitNote.TaxRegister ? debitNote.TaxRegister.TaxRegisterName : null;
                debitNoteModel.CurrencyName = null != debitNote.Currency ? debitNote.Currency.CurrencyName : null;
                debitNoteModel.StatusName = null != debitNote.Status ? debitNote.Status.StatusName : null;
                debitNoteModel.PreparedByName = null != debitNote.PreparedByUser ? debitNote.PreparedByUser.UserName : null;

                return debitNoteModel;
            });

        }

        #endregion Private Methods
    }
}
