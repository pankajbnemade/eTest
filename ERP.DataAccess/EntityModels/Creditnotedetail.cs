﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace ERP.DataAccess.EntityModels
{
    public partial class Creditnotedetail
    {
        public Creditnotedetail()
        {
            Creditnotedetailtaxes = new HashSet<Creditnotedetailtax>();
        }

        public int CreditNoteDetId { get; set; }
        public int CreditNoteId { get; set; }
        public int SrNo { get; set; }
        public string Description { get; set; }
        public int UnitOfMeasurementId { get; set; }
        public decimal Quantity { get; set; }
        public int PerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal GrossAmountFc { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal TaxAmountFc { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal NetAmountFc { get; set; }
        public decimal NetAmount { get; set; }
        public int PreparedByUserId { get; set; }
        public DateTime? PreparedDateTime { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }

        public virtual Creditnote CreditNote { get; set; }
        public virtual Aspnetuser PreparedByUser { get; set; }
        public virtual Unitofmeasurement UnitOfMeasurement { get; set; }
        public virtual Aspnetuser UpdatedByUser { get; set; }
        public virtual ICollection<Creditnotedetailtax> Creditnotedetailtaxes { get; set; }
    }
}