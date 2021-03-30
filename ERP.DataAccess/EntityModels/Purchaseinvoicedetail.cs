﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.DataAccess.EntityModels
{
    [Table("purchaseinvoicedetail")]
    public partial class Purchaseinvoicedetail
    {
        public Purchaseinvoicedetail()
        {
            Purchaseinvoicedetailtaxes = new HashSet<Purchaseinvoicedetailtax>();
        }

        [Key]
        public int PurchaseInvoiceDetId { get; set; }
        public int? PurchaseInvoiceId { get; set; }
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
        [Column("TaxAmount_FC", TypeName = "decimal(18,4)")]
        public decimal? TaxAmountFc { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? TaxAmount { get; set; }
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

        [ForeignKey(nameof(PreparedByUserId))]
        [InverseProperty(nameof(User.PurchaseinvoicedetailPreparedByUsers))]
        public virtual User PreparedByUser { get; set; }
        [ForeignKey(nameof(PurchaseInvoiceId))]
        [InverseProperty(nameof(Purchaseinvoice.Purchaseinvoicedetails))]
        public virtual Purchaseinvoice PurchaseInvoice { get; set; }
        [ForeignKey(nameof(UnitOfMeasurementId))]
        [InverseProperty(nameof(Unitofmeasurement.Purchaseinvoicedetails))]
        public virtual Unitofmeasurement UnitOfMeasurement { get; set; }
        [ForeignKey(nameof(UpdatedByUserId))]
        [InverseProperty(nameof(User.PurchaseinvoicedetailUpdatedByUsers))]
        public virtual User UpdatedByUser { get; set; }
        [InverseProperty(nameof(Purchaseinvoicedetailtax.PurchaseInvoiceDet))]
        public virtual ICollection<Purchaseinvoicedetailtax> Purchaseinvoicedetailtaxes { get; set; }
    }
}