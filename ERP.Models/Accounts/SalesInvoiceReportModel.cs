using ERP.Models.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class SalesInvoiceReportModel
    {
        public int SalesInvoiceId { get; set; }

        public string InvoiceNo { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public int CustomerLedgerId { get; set; }

        public int BillToAddressId { get; set; }

        public int AccountLedgerId { get; set; }

        public int BankLedgerId { get; set; }

        public string CustomerReferenceNo { get; set; }

        public DateTime? CustomerReferenceDate { get; set; }

        public int CreditLimitDays { get; set; }

        public string PaymentTerm { get; set; }

        public string Remark { get; set; }

        public string TaxModelType { get; set; }

        public int TaxRegisterId { get; set; }

        public int CurrencyId { get; set; }

        public decimal ExchangeRate { get; set; }

        public decimal TotalLineItemAmountFc { get; set; }

        public decimal TotalLineItemAmount { get; set; }

        public decimal GrossAmountFc { get; set; }

        public decimal GrossAmount { get; set; }

        public decimal NetAmountFc { get; set; }

        public decimal NetAmount { get; set; }

        public string NetAmountFcInWord { get; set; }

        public decimal TaxAmountFc { get; set; }

        public decimal TaxAmount { get; set; }

        public string DiscountPercentageOrAmount { get; set; }

        public decimal DiscountPerOrAmountFc { get; set; }

        public decimal DiscountAmountFc { get; set; }

        public decimal DiscountAmount { get; set; }

        public int StatusId { get; set; }
        public int CompanyId { get; set; }
        public int FinancialYearId { get; set; }
        public int MaxNo { get; set; }
        public int VoucherStyleId { get; set; }

        //####

        public string CustomerLedgerName { get; set; }
        public string CustomerBillToAddress { get; set; }
        public string CustomerBillToCountryName { get; set; }
        public string CustomerBillToStateName { get; set; }
        public string CustomerBillToCityName { get; set; }
        public string CustomerBillToEmailAddress { get; set; }
        public string CustomerBillToPhoneNo { get; set; }
        public string CustomerBillToPostalCode { get; set; }
        public string CustomerBillToFaxNo { get; set; }

        public string BankLedgerName { get; set; }
        public string CurrencyCode { get; set; }

        public string StatusName { get; set; }
        public string PreparedByName { get; set; }

        //'''# Detail

        public int SalesInvoiceDetId { get; set; }

        public int SrNo { get; set; }

        public string Description { get; set; }

        public int UnitOfMeasurementId { get; set; }

        public decimal Quantity { get; set; }

        public int PerUnit { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal GrossAmountFc_Det { get; set; }

        public decimal GrossAmount_Det { get; set; }

        public decimal TaxAmountFc_Det { get; set; }

        public decimal TaxAmount_Det { get; set; }

        public decimal NetAmountFc_Det { get; set; }

        public decimal NetAmount_Det { get; set; }

        public string UnitOfMeasurementName { get; set; }

        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyEmailAddress { get; set; }
        public string CompanyWebsite { get; set; }
        public string CompanyPhoneNo { get; set; }
        public string CompanyAlternatePhoneNo { get; set; }
        public string CompanyFaxNo { get; set; }
        public string CompanyPostalCode { get; set; }

    }
}
