using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Master;
using ERP.Services.Accounts.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class PurchaseInvoiceService : Repository<Purchaseinvoice>, IPurchaseInvoice
    {
        public PurchaseInvoiceService(ErpDbContext dbContext) : base(dbContext) { }

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

            // assign values.
            Purchaseinvoice purchaseInvoice = new Purchaseinvoice();

            purchaseInvoice.InvoiceNo = purchaseInvoiceModel.InvoiceNo;
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
            purchaseInvoice.DiscountPercentageOrAmount = purchaseInvoiceModel.DiscountPercentageOrAmount;
            purchaseInvoice.DiscountPercentage = purchaseInvoiceModel.DiscountPercentage;
            purchaseInvoice.DiscountAmountFc = purchaseInvoiceModel.DiscountAmountFc;
            purchaseInvoice.DiscountAmount = 0;
            purchaseInvoice.TaxAmountFc = 0;
            purchaseInvoice.TaxAmount = 0;
            purchaseInvoice.NetAmountFc = 0;
            purchaseInvoice.NetAmount = 0;
            purchaseInvoice.NetAmountFcinWord = "";

            purchaseInvoice.StatusId = purchaseInvoiceModel.StatusId;
            purchaseInvoice.CompanyId = purchaseInvoiceModel.CompanyId;
            purchaseInvoice.FinancialYearId = purchaseInvoiceModel.FinancialYearId;
            purchaseInvoice.MaxNo = purchaseInvoiceModel.MaxNo;
            purchaseInvoice.VoucherStyleId = purchaseInvoiceModel.VoucherStyleId;

            purchaseInvoiceId = await Create(purchaseInvoice);

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
            Purchaseinvoice purchaseInvoice = await GetByIdAsync(w => w.InvoiceId == purchaseInvoiceModel.InvoiceId);
            if (null != purchaseInvoice)
            {
                // assign values.

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
                //purchaseInvoice.TotalLineItemAmountFc = 0;
                //purchaseInvoice.TotalLineItemAmount = 0;
                //purchaseInvoice.GrossAmountFc = 0;
                //purchaseInvoice.GrossAmount = 0;
                purchaseInvoice.DiscountPercentageOrAmount = purchaseInvoiceModel.DiscountPercentageOrAmount;
                purchaseInvoice.DiscountPercentage = purchaseInvoiceModel.DiscountPercentage;
                purchaseInvoice.DiscountAmountFc = purchaseInvoiceModel.DiscountAmountFc;
                //purchaseInvoice.DiscountAmount = 0;
                //purchaseInvoice.TaxAmountFc = 0;
                //purchaseInvoice.TaxAmount = 0;
                //purchaseInvoice.NetAmountFc = 0;
                //purchaseInvoice.NetAmount = 0;
                //purchaseInvoice.NetAmountFcinWord = "";

                //purchaseInvoice.StatusId = purchaseInvoiceModel.StatusId;
                //purchaseInvoice.CompanyId = purchaseInvoiceModel.CompanyId;
                //purchaseInvoice.FinancialYearId = purchaseInvoiceModel.FinancialYearId;
                //purchaseInvoice.MaxNo = purchaseInvoiceModel.MaxNo;
                //purchaseInvoice.VoucherStyleId = purchaseInvoiceModel.VoucherStyleId;

                isUpdated = await Update(purchaseInvoice);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// delete purchase invoice.
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> DeletePurchaseInvoice(int invoiceId)
        {
            bool isDeleted = false;

            // get record.
            Purchaseinvoice purchaseInvoice = await GetByIdAsync(w => w.InvoiceId == invoiceId);
            if (null != purchaseInvoice)
            {
                isDeleted = await Delete(purchaseInvoice);
            }

            return isDeleted; // returns.
        }

        /// <summary>
        /// get purchase invoice based on invoiceId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<PurchaseInvoiceModel> GetPurchaseInvoiceById(int invoiceId)
        {
            PurchaseInvoiceModel purchaseInvoiceModel = null;

            // get record.
            Purchaseinvoice purchaseinvoice = await GetByIdAsync(w => w.InvoiceId == invoiceId);
            if (null != purchaseInvoiceModel)
            {
                // assign values.
                purchaseInvoiceModel.InvoiceId = purchaseinvoice.InvoiceId;

            }

            return purchaseInvoiceModel; // returns.
        }

        public async Task<DataTableResultModel<PurchaseInvoiceModel>> GetPurchaseInvoiceList()
        {
            DataTableResultModel<PurchaseInvoiceModel> purchaseInvoiceModel = new DataTableResultModel<PurchaseInvoiceModel>();

            IList<PurchaseInvoiceModel> purchaseInvoiceModelList = await GetPurchaseInvoiceList(0);

            if (null != purchaseInvoiceModelList && purchaseInvoiceModelList.Any())
            {
                purchaseInvoiceModel = new DataTableResultModel<PurchaseInvoiceModel>();
                purchaseInvoiceModel.ResultList = purchaseInvoiceModelList;
                purchaseInvoiceModel.TotalResultCount = purchaseInvoiceModelList.Count();
            }

            return purchaseInvoiceModel; // returns.
        }

        private async Task<IList<PurchaseInvoiceModel>> GetPurchaseInvoiceList(int purchaseInvoiceId)
        {
            IList<PurchaseInvoiceModel> purchaseInvoiceModelList = null;

            // create query.
            IQueryable<Purchaseinvoice> query = GetQueryByCondition(w => w.InvoiceId != 0)
                                            .Include(w => w.SupplierLedger).Include(w => w.BillToAddress)
                                            .Include(w => w.AccountLedger)
                                            .Include(w => w.TaxRegister).Include(w => w.Currency)
                                            .Include(w => w.Status).Include(w => w.PreparedByUser);

            // apply filters.
            if (0 != purchaseInvoiceId)
                query = query.Where(w => w.InvoiceId == purchaseInvoiceId);

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

                purchaseInvoiceModel.InvoiceId = purchaseInvoice.InvoiceId;
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
                purchaseInvoiceModel.NetAmountFcinWord = purchaseInvoice.NetAmountFcinWord;
                purchaseInvoiceModel.TaxAmountFc = purchaseInvoice.TaxAmountFc;
                purchaseInvoiceModel.TaxAmount = purchaseInvoice.TaxAmount;
                purchaseInvoiceModel.DiscountPercentageOrAmount = purchaseInvoice.DiscountPercentageOrAmount;
                purchaseInvoiceModel.DiscountPercentage = purchaseInvoice.DiscountPercentage;
                purchaseInvoiceModel.DiscountAmountFc = purchaseInvoice.DiscountAmountFc;
                purchaseInvoiceModel.DiscountAmount = purchaseInvoice.DiscountAmount;
                purchaseInvoiceModel.StatusId = purchaseInvoice.StatusId;
                purchaseInvoiceModel.CompanyId = purchaseInvoice.CompanyId;
                purchaseInvoiceModel.FinancialYearId = purchaseInvoice.FinancialYearId;
                purchaseInvoiceModel.MaxNo = purchaseInvoice.MaxNo;
                purchaseInvoiceModel.VoucherStyleId = purchaseInvoice.VoucherStyleId;
                purchaseInvoiceModel.PreparedByUserId = purchaseInvoice.PreparedByUserId;
                purchaseInvoiceModel.UpdatedByUserId = purchaseInvoice.UpdatedByUserId;
                purchaseInvoiceModel.PreparedDateTime = purchaseInvoice.PreparedDateTime;
                purchaseInvoiceModel.UpdatedDateTime = purchaseInvoice.UpdatedDateTime;

                purchaseInvoiceModel.SupplierLedgerName = purchaseInvoice.SupplierLedger.LedgerName;
                purchaseInvoiceModel.BillToAddress = purchaseInvoice.BillToAddress.AddressDescription;
                purchaseInvoiceModel.AccountLedgerName = purchaseInvoice.AccountLedger.LedgerName;
                purchaseInvoiceModel.TaxRegisterName = purchaseInvoice.TaxRegister.TaxRegisterName;
                purchaseInvoiceModel.CurrencyName = purchaseInvoice.Currency.CurrencyName;
                purchaseInvoiceModel.StatusName = purchaseInvoice.Status.StatusName;
                purchaseInvoiceModel.PreparedByName = purchaseInvoice.PreparedByUser.UserName;

                return purchaseInvoiceModel;
            });

        }

    }
}
