﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using ERP.Models.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public partial class AdvanceAdjustmentVoucherDetailModel
    {
        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }

        [Display(Name = "Exchange Rate")]
        public decimal ExchangeRate { get; set; }
    }
}