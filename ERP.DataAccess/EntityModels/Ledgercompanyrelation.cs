// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ERP.DataAccess.EntityModels
{
    public partial class Ledgercompanyrelation
    {
        public int RelationId { get; set; }
        public int CompanyId { get; set; }
        public int LedgerId { get; set; }
        public int PreparedByUserId { get; set; }
        public DateTime? PreparedDateTime { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }

        public virtual Company Company { get; set; }
        public virtual Ledger Ledger { get; set; }
        public virtual Aspnetuser PreparedByUser { get; set; }
        public virtual Aspnetuser UpdatedByUser { get; set; }
    }
}