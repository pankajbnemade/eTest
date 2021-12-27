﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ERP.DataAccess.EntityModels
{
    public partial class Purchaseinvoice
    {
        public Purchaseinvoice()
        {
            Advanceadjustmentdetails = new HashSet<Advanceadjustmentdetail>();
            Journalvoucherdetails = new HashSet<Journalvoucherdetail>();
            Paymentvoucherdetails = new HashSet<Paymentvoucherdetail>();
            Purchaseinvoicedetails = new HashSet<Purchaseinvoicedetail>();
            Purchaseinvoicetaxes = new HashSet<Purchaseinvoicetax>();
        }

        public int PurchaseInvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int SupplierLedgerId { get; set; }
        public int BillToAddressId { get; set; }
        public int AccountLedgerId { get; set; }
        public string SupplierReferenceNo { get; set; }
        public DateTime? SupplierReferenceDate { get; set; }
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
        public string DiscountPercentageOrAmount { get; set; }
        public decimal DiscountPerOrAmountFc { get; set; }
        public decimal DiscountAmountFc { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmountFc { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal NetAmountFc { get; set; }
        public decimal NetAmount { get; set; }
        public string NetAmountFcinWord { get; set; }
        public int StatusId { get; set; }
        public int CompanyId { get; set; }
        public int FinancialYearId { get; set; }
        public int MaxNo { get; set; }
        public int VoucherStyleId { get; set; }
        public int PreparedByUserId { get; set; }
        public DateTime? PreparedDateTime { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }

        public virtual Ledger AccountLedger { get; set; }
        public virtual Ledgeraddress BillToAddress { get; set; }
        public virtual Company Company { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual Financialyear FinancialYear { get; set; }
        public virtual Aspnetuser PreparedByUser { get; set; }
        public virtual Status Status { get; set; }
        public virtual Ledger SupplierLedger { get; set; }
        public virtual Taxregister TaxRegister { get; set; }
        public virtual Aspnetuser UpdatedByUser { get; set; }
        public virtual Voucherstyle VoucherStyle { get; set; }
        public virtual ICollection<Advanceadjustmentdetail> Advanceadjustmentdetails { get; set; }
        public virtual ICollection<Journalvoucherdetail> Journalvoucherdetails { get; set; }
        public virtual ICollection<Paymentvoucherdetail> Paymentvoucherdetails { get; set; }
        public virtual ICollection<Purchaseinvoicedetail> Purchaseinvoicedetails { get; set; }
        public virtual ICollection<Purchaseinvoicetax> Purchaseinvoicetaxes { get; set; }
    }
}