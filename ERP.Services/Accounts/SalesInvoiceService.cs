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
    public class SalesInvoiceService : Repository<Salesinvoice>, ISalesInvoice
    {
        public SalesInvoiceService(ErpDbContext dbContext) : base(dbContext) { }

        /// <summary>
        /// create new sales invoice.
        /// </summary>
        /// <param name="salesInvoiceModel"></param>
        /// <returns>
        /// return id.
        /// </returns>
        public async Task<int> CreateSalesInvoice(SalesInvoiceModel salesInvoiceModel)
        {
            int salesInvoiceId = 0;

            // assign values.
            Salesinvoice salesInvoice = new Salesinvoice();

            salesInvoice.InvoiceId = salesInvoiceModel.InvoiceId;
            salesInvoice.InvoiceNo = salesInvoiceModel.InvoiceNo;
            salesInvoice.InvoiceDate = salesInvoiceModel.InvoiceDate;
            salesInvoice.CustomerLedgerId = salesInvoiceModel.CustomerLedgerId;
            salesInvoice.BillToAddressId = salesInvoiceModel.BillToAddressId;
            salesInvoice.AccountLedgerId = salesInvoiceModel.AccountLedgerId;
            salesInvoice.BankLedgerId = salesInvoiceModel.BankLedgerId;
            salesInvoice.CustomerReferenceNo = salesInvoiceModel.CustomerReferenceNo;
            salesInvoice.CustomerReferenceDate = salesInvoiceModel.CustomerReferenceDate;
            salesInvoice.CreditLimitDays = salesInvoiceModel.CreditLimitDays;
            salesInvoice.PaymentTerm = salesInvoiceModel.PaymentTerm;
            salesInvoice.Remark = salesInvoiceModel.Remark;
            salesInvoice.TaxModelType = salesInvoiceModel.TaxModelType;
            salesInvoice.TaxRegisterId = salesInvoiceModel.TaxRegisterId;
            salesInvoice.CurrencyId = salesInvoiceModel.CurrencyId;
            salesInvoice.ExchangeRate = salesInvoiceModel.ExchangeRate;
            salesInvoice.TotalLineItemAmountFc = 0;
            salesInvoice.TotalLineItemAmount = 0;
            salesInvoice.GrossAmountFc = 0;
            salesInvoice.GrossAmount = 0;
            salesInvoice.NetAmountFc = 0;
            salesInvoice.NetAmount = 0;
            salesInvoice.NetAmountFcinWord = "";
            salesInvoice.TaxAmountFc = 0;
            salesInvoice.TaxAmount = 0;

            salesInvoice.DiscountPercentageOrAmount = salesInvoiceModel.DiscountPercentageOrAmount;
            salesInvoice.DiscountPercentage = salesInvoiceModel.DiscountPercentage;
            salesInvoice.DiscountAmountFc = 0;
            salesInvoice.DiscountAmount = 0;
            salesInvoice.StatusId = salesInvoiceModel.StatusId;
            salesInvoice.CompanyId = salesInvoiceModel.CompanyId;
            salesInvoice.FinancialYearId = salesInvoiceModel.FinancialYearId;
            salesInvoice.MaxNo = salesInvoiceModel.MaxNo;

            //salesInvoice.VoucherStyleId = salesInvoiceModel.VoucherStyleId;
            //salesInvoice.PreparedByUserId = salesInvoiceModel.PreparedByUserId;
            //salesInvoice.UpdatedByUserId = salesInvoiceModel.UpdatedByUserId;
            //salesInvoice.PreparedDateTime = salesInvoiceModel.PreparedDateTime;
            //salesInvoice.UpdatedDateTime = salesInvoiceModel.UpdatedDateTime;

            salesInvoiceId = await Create(salesInvoice);

            return salesInvoiceId; // returns.
        }

        /// <summary>
        /// update sales invoice.
        /// </summary>
        /// <param name="salesInvoiceModel"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> UpdateSalesInvoice(SalesInvoiceModel salesInvoiceModel)
        {
            bool isUpdated = false;

            // get record.
            Salesinvoice salesInvoice = await GetByIdAsync(w => w.InvoiceId == salesInvoiceModel.InvoiceId);
            if (null != salesInvoice)
            {
                // assign values.
                salesInvoice.InvoiceId = salesInvoiceModel.InvoiceId;
                salesInvoice.InvoiceNo = salesInvoiceModel.InvoiceNo;
                salesInvoice.InvoiceDate = salesInvoiceModel.InvoiceDate;
                salesInvoice.CustomerLedgerId = salesInvoiceModel.CustomerLedgerId;
                salesInvoice.BillToAddressId = salesInvoiceModel.BillToAddressId;
                salesInvoice.AccountLedgerId = salesInvoiceModel.AccountLedgerId;
                salesInvoice.BankLedgerId = salesInvoiceModel.BankLedgerId;
                salesInvoice.CustomerReferenceNo = salesInvoiceModel.CustomerReferenceNo;
                salesInvoice.CustomerReferenceDate = salesInvoiceModel.CustomerReferenceDate;
                salesInvoice.CreditLimitDays = salesInvoiceModel.CreditLimitDays;
                salesInvoice.PaymentTerm = salesInvoiceModel.PaymentTerm;
                salesInvoice.Remark = salesInvoiceModel.Remark;
                salesInvoice.TaxModelType = salesInvoiceModel.TaxModelType;
                salesInvoice.TaxRegisterId = salesInvoiceModel.TaxRegisterId;
                salesInvoice.CurrencyId = salesInvoiceModel.CurrencyId;
                salesInvoice.ExchangeRate = salesInvoiceModel.ExchangeRate;

                //salesInvoice.TotalLineItemAmountFc = 0;
                //salesInvoice.TotalLineItemAmount = 0;
                //salesInvoice.GrossAmountFc = 0;
                //salesInvoice.GrossAmount = 0;
                //salesInvoice.NetAmountFc = 0;
                //salesInvoice.NetAmount = 0;
                //salesInvoice.NetAmountFcinWord = "";
                //salesInvoice.TaxAmountFc = 0;
                //salesInvoice.TaxAmount = 0;
                //salesInvoice.ChargeAmountFc = 0;
                //salesInvoice.ChargeAmount = 0;

                salesInvoice.DiscountPercentageOrAmount = salesInvoiceModel.DiscountPercentageOrAmount;
                salesInvoice.DiscountPercentage = salesInvoiceModel.DiscountPercentage;

                //salesInvoice.DiscountAmountFc = 0;
                //salesInvoice.DiscountAmount = 0;

                //salesInvoice.StatusId = salesInvoiceModel.StatusId;
                salesInvoice.CompanyId = salesInvoiceModel.CompanyId;

                //salesInvoice.FinancialYearId = salesInvoiceModel.FinancialYearId;
                //salesInvoice.MaxNo = salesInvoiceModel.MaxNo;

                isUpdated = await Update(salesInvoice);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// delete sales invoice.
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> DeleteSalesInvoice(int invoiceId)
        {
            bool isDeleted = false;

            // get record.
            Salesinvoice salesInvoice = await GetByIdAsync(w => w.InvoiceId == invoiceId);
            if (null != salesInvoice)
            {
                isDeleted = await Delete(salesInvoice);
            }

            return isDeleted; // returns.
        }

        /// <summary>
        /// get sales invoice based on invoiceId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<SalesInvoiceModel> GetSalesInvoiceById(int invoiceId)
        {
            SalesInvoiceModel salesInvoiceModel = null;

            IList<SalesInvoiceModel> salesInvoiceModelList = await GetSalesInvoiceList(invoiceId);

            if (null != salesInvoiceModelList && salesInvoiceModelList.Any())
            {
                salesInvoiceModel = salesInvoiceModelList.FirstOrDefault();
            }

            return salesInvoiceModel; // returns.
        }

        public async Task<DataTableResultModel<SalesInvoiceModel>> GetSalesInvoiceList()
        {
            DataTableResultModel<SalesInvoiceModel> salesInvoiceModel = new DataTableResultModel<SalesInvoiceModel>();

            IList<SalesInvoiceModel> salesInvoiceModelList = await GetSalesInvoiceList(0);

            if (null != salesInvoiceModelList && salesInvoiceModelList.Any())
            {
                salesInvoiceModel = new DataTableResultModel<SalesInvoiceModel>();
                salesInvoiceModel.ResultList = salesInvoiceModelList;
                salesInvoiceModel.TotalResultCount = salesInvoiceModelList.Count();
            }

            return salesInvoiceModel; // returns.
        }

        private async Task<IList<SalesInvoiceModel>> GetSalesInvoiceList(int salesInvoiceId)
        {
            IList<SalesInvoiceModel> salesInvoiceModelList = null;

            // create query.
            IQueryable<Salesinvoice> query = GetQueryByCondition(w => w.InvoiceId != 0)
                                            .Include(w => w.CustomerLedger).Include(w => w.BillToAddress)
                                            .Include(w => w.AccountLedger).Include(w => w.BankLedger)
                                            .Include(w => w.TaxRegister).Include(w => w.Currency)
                                            .Include(w => w.Status).Include(w => w.PreparedByUser);

            // apply filters.
            if (0 != salesInvoiceId)
                query = query.Where(w => w.InvoiceId == salesInvoiceId);

            // get records by query.
            List<Salesinvoice> salesInvoiceList = await query.ToListAsync();

            if (null != salesInvoiceList && salesInvoiceList.Count > 0)
            {
                salesInvoiceModelList = new List<SalesInvoiceModel>();

                foreach (Salesinvoice salesInvoice in salesInvoiceList)
                {
                    salesInvoiceModelList.Add(await AssignValueToModel(salesInvoice));
                }
            }

            return salesInvoiceModelList; // returns.
        }

        private async Task<SalesInvoiceModel> AssignValueToModel(Salesinvoice salesInvoice)
        {
            return await Task.Run(() =>
            {
                SalesInvoiceModel salesInvoiceModel = new SalesInvoiceModel();

                salesInvoiceModel.InvoiceId = salesInvoice.InvoiceId;
                salesInvoiceModel.InvoiceNo = salesInvoice.InvoiceNo;
                salesInvoiceModel.InvoiceDate = salesInvoice.InvoiceDate;
                salesInvoiceModel.CustomerLedgerId = salesInvoice.CustomerLedgerId;
                salesInvoiceModel.BillToAddressId = salesInvoice.BillToAddressId;
                salesInvoiceModel.AccountLedgerId = salesInvoice.AccountLedgerId;
                salesInvoiceModel.BankLedgerId = salesInvoice.BankLedgerId;
                salesInvoiceModel.CustomerReferenceNo = salesInvoice.CustomerReferenceNo;
                salesInvoiceModel.CustomerReferenceDate = salesInvoice.CustomerReferenceDate;
                salesInvoiceModel.CreditLimitDays = salesInvoice.CreditLimitDays;
                salesInvoiceModel.PaymentTerm = salesInvoice.PaymentTerm;
                salesInvoiceModel.Remark = salesInvoice.Remark;
                salesInvoiceModel.TaxModelType = salesInvoice.TaxModelType;
                salesInvoiceModel.TaxRegisterId = salesInvoice.TaxRegisterId;
                salesInvoiceModel.CurrencyId = salesInvoice.CurrencyId;
                salesInvoiceModel.ExchangeRate = salesInvoice.ExchangeRate;
                salesInvoiceModel.TotalLineItemAmountFc = salesInvoice.TotalLineItemAmountFc;
                salesInvoiceModel.TotalLineItemAmount = salesInvoice.TotalLineItemAmount;
                salesInvoiceModel.GrossAmountFc = salesInvoice.GrossAmountFc;
                salesInvoiceModel.GrossAmount = salesInvoice.GrossAmount;
                salesInvoiceModel.NetAmountFc = salesInvoice.NetAmountFc;
                salesInvoiceModel.NetAmount = salesInvoice.NetAmount;
                salesInvoiceModel.NetAmountFcinWord = salesInvoice.NetAmountFcinWord;
                salesInvoiceModel.TaxAmountFc = salesInvoice.TaxAmountFc;
                salesInvoiceModel.TaxAmount = salesInvoice.TaxAmount;
                salesInvoiceModel.DiscountPercentageOrAmount = salesInvoice.DiscountPercentageOrAmount;
                salesInvoiceModel.DiscountPercentage = salesInvoice.DiscountPercentage;
                salesInvoiceModel.DiscountAmountFc = salesInvoice.DiscountAmountFc;
                salesInvoiceModel.DiscountAmount = salesInvoice.DiscountAmount;
                salesInvoiceModel.StatusId = salesInvoice.StatusId;
                salesInvoiceModel.CompanyId = salesInvoice.CompanyId;
                salesInvoiceModel.FinancialYearId = salesInvoice.FinancialYearId;
                salesInvoiceModel.MaxNo = salesInvoice.MaxNo;
                salesInvoiceModel.VoucherStyleId = salesInvoice.VoucherStyleId;
                salesInvoiceModel.PreparedByUserId = salesInvoice.PreparedByUserId;
                salesInvoiceModel.UpdatedByUserId = salesInvoice.UpdatedByUserId;
                salesInvoiceModel.PreparedDateTime = salesInvoice.PreparedDateTime;
                salesInvoiceModel.UpdatedDateTime = salesInvoice.UpdatedDateTime;

                salesInvoiceModel.CustomerLedgerName = salesInvoice.CustomerLedger.LedgerName;
                salesInvoiceModel.BillToAddress = salesInvoice.BillToAddress.AddressDescription;
                salesInvoiceModel.AccountLedgerName = salesInvoice.AccountLedger.LedgerName;
                salesInvoiceModel.BankLedgerName = salesInvoice.BankLedger.LedgerName;
                salesInvoiceModel.TaxRegisterName = salesInvoice.TaxRegister.TaxRegisterName;
                salesInvoiceModel.CurrencyName = salesInvoice.Currency.CurrencyName;
                salesInvoiceModel.StatusName = salesInvoice.Status.StatusName;
                salesInvoiceModel.PreparedByName = salesInvoice.PreparedByUser.UserName;

                return salesInvoiceModel;
            });

        }

    }
}
