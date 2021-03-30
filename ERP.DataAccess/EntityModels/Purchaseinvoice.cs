﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.DataAccess.EntityModels
{
    [Table("purchaseinvoice")]
    public partial class Purchaseinvoice
    {
        public Purchaseinvoice()
        {
            Purchaseinvoicedetails = new HashSet<Purchaseinvoicedetail>();
            Purchaseinvoicetaxes = new HashSet<Purchaseinvoicetax>();
        }

        [Key]
        public int PurchaseInvoiceId { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string InvoiceNo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? InvoiceDate { get; set; }
        public int? SupplierLedgerId { get; set; }
        public int? BillToAddressId { get; set; }
        public int? AccountLedgerId { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string SupplierReferenceNo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? SupplierReferenceDate { get; set; }
        public int? CreditLimitDays { get; set; }
        [Column(TypeName = "varchar(2000)")]
        public string PaymentTerm { get; set; }
        [Column(TypeName = "varchar(2000)")]
        public string Remark { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string TaxModelType { get; set; }
        public int? TaxRegisterId { get; set; }
        public int CurrencyId { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal ExchangeRate { get; set; }
        [Column("TotalLineItemAmount_FC", TypeName = "decimal(18,4)")]
        public decimal? TotalLineItemAmountFc { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? TotalLineItemAmount { get; set; }
        [Column("GrossAmount_FC", TypeName = "decimal(18,4)")]
        public decimal? GrossAmountFc { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? GrossAmount { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string DiscountPercentageOrAmount { get; set; }
        [Column("DiscountPerOrAmount_FC", TypeName = "decimal(18,4)")]
        public decimal? DiscountPerOrAmountFc { get; set; }
        [Column("DiscountAmount_FC", TypeName = "decimal(18,4)")]
        public decimal? DiscountAmountFc { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? DiscountAmount { get; set; }
        [Column("TaxAmount_FC", TypeName = "decimal(18,4)")]
        public decimal? TaxAmountFc { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? TaxAmount { get; set; }
        [Column("NetAmount_FC", TypeName = "decimal(18,4)")]
        public decimal? NetAmountFc { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? NetAmount { get; set; }
        [Column("NetAmount_FCInWord", TypeName = "varchar(2000)")]
        public string NetAmountFcinWord { get; set; }
        public int? StatusId { get; set; }
        public int? CompanyId { get; set; }
        public int? FinancialYearId { get; set; }
        public int? MaxNo { get; set; }
        public int? VoucherStyleId { get; set; }
        public int? PreparedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PreparedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDateTime { get; set; }

        [ForeignKey(nameof(AccountLedgerId))]
        [InverseProperty(nameof(Ledger.PurchaseinvoiceAccountLedgers))]
        public virtual Ledger AccountLedger { get; set; }
        [ForeignKey(nameof(BillToAddressId))]
        [InverseProperty(nameof(Ledgeraddress.Purchaseinvoices))]
        public virtual Ledgeraddress BillToAddress { get; set; }
        [ForeignKey(nameof(CompanyId))]
        [InverseProperty("Purchaseinvoices")]
        public virtual Company Company { get; set; }
        [ForeignKey(nameof(CurrencyId))]
        [InverseProperty("Purchaseinvoices")]
        public virtual Currency Currency { get; set; }
        [ForeignKey(nameof(FinancialYearId))]
        [InverseProperty(nameof(Financialyear.Purchaseinvoices))]
        public virtual Financialyear FinancialYear { get; set; }
        [ForeignKey(nameof(PreparedByUserId))]
        [InverseProperty(nameof(User.PurchaseinvoicePreparedByUsers))]
        public virtual User PreparedByUser { get; set; }
        [ForeignKey(nameof(StatusId))]
        [InverseProperty("Purchaseinvoices")]
        public virtual Status Status { get; set; }
        [ForeignKey(nameof(SupplierLedgerId))]
        [InverseProperty(nameof(Ledger.PurchaseinvoiceSupplierLedgers))]
        public virtual Ledger SupplierLedger { get; set; }
        [ForeignKey(nameof(TaxRegisterId))]
        [InverseProperty(nameof(Taxregister.Purchaseinvoices))]
        public virtual Taxregister TaxRegister { get; set; }
        [ForeignKey(nameof(UpdatedByUserId))]
        [InverseProperty(nameof(User.PurchaseinvoiceUpdatedByUsers))]
        public virtual User UpdatedByUser { get; set; }
        [ForeignKey(nameof(VoucherStyleId))]
        [InverseProperty(nameof(Voucherstyle.Purchaseinvoices))]
        public virtual Voucherstyle VoucherStyle { get; set; }
        [InverseProperty(nameof(Purchaseinvoicedetail.PurchaseInvoice))]
        public virtual ICollection<Purchaseinvoicedetail> Purchaseinvoicedetails { get; set; }
        [InverseProperty(nameof(Purchaseinvoicetax.PurchaseInvoice))]
        public virtual ICollection<Purchaseinvoicetax> Purchaseinvoicetaxes { get; set; }
    }
}