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

            // get record.
            Salesinvoice salesinvoice = await GetByIdAsync(w => w.InvoiceId == invoiceId);
            if (null != salesInvoiceModel)
            {
                // assign values.
                salesInvoiceModel.InvoiceId = salesinvoice.InvoiceId;
                salesInvoiceModel.InvoiceNo = salesinvoice.InvoiceNo;
                salesInvoiceModel.InvoiceDate = salesinvoice.InvoiceDate;
                salesInvoiceModel.CustomerLedgerId = salesinvoice.CustomerLedgerId;
                salesInvoiceModel.BillToAddressId = salesinvoice.BillToAddressId;
                salesInvoiceModel.AccountLedgerId = salesinvoice.AccountLedgerId;
                salesInvoiceModel.BankLedgerId = salesinvoice.BankLedgerId;
                salesInvoiceModel.CustomerReferenceNo = salesinvoice.CustomerReferenceNo;
                salesInvoiceModel.CustomerReferenceDate = salesinvoice.CustomerReferenceDate;
                salesInvoiceModel.CreditLimitDays = salesinvoice.CreditLimitDays;
                salesInvoiceModel.PaymentTerm = salesinvoice.PaymentTerm;
                salesInvoiceModel.Remark = salesinvoice.Remark;
                salesInvoiceModel.TaxModelType = salesinvoice.TaxModelType;
                salesInvoiceModel.TaxRegisterId = salesinvoice.TaxRegisterId;
                salesInvoiceModel.CurrencyId = salesinvoice.CurrencyId;
                salesInvoiceModel.ExchangeRate = salesinvoice.ExchangeRate;
                salesInvoiceModel.TotalLineItemAmountFc = salesinvoice.TotalLineItemAmountFc;
                salesInvoiceModel.TotalLineItemAmount = salesinvoice.TotalLineItemAmount;
                salesInvoiceModel.GrossAmountFc = salesinvoice.GrossAmountFc;
                salesInvoiceModel.GrossAmount = salesinvoice.GrossAmount;
                salesInvoiceModel.NetAmountFc = salesinvoice.NetAmountFc;
                salesInvoiceModel.NetAmount = salesinvoice.NetAmount;
                salesInvoiceModel.NetAmountFcinWord = salesinvoice.NetAmountFcinWord;
                salesInvoiceModel.TaxAmountFc = salesinvoice.TaxAmountFc;
                salesInvoiceModel.TaxAmount = salesinvoice.TaxAmount;
                
                salesInvoiceModel.DiscountPercentageOrAmount = salesinvoice.DiscountPercentageOrAmount;
                salesInvoiceModel.DiscountPercentage = salesinvoice.DiscountPercentage;
                salesInvoiceModel.DiscountAmountFc = salesinvoice.DiscountAmountFc;
                salesInvoiceModel.DiscountAmount = salesinvoice.DiscountAmount;
                salesInvoiceModel.StatusId = salesinvoice.StatusId;
                salesInvoiceModel.CompanyId = salesinvoice.CompanyId;
                salesInvoiceModel.FinancialYearId = salesinvoice.FinancialYearId;
                salesInvoiceModel.MaxNo = salesinvoice.MaxNo;
            }

            return salesInvoiceModel; // returns.
        }
    }
}
