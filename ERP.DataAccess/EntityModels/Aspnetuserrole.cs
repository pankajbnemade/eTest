﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ERP.DataAccess.EntityModels
{
    public partial class Aspnetuserrole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public virtual Aspnetrole Role { get; set; }
        public virtual Aspnetuser User { get; set; }
    }
}