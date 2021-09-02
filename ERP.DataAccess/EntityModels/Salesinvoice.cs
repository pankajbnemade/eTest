﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.DataAccess.EntityModels
{
    [Table("salesinvoice")]
    public partial class Salesinvoice
    {
        public Salesinvoice()
        {
            Advanceadjustmentdetails = new HashSet<Advanceadjustmentdetail>();
            Journalvoucherdetails = new HashSet<Journalvoucherdetail>();
            Receiptvoucherdetails = new HashSet<Receiptvoucherdetail>();
            Salesinvoicedetails = new HashSet<Salesinvoicedetail>();
            Salesinvoicetaxes = new HashSet<Salesinvoicetax>();
        }

        [Key]
        public int SalesInvoiceId { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string InvoiceNo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? InvoiceDate { get; set; }
        public int? CustomerLedgerId { get; set; }
        public int? BillToAddressId { get; set; }
        public int? AccountLedgerId { get; set; }
        public int? BankLedgerId { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string CustomerReferenceNo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CustomerReferenceDate { get; set; }
        public int? CreditLimitDays { get; set; }
        [Column(TypeName = "varchar(2000)")]
        public string PaymentTerm { get; set; }
        [Column(TypeName = "varchar(2000)")]
        public string Remark { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string TaxModelType { get; set; }
        public int? TaxRegisterId { get; set; }
        public int? CurrencyId { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal? ExchangeRate { get; set; }
        [Column("TotalLineItemAmount_FC", TypeName = "decimal(18,4)")]
        public decimal? TotalLineItemAmountFc { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? TotalLineItemAmount { get; set; }
        [Column("GrossAmount_FC", TypeName = "decimal(18,4)")]
        public decimal? GrossAmountFc { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? GrossAmount { get; set; }
        [Column("NetAmount_FC", TypeName = "decimal(18,4)")]
        public decimal? NetAmountFc { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? NetAmount { get; set; }
        [Column("NetAmount_FCInWord", TypeName = "varchar(2000)")]
        public string NetAmountFcinWord { get; set; }
        [Column("TaxAmount_FC", TypeName = "decimal(18,4)")]
        public decimal? TaxAmountFc { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? TaxAmount { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string DiscountPercentageOrAmount { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? DiscountPercentage { get; set; }
        [Column("DiscountAmount_FC", TypeName = "decimal(18,4)")]
        public decimal? DiscountAmountFc { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? DiscountAmount { get; set; }
        public int? StatusId { get; set; }
        public int? CompanyId { get; set; }
        public int? FinancialYearId { get; set; }
        public int? MaxNo { get; set; }
        public int? VoucherStyleId { get; set; }
        public int? PreparedByUserId { get; set; }
        public int? UpdatedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PreparedDateTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDateTime { get; set; }

        [ForeignKey(nameof(AccountLedgerId))]
        [InverseProperty(nameof(Ledger.SalesinvoiceAccountLedgers))]
        public virtual Ledger AccountLedger { get; set; }
        [ForeignKey(nameof(BankLedgerId))]
        [InverseProperty(nameof(Ledger.SalesinvoiceBankLedgers))]
        public virtual Ledger BankLedger { get; set; }
        [ForeignKey(nameof(BillToAddressId))]
        [InverseProperty(nameof(Ledgeraddress.Salesinvoices))]
        public virtual Ledgeraddress BillToAddress { get; set; }
        [ForeignKey(nameof(CompanyId))]
        [InverseProperty("Salesinvoices")]
        public virtual Company Company { get; set; }
        [ForeignKey(nameof(CurrencyId))]
        [InverseProperty("Salesinvoices")]
        public virtual Currency Currency { get; set; }
        [ForeignKey(nameof(CustomerLedgerId))]
        [InverseProperty(nameof(Ledger.SalesinvoiceCustomerLedgers))]
        public virtual Ledger CustomerLedger { get; set; }
        [ForeignKey(nameof(FinancialYearId))]
        [InverseProperty(nameof(Financialyear.Salesinvoices))]
        public virtual Financialyear FinancialYear { get; set; }
        [ForeignKey(nameof(PreparedByUserId))]
        [InverseProperty(nameof(Aspnetuser.SalesinvoicePreparedByUsers))]
        public virtual Aspnetuser PreparedByUser { get; set; }
        [ForeignKey(nameof(StatusId))]
        [InverseProperty("Salesinvoices")]
        public virtual Status Status { get; set; }
        [ForeignKey(nameof(TaxRegisterId))]
        [InverseProperty(nameof(Taxregister.Salesinvoices))]
        public virtual Taxregister TaxRegister { get; set; }
        [ForeignKey(nameof(UpdatedByUserId))]
        [InverseProperty(nameof(Aspnetuser.SalesinvoiceUpdatedByUsers))]
        public virtual Aspnetuser UpdatedByUser { get; set; }
        [ForeignKey(nameof(VoucherStyleId))]
        [InverseProperty(nameof(Voucherstyle.Salesinvoices))]
        public virtual Voucherstyle VoucherStyle { get; set; }
        [InverseProperty(nameof(Advanceadjustmentdetail.SalesInvoice))]
        public virtual ICollection<Advanceadjustmentdetail> Advanceadjustmentdetails { get; set; }
        [InverseProperty(nameof(Journalvoucherdetail.SalesInvoice))]
        public virtual ICollection<Journalvoucherdetail> Journalvoucherdetails { get; set; }
        [InverseProperty(nameof(Receiptvoucherdetail.SalesInvoice))]
        public virtual ICollection<Receiptvoucherdetail> Receiptvoucherdetails { get; set; }
        [InverseProperty(nameof(Salesinvoicedetail.SalesInvoice))]
        public virtual ICollection<Salesinvoicedetail> Salesinvoicedetails { get; set; }
        [InverseProperty(nameof(Salesinvoicetax.SalesInvoice))]
        public virtual ICollection<Salesinvoicetax> Salesinvoicetaxes { get; set; }
    }
}