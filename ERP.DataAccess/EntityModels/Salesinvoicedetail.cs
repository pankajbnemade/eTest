﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.DataAccess.EntityModels
{
    [Table("salesinvoicedetail")]
    public partial class Salesinvoicedetail
    {
        public Salesinvoicedetail()
        {
            Salesinvoicedetailtaxes = new HashSet<Salesinvoicedetailtax>();
        }

        [Key]
        public int InvoiceDetId { get; set; }
        public int? InvoiceId { get; set; }
        public int? SrNo { get; set; }
        [Column(TypeName = "varchar(2000)")]
        public string Description { get; set; }
        public int? UnitOfMeasurementId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Quantity { get; set; }
        public int? PerUnit { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? UnitPrice { get; set; }
        [Column("GrossAmount_FC", TypeName = "decimal(18,4)")]
        public decimal? GrossAmountFc { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? GrossAmount { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? TaxAmount { get; set; }
        [Column("TaxAmount_FC", TypeName = "decimal(18,4)")]
        public decimal? TaxAmountFc { get; set; }
        [Column("NetAmount_FC", TypeName = "decimal(18,4)")]
        public decimal? NetAmountFc { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? NetAmount { get; set; }
        public int? PreparedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PreparedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDateTime { get; set; }

        [ForeignKey(nameof(InvoiceId))]
        [InverseProperty(nameof(Salesinvoice.Salesinvoicedetails))]
        public virtual Salesinvoice Invoice { get; set; }
        [ForeignKey(nameof(PreparedByUserId))]
        [InverseProperty(nameof(Aspnetuser.SalesinvoicedetailPreparedByUsers))]
        public virtual Aspnetuser PreparedByUser { get; set; }
        [ForeignKey(nameof(UnitOfMeasurementId))]
        [InverseProperty(nameof(Unitofmeasurement.Salesinvoicedetails))]
        public virtual Unitofmeasurement UnitOfMeasurement { get; set; }
        [ForeignKey(nameof(UpdatedByUserId))]
        [InverseProperty(nameof(Aspnetuser.SalesinvoicedetailUpdatedByUsers))]
        public virtual Aspnetuser UpdatedByUser { get; set; }
        [InverseProperty(nameof(Salesinvoicedetailtax.InvoiceDet))]
        public virtual ICollection<Salesinvoicedetailtax> Salesinvoicedetailtaxes { get; set; }
    }
}