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
using ERP.Models.Accounts.Enums;

namespace ERP.Services.Accounts
{
    public class PurchaseInvoiceService : Repository<Purchaseinvoice>, IPurchaseInvoice
    {
        private readonly ICommon common;
        public PurchaseInvoiceService(ErpDbContext dbContext, ICommon _common) : base(dbContext)
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
        public async Task<GenerateNoModel> GenerateInvoiceNo(int companyId, int financialYearId)
        {
            int voucherSetupId = 3;
            // get maxno.
            int maxNo = await GetQueryByCondition(w => w.CompanyId == companyId && w.FinancialYearId == financialYearId).MaxAsync(m => m.MaxNo);

            GenerateNoModel generateNoModel = await common.GenerateVoucherNo(Convert.ToInt32(maxNo), voucherSetupId, companyId, financialYearId);

            return generateNoModel; // returns.
        }

        /// <summary>
        /// create new purchase invoice.
        /// </summary>
        /// <param name="purchaseInvoiceModel"></param>
        /// <returns>
        /// return id.
        /// </returns>
        public async Task<int> CreatePurchaseInvoice(PurchaseInvoiceModel purchaseInvoiceModel)
        {
            int purchaseInvoiceId = 0;

            GenerateNoModel generateNoModel = await GenerateInvoiceNo(purchaseInvoiceModel.CompanyId, purchaseInvoiceModel.FinancialYearId);

            // assign values.
            Purchaseinvoice purchaseInvoice = new Purchaseinvoice();

            purchaseInvoice.InvoiceNo = generateNoModel.VoucherNo;
            purchaseInvoice.MaxNo = generateNoModel.MaxNo;
            purchaseInvoice.VoucherStyleId = generateNoModel.VoucherStyleId;

            purchaseInvoice.InvoiceDate = purchaseInvoiceModel.InvoiceDate;
            purchaseInvoice.SupplierLedgerId = purchaseInvoiceModel.SupplierLedgerId;
            purchaseInvoice.BillToAddressId = purchaseInvoiceModel.BillToAddressId;
            purchaseInvoice.AccountLedgerId = purchaseInvoiceModel.AccountLedgerId;
            purchaseInvoice.SupplierReferenceNo = purchaseInvoiceModel.SupplierReferenceNo;
            purchaseInvoice.SupplierReferenceDate = purchaseInvoiceModel.SupplierReferenceDate;
            purchaseInvoice.CreditLimitDays = purchaseInvoiceModel.CreditLimitDays;
            purchaseInvoice.PaymentTerm = purchaseInvoiceModel.PaymentTerm;
            purchaseInvoice.Remark = purchaseInvoiceModel.Remark;
            purchaseInvoice.TaxModelType = purchaseInvoiceModel.TaxModelType;
            purchaseInvoice.TaxRegisterId = purchaseInvoiceModel.TaxRegisterId;
            purchaseInvoice.CurrencyId = purchaseInvoiceModel.CurrencyId;
            purchaseInvoice.ExchangeRate = purchaseInvoiceModel.ExchangeRate;
            purchaseInvoice.TotalLineItemAmountFc = 0;
            purchaseInvoice.TotalLineItemAmount = 0;
            purchaseInvoice.GrossAmountFc = 0;
            purchaseInvoice.GrossAmount = 0;
            purchaseInvoice.NetAmountFc = 0;
            purchaseInvoice.NetAmount = 0;
            purchaseInvoice.NetAmountFcinWord = "";
            purchaseInvoice.TaxAmountFc = 0;
            purchaseInvoice.TaxAmount = 0;

            purchaseInvoice.DiscountPercentageOrAmount = purchaseInvoiceModel.DiscountPercentageOrAmount;
            purchaseInvoice.DiscountPerOrAmountFc = purchaseInvoiceModel.DiscountPerOrAmountFc;
            purchaseInvoice.DiscountAmountFc = 0;
            purchaseInvoice.DiscountAmount = 0;

            purchaseInvoice.StatusId = (int)DocumentStatus.Inprocess;
            purchaseInvoice.CompanyId = purchaseInvoiceModel.CompanyId;
            purchaseInvoice.FinancialYearId = purchaseInvoiceModel.FinancialYearId;

            await Create(purchaseInvoice);

            purchaseInvoiceId = purchaseInvoice.PurchaseInvoiceId;

            if (purchaseInvoiceId != 0)
            {
                await UpdatePurchaseInvoiceMasterAmount(purchaseInvoiceId);
            }

            return purchaseInvoiceId; // returns.
        }

