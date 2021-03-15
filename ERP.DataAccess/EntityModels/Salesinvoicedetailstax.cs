﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.DataAccess.EntityModels
{
    [Table("salesinvoicedetailstax")]
    public partial class Salesinvoicedetailstax
    {
        [Key]
        public int InvoiceDetTaxId { get; set; }
        public int? InvoiceDetId { get; set; }
        public int? SrNo { get; set; }
        public int? TaxLedgerId { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string TaxPercentageOrAmount { get; set; }
        [Column("TaxPerOrAmount_FC", TypeName = "decimal(18,4)")]
        public decimal? TaxPerOrAmountFc { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string TaxAddOrDeduct { get; set; }
        [Column("TaxAmount_FC", TypeName = "decimal(18,4)")]
        public decimal? TaxAmountFc { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? TaxAmount { get; set; }
        [Column(TypeName = "varchar(2000)")]
        public string Remark { get; set; }
        public int? PreparedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PreparedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDateTime { get; set; }

        [ForeignKey(nameof(InvoiceDetId))]
        [InverseProperty(nameof(Salesinvoicedetail.Salesinvoicedetailstaxes))]
        public virtual Salesinvoicedetail InvoiceDet { get; set; }
        [ForeignKey(nameof(PreparedByUserId))]
        [InverseProperty(nameof(User.SalesinvoicedetailstaxPreparedByUsers))]
        public virtual User PreparedByUser { get; set; }
        [ForeignKey(nameof(TaxLedgerId))]
        [InverseProperty(nameof(Ledger.Salesinvoicedetailstaxes))]
        public virtual Ledger TaxLedger { get; set; }
        [ForeignKey(nameof(UpdatedByUserId))]
        [InverseProperty(nameof(User.SalesinvoicedetailstaxUpdatedByUsers))]
        public virtual User UpdatedByUser { get; set; }
    }
}