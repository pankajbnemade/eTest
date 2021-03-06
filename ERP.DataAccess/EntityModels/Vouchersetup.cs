// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ERP.DataAccess.EntityModels
{
    public partial class Vouchersetup
    {
        public Vouchersetup()
        {
            Vouchersetupdetails = new HashSet<Vouchersetupdetail>();
        }

        public int VoucherSetupId { get; set; }
        public string VoucherSetupName { get; set; }
        public int ModuleId { get; set; }
        public sbyte IsActive { get; set; }
        public int PreparedByUserId { get; set; }
        public DateTime? PreparedDateTime { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }

        public virtual Module Module { get; set; }
        public virtual Aspnetuser PreparedByUser { get; set; }
        public virtual Aspnetuser UpdatedByUser { get; set; }
        public virtual ICollection<Vouchersetupdetail> Vouchersetupdetails { get; set; }
    }
}