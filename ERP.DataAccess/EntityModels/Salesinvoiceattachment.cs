﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ERP.DataAccess.EntityModels
{
    public partial class Salesinvoiceattachment
    {
        public int AssociationId { get; set; }
        public int SalesInvoiceId { get; set; }
        public int AttachmentId { get; set; }
        public int PreparedByUserId { get; set; }
        public DateTime? PreparedDateTime { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }

        public virtual Attachment Attachment { get; set; }
        public virtual Aspnetuser PreparedByUser { get; set; }
        public virtual Salesinvoice SalesInvoice { get; set; }
        public virtual Aspnetuser UpdatedByUser { get; set; }
    }
}