        /// <summary>
        /// update purchase invoice.
        /// </summary>
        /// <param name="purchaseInvoiceModel"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> UpdatePurchaseInvoice(PurchaseInvoiceModel purchaseInvoiceModel)
        {
            bool isUpdated = false;

            // get record.
            Purchaseinvoice purchaseInvoice = await GetByIdAsync(w => w.PurchaseInvoiceId == purchaseInvoiceModel.PurchaseInvoiceId);

            if (null != purchaseInvoice)
            {
                // assign values.
                //purchaseInvoice.PurchaseInvoiceId = purchaseInvoiceModel.PurchaseInvoiceId;
                //purchaseInvoice.InvoiceNo = purchaseInvoiceModel.InvoiceNo;
                purchaseInvoice.InvoiceDate = purchaseInvoiceModel.InvoiceDate;
                purchaseInvoice.SupplierLedgerId = purchaseInvoiceModel.SupplierLedgerId;
                purchaseInvoice.BillToAddressId = purchaseInvoiceModel.BillToAddressId;
                purchaseInvoice.AccountLedgerId = purchaseInvoiceModel.AccountLedgerId;
                purchaseInvoice.SupplierReferenceNo = purchaseInvoiceModel.SupplierReferenceNo;
                purchaseInvoice.SupplierReferenceDate = purchaseInvoiceModel.SupplierReferenceDate;
                purchaseInvoice.CreditLimitDays = purchaseInvoiceModel.CreditLimitDays;
                purchaseInvoice.PaymentTerm = purchaseInvoiceModel.PaymentTerm;
                purchaseInvoice.Remark = purchaseInvoiceModel.Remark;
                purchaseInvoice.TaxModelType = purchaseInvoiceModel.TaxModelType;
                purchaseInvoice.TaxRegisterId = purchaseInvoiceModel.TaxRegisterId;
                purchaseInvoice.CurrencyId = purchaseInvoiceModel.CurrencyId;
                purchaseInvoice.ExchangeRate = purchaseInvoiceModel.ExchangeRate;

                purchaseInvoice.TotalLineItemAmountFc = 0;
                purchaseInvoice.TotalLineItemAmount = 0;
                purchaseInvoice.GrossAmountFc = 0;
                purchaseInvoice.GrossAmount = 0;
                purchaseInvoice.NetAmountFc = 0;
                purchaseInvoice.NetAmount = 0;
                purchaseInvoice.NetAmountFcinWord = "";
                purchaseInvoice.TaxAmountFc = 0;
                purchaseInvoice.TaxAmount = 0;

                purchaseInvoice.DiscountPercentageOrAmount = purchaseInvoiceModel.DiscountPercentageOrAmount;
                purchaseInvoice.DiscountPerOrAmountFc = purchaseInvoiceModel.DiscountPerOrAmountFc;

                purchaseInvoice.DiscountAmountFc = 0;
                purchaseInvoice.DiscountAmount = 0;

                isUpdated = await Update(purchaseInvoice);
            }

            if (isUpdated != false)
            {
                await UpdatePurchaseInvoiceMasterAmount(purchaseInvoice.PurchaseInvoiceId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateStatusPurchaseInvoice(int purchaseInvoiceId, int statusId)
        {
            bool isUpdated = false;

            // get record.
            Purchaseinvoice purchaseInvoice = await GetByIdAsync(w => w.PurchaseInvoiceId == purchaseInvoiceId);

            if (null != purchaseInvoice)
            {
                purchaseInvoice.StatusId = statusId;
                isUpdated = await Update(purchaseInvoice);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// delete purchase invoice.
        /// </summary>
        /// <param name="purchaseInvoiceId"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> DeletePurchaseInvoice(int purchaseInvoiceId)
        {
            bool isDeleted = false;

            // get record.
            Purchaseinvoice purchaseInvoice = await GetByIdAsync(w => w.PurchaseInvoiceId == purchaseInvoiceId);

            if (null != purchaseInvoice)
            {
                isDeleted = await Delete(purchaseInvoice);
            }

            return isDeleted; // returns.
        }

        public async Task<bool> UpdatePurchaseInvoiceMasterAmount(int? purchaseInvoiceId)
        {
            bool isUpdated = false;

            // get record.
            Purchaseinvoice purchaseInvoice = await GetQueryByCondition(w => w.PurchaseInvoiceId == purchaseInvoiceId)
                                                    .Include(w => w.Purchaseinvoicedetails).Include(w => w.Purchaseinvoicetaxes)
                                                    .Include(w => w.Currency).FirstOrDefaultAsync();

            if (null != purchaseInvoice)
            {
                purchaseInvoice.TotalLineItemAmountFc = purchaseInvoice.Purchaseinvoicedetails.Sum(w => w.GrossAmountFc);
                purchaseInvoice.TotalLineItemAmount = purchaseInvoice.TotalLineItemAmountFc / purchaseInvoice.ExchangeRate;

                if (DiscountType.Percentage.ToString() == purchaseInvoice.DiscountPercentageOrAmount)
                {
                    purchaseInvoice.DiscountAmountFc = (purchaseInvoice.TotalLineItemAmountFc * purchaseInvoice.DiscountPerOrAmountFc) / 100;
                }
                else
                {
                    purchaseInvoice.DiscountAmountFc = purchaseInvoice.DiscountPerOrAmountFc;
                }

                purchaseInvoice.DiscountAmount = purchaseInvoice.DiscountAmountFc / purchaseInvoice.ExchangeRate;
                purchaseInvoice.GrossAmountFc = purchaseInvoice.TotalLineItemAmountFc - purchaseInvoice.DiscountAmountFc;
                purchaseInvoice.GrossAmount = purchaseInvoice.GrossAmountFc / purchaseInvoice.ExchangeRate;

                if (TaxModelType.LineWise.ToString() == purchaseInvoice.TaxModelType)
                {
                    purchaseInvoice.TaxAmountFc = purchaseInvoice.Purchaseinvoicedetails.Sum(w => w.TaxAmountFc);
                }
                else
                {
                    purchaseInvoice.TaxAmountFc = purchaseInvoice.Purchaseinvoicetaxes.Sum(w => w.TaxAmountFc);
                }

                purchaseInvoice.TaxAmount = purchaseInvoice.TaxAmountFc / purchaseInvoice.ExchangeRate;
                purchaseInvoice.NetAmountFc = purchaseInvoice.GrossAmountFc + purchaseInvoice.TaxAmountFc;
                purchaseInvoice.NetAmount = purchaseInvoice.NetAmountFc / purchaseInvoice.ExchangeRate;

                purchaseInvoice.NetAmountFcinWord = await common.AmountInWord_Million(purchaseInvoice.NetAmountFc.ToString(), purchaseInvoice.Currency.CurrencyCode, purchaseInvoice.Currency.Denomination);
                
                if(purchaseInvoice.StatusId == (int)DocumentStatus.Approved || purchaseInvoice.StatusId == (int)DocumentStatus.ApprovalRequested)
                {
                    purchaseInvoice.StatusId = (int)DocumentStatus.Inprocess;
                }

                isUpdated = await Update(purchaseInvoice);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// get purchase invoice based on purchaseInvoiceId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<PurchaseInvoiceModel> GetPurchaseInvoiceById(int purchaseInvoiceId)
        {
            PurchaseInvoiceModel purchaseInvoiceModel = null;

            IList<PurchaseInvoiceModel> purchaseInvoiceModelList = await GetPurchaseInvoiceList(purchaseInvoiceId);

            if (null != purchaseInvoiceModelList && purchaseInvoiceModelList.Any())
            {
                purchaseInvoiceModel = purchaseInvoiceModelList.FirstOrDefault();
            }

            return purchaseInvoiceModel; // returns.
        }

        /// <summary>
        /// get search purchase invoice result list.
        /// </summary>
        /// <param name="dataTableAjaxPostModel"></param>
        /// <param name="searchFilterModel"></param>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<DataTableResultModel<PurchaseInvoiceModel>> GetPurchaseInvoiceList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterPurchaseInvoiceModel searchFilterModel)
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
            DataTableResultModel<PurchaseInvoiceModel> resultModel = await GetDataFromDbase(searchFilterModel, searchBy, take, skip, sortBy, sortDir);

            return resultModel; // returns.
        }

        /// <summary>
        /// get purchase invoice List based on supplierLedgerId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<IList<OutstandingInvoiceModel>> GetPurchaseInvoiceListBySupplierLedgerId(int supplierLedgerId)
        {
            IList<OutstandingInvoiceModel> outstandingInvoiceModelList = null;

            // create query.
            IQueryable<Purchaseinvoice> query = GetQueryByCondition(w => w.PurchaseInvoiceId != 0);

            // apply filters.
            if (0 != supplierLedgerId)
                query = query.Where(w => w.SupplierLedgerId == supplierLedgerId);

            // get records by query.
            List<Purchaseinvoice> purchaseInvoiceList = await query.ToListAsync();

            outstandingInvoiceModelList = new List<OutstandingInvoiceModel>();

            if (null != purchaseInvoiceList && purchaseInvoiceList.Count > 0)
            {
                foreach (Purchaseinvoice purchaseInvoice in purchaseInvoiceList)
                {
                    outstandingInvoiceModelList.Add(new OutstandingInvoiceModel()
                    {
                        InvoiceId = purchaseInvoice.PurchaseInvoiceId,
                        InvoiceType = "Purchase Invoice",
                        InvoiceNo = purchaseInvoice.InvoiceNo,
                        InvoiceDate = purchaseInvoice.InvoiceDate,
                        InvoiceAmount = purchaseInvoice.NetAmount,
                        OutstandingAmount = purchaseInvoice.NetAmount,
                        PurchaseInvoiceId = purchaseInvoice.PurchaseInvoiceId,
                        SalesInvoiceId = 0,
                        CreditNoteId = 0,
                        DebitNoteId = 0
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
        private async Task<DataTableResultModel<PurchaseInvoiceModel>> GetDataFromDbase(SearchFilterPurchaseInvoiceModel searchFilterModel, string searchBy, int take, int skip, string sortBy, string sortDir)
        {
            DataTableResultModel<PurchaseInvoiceModel> resultModel = new DataTableResultModel<PurchaseInvoiceModel>();

            IQueryable<Purchaseinvoice> query = GetQueryByCondition(w => w.PurchaseInvoiceId != 0)
                                                .Include(w => w.SupplierLedger).Include(w => w.Currency)
                                                .Include(w=>w.PreparedByUser).Include(w=>w.Status);

           //sortBy
            if (string.IsNullOrEmpty(sortBy) || sortBy == "0")
            {
                sortBy = "InvoiceNo";
            }

            //sortDir
            if (string.IsNullOrEmpty(sortDir) || sortDir == "")
            {
                sortDir = "asc";
            }

            if (!string.IsNullOrEmpty(searchFilterModel.InvoiceNo))
            {
                query = query.Where(w => w.InvoiceNo.Contains(searchFilterModel.InvoiceNo));
            }

            if (null != searchFilterModel.SupplierLedgerId)
            {
                query = query.Where(w => w.SupplierLedgerId == searchFilterModel.SupplierLedgerId);
            }

            if (null != searchFilterModel.FromDate)
            {
                query = query.Where(w => w.InvoiceDate >= searchFilterModel.FromDate);
            }

            if (null != searchFilterModel.ToDate)
            {
                query = query.Where(w => w.InvoiceDate <= searchFilterModel.ToDate);
            }

            if (!string.IsNullOrEmpty(searchFilterModel.SupplierReferenceNo))
            {
                query = query.Where(w => w.SupplierReferenceNo.Contains(searchFilterModel.SupplierReferenceNo));
            }
            
            if (null != searchFilterModel.AccountLedgerId)
            {
                query = query.Where(w => w.AccountLedgerId == searchFilterModel.AccountLedgerId);
            }

            // get total count.
            resultModel.TotalResultCount = await query.CountAsync();

            // datatable search
            if (!string.IsNullOrEmpty(searchBy))
            {
                query = query.Where(w => w.InvoiceNo.ToLower().Contains(searchBy.ToLower()));
            }

            // get records based on pagesize.
            query = query.Skip(skip).Take(take);

            resultModel.ResultList = await query.Select(s => new PurchaseInvoiceModel
            {
                PurchaseInvoiceId = s.PurchaseInvoiceId,
                InvoiceNo = s.InvoiceNo,
                InvoiceDate = s.InvoiceDate,
                NetAmountFc = s.NetAmountFc,
                SupplierReferenceNo = s.SupplierReferenceNo,
                SupplierReferenceDate = s.SupplierReferenceDate,
                SupplierLedgerName=s.SupplierLedger.LedgerName,
                CurrencyCode=s.Currency.CurrencyCode,
                PreparedByName=s.Currency.PreparedByUser.UserName,
                StatusName=s.Status.StatusName,
            }).OrderBy($"{sortBy} {sortDir}").ToListAsync();

            // get filter record count.
            resultModel.FilterResultCount = await query.CountAsync();

            return resultModel; // returns.
        }

        private async Task<IList<PurchaseInvoiceModel>> GetPurchaseInvoiceList(int purchaseInvoiceId)
        {
            IList<PurchaseInvoiceModel> purchaseInvoiceModelList = null;

            // create query.
            IQueryable<Purchaseinvoice> query = GetQueryByCondition(w => w.PurchaseInvoiceId != 0)
                                            .Include(w => w.SupplierLedger).Include(w => w.BillToAddress)
                                            .Include(w => w.AccountLedger)
                                            .Include(w => w.TaxRegister).Include(w => w.Currency)
                                            .Include(w => w.Status).Include(w => w.PreparedByUser);

            // apply filters.
            if (0 != purchaseInvoiceId)
                query = query.Where(w => w.PurchaseInvoiceId == purchaseInvoiceId);

            // get records by query.
            List<Purchaseinvoice> purchaseInvoiceList = await query.ToListAsync();

            if (null != purchaseInvoiceList && purchaseInvoiceList.Count > 0)
            {
                purchaseInvoiceModelList = new List<PurchaseInvoiceModel>();

                foreach (Purchaseinvoice purchaseInvoice in purchaseInvoiceList)
                {
                    purchaseInvoiceModelList.Add(await AssignValueToModel(purchaseInvoice));
                }
            }

            return purchaseInvoiceModelList; // returns.
        }

        private async Task<PurchaseInvoiceModel> AssignValueToModel(Purchaseinvoice purchaseInvoice)
        {
            return await Task.Run(() =>
            {
                PurchaseInvoiceModel purchaseInvoiceModel = new PurchaseInvoiceModel();

                purchaseInvoiceModel.PurchaseInvoiceId = purchaseInvoice.PurchaseInvoiceId;
                purchaseInvoiceModel.InvoiceNo = purchaseInvoice.InvoiceNo;
                purchaseInvoiceModel.InvoiceDate = purchaseInvoice.InvoiceDate;
                purchaseInvoiceModel.SupplierLedgerId = purchaseInvoice.SupplierLedgerId;
                purchaseInvoiceModel.BillToAddressId = purchaseInvoice.BillToAddressId;
                purchaseInvoiceModel.AccountLedgerId = purchaseInvoice.AccountLedgerId;
                purchaseInvoiceModel.SupplierReferenceNo = purchaseInvoice.SupplierReferenceNo;
                purchaseInvoiceModel.SupplierReferenceDate = purchaseInvoice.SupplierReferenceDate;
                purchaseInvoiceModel.CreditLimitDays = purchaseInvoice.CreditLimitDays;
                purchaseInvoiceModel.PaymentTerm = purchaseInvoice.PaymentTerm;
                purchaseInvoiceModel.Remark = purchaseInvoice.Remark;
                purchaseInvoiceModel.TaxModelType = purchaseInvoice.TaxModelType;
                purchaseInvoiceModel.TaxRegisterId = purchaseInvoice.TaxRegisterId;
                purchaseInvoiceModel.CurrencyId = purchaseInvoice.CurrencyId;
                purchaseInvoiceModel.ExchangeRate = purchaseInvoice.ExchangeRate;
                purchaseInvoiceModel.TotalLineItemAmountFc = purchaseInvoice.TotalLineItemAmountFc;
                purchaseInvoiceModel.TotalLineItemAmount = purchaseInvoice.TotalLineItemAmount;
                purchaseInvoiceModel.GrossAmountFc = purchaseInvoice.GrossAmountFc;
                purchaseInvoiceModel.GrossAmount = purchaseInvoice.GrossAmount;
                purchaseInvoiceModel.NetAmountFc = purchaseInvoice.NetAmountFc;
                purchaseInvoiceModel.NetAmount = purchaseInvoice.NetAmount;
                purchaseInvoiceModel.NetAmountFcInWord = purchaseInvoice.NetAmountFcinWord;
                purchaseInvoiceModel.TaxAmountFc = purchaseInvoice.TaxAmountFc;
                purchaseInvoiceModel.TaxAmount = purchaseInvoice.TaxAmount;
                purchaseInvoiceModel.DiscountPercentageOrAmount = purchaseInvoice.DiscountPercentageOrAmount;
                purchaseInvoiceModel.DiscountPerOrAmountFc = purchaseInvoice.DiscountPerOrAmountFc;
                purchaseInvoiceModel.DiscountAmountFc = purchaseInvoice.DiscountAmountFc;
                purchaseInvoiceModel.DiscountAmount = purchaseInvoice.DiscountAmount;
                purchaseInvoiceModel.StatusId = purchaseInvoice.StatusId;
                purchaseInvoiceModel.CompanyId = Convert.ToInt32(purchaseInvoice.CompanyId);
                purchaseInvoiceModel.FinancialYearId = Convert.ToInt32(purchaseInvoice.FinancialYearId);
                purchaseInvoiceModel.MaxNo = purchaseInvoice.MaxNo;
                purchaseInvoiceModel.VoucherStyleId = purchaseInvoice.VoucherStyleId;
                //purchaseInvoiceModel.PreparedByUserId = purchaseInvoice.PreparedByUserId;
                //purchaseInvoiceModel.UpdatedByUserId = purchaseInvoice.UpdatedByUserId;
                //purchaseInvoiceModel.PreparedDateTime = purchaseInvoice.PreparedDateTime;
                //purchaseInvoiceModel.UpdatedDateTime = purchaseInvoice.UpdatedDateTime;

                // ###
                purchaseInvoiceModel.SupplierLedgerName = null != purchaseInvoice.SupplierLedger ? purchaseInvoice.SupplierLedger.LedgerName : null;
                purchaseInvoiceModel.BillToAddress = null != purchaseInvoice.BillToAddress ? purchaseInvoice.BillToAddress.AddressDescription : null;
                purchaseInvoiceModel.AccountLedgerName = null != purchaseInvoice.AccountLedger ? purchaseInvoice.AccountLedger.LedgerName : null;
                purchaseInvoiceModel.TaxRegisterName = null != purchaseInvoice.TaxRegister ? purchaseInvoice.TaxRegister.TaxRegisterName : null;
                purchaseInvoiceModel.CurrencyCode = null != purchaseInvoice.Currency ? purchaseInvoice.Currency.CurrencyCode : null;
                purchaseInvoiceModel.StatusName = null != purchaseInvoice.Status ? purchaseInvoice.Status.StatusName : null;
                purchaseInvoiceModel.PreparedByName = null != purchaseInvoice.PreparedByUser ? purchaseInvoice.PreparedByUser.UserName : null;

                return purchaseInvoiceModel;
            });

        }

        #endregion Private Methods
    }
}
