﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using ERP.Models.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public partial class ContraVoucherMasterButtonsModel
    {
        public int ContraVoucherId { get; set; }
        public bool IsApprovalRequestVisible { get; set; }
        public bool IsApproveVisible { get; set; }
        public bool IsCancelVisible { get; set; }
    }
}