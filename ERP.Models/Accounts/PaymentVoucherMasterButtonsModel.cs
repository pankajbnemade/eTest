﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using ERP.Models.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public partial class PaymentVoucherMasterButtonsModel
    {
        public int PaymentVoucherId { get; set; }
        public bool IsApprovalRequestVisible { get; set; }
        public bool IsApproveVisible { get; set; }
        public bool IsCancelVisible { get; set; }
    }
}