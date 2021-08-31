﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.DataAccess.EntityModels
{
    [Table("debitnote")]
    public partial class Debitnote
    {
        public Debitnote()
        {
            Debitnotedetails = new HashSet<Debitnotedetail>();
            Debitnotetaxes = new HashSet<Debitnotetax>();
        }

        [Key]
        public int DebitNoteId { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string DebitNoteNo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DebitNoteDate { get; set; }
        public int? PartyLedgerId { get; set; }
        public int? BillToAddressId { get; set; }
        public int? AccountLedgerId { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string PartyReferenceNo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PartyReferenceDate { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string OurReferenceNo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? OurReferenceDate { get; set; }
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
        [InverseProperty(nameof(Ledger.DebitnoteAccountLedgers))]
        public virtual Ledger AccountLedger { get; set; }
        [ForeignKey(nameof(BillToAddressId))]
        [InverseProperty(nameof(Ledgeraddress.Debitnotes))]
        public virtual Ledgeraddress BillToAddress { get; set; }
        [ForeignKey(nameof(CompanyId))]
        [InverseProperty("Debitnotes")]
        public virtual Company Company { get; set; }
        [ForeignKey(nameof(CurrencyId))]
        [InverseProperty("Debitnotes")]
        public virtual Currency Currency { get; set; }
        [ForeignKey(nameof(FinancialYearId))]
        [InverseProperty(nameof(Financialyear.Debitnotes))]
        public virtual Financialyear FinancialYear { get; set; }
        [ForeignKey(nameof(PartyLedgerId))]
        [InverseProperty(nameof(Ledger.DebitnotePartyLedgers))]
        public virtual Ledger PartyLedger { get; set; }
        [ForeignKey(nameof(PreparedByUserId))]
        [InverseProperty(nameof(Aspnetuser.DebitnotePreparedByUsers))]
        public virtual Aspnetuser PreparedByUser { get; set; }
        [ForeignKey(nameof(StatusId))]
        [InverseProperty("Debitnotes")]
        public virtual Status Status { get; set; }
        [ForeignKey(nameof(TaxRegisterId))]
        [InverseProperty(nameof(Taxregister.Debitnotes))]
        public virtual Taxregister TaxRegister { get; set; }
        [ForeignKey(nameof(UpdatedByUserId))]
        [InverseProperty(nameof(Aspnetuser.DebitnoteUpdatedByUsers))]
        public virtual Aspnetuser UpdatedByUser { get; set; }
        [ForeignKey(nameof(VoucherStyleId))]
        [InverseProperty(nameof(Voucherstyle.Debitnotes))]
        public virtual Voucherstyle VoucherStyle { get; set; }
        [InverseProperty(nameof(Debitnotedetail.DebitNote))]
        public virtual ICollection<Debitnotedetail> Debitnotedetails { get; set; }
        [InverseProperty(nameof(Debitnotetax.DebitNote))]
        public virtual ICollection<Debitnotetax> Debitnotetaxes { get; set; }
    }
}