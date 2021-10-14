﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using ERP.Models.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public partial class JournalVoucherModel
    {
        public int JournalVoucherId { get; set; }

        [Display(Name = "Voucher No")]
        [Required(ErrorMessage = "Vocuher No is required.")]
        public string VoucherNo { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Voucher Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Voucher Date is required.")]
        public DateTime? VoucherDate { get; set; }

        [Display(Name = "Currency")]
        [Required(ErrorMessage = "Currency is required.")]
        public int CurrencyId { get; set; }

        [Display(Name = "Exchange Rate")]
        [Required(ErrorMessage = "Exchange Rate is required.")]
        //[RegularExpression(RegexHelper.DecimalOnly, ErrorMessage = "Numbers only.")]
        public decimal ExchangeRate { get; set; }

        [Display(Name = "Narration")]
        [StringLength(2000, ErrorMessage = "Narration cannot exceed 2000 characters.")]
        public string Narration { get; set; }

        [Display(Name = "Amount FC")]
        [Required(ErrorMessage = "Amount FC is required.")]
        //[RegularExpression(RegexHelper.DecimalOnly, ErrorMessage = "Numbers only.")]
        public decimal AmountFc { get; set; }

        [Display(Name = "Amount")]
        [Required(ErrorMessage = "Amount is required.")]
        [RegularExpression(RegexHelper.DecimalOnly, ErrorMessage = "Decimal only.")]
        public decimal Amount { get; set; }

        [Display(Name = "Amount FC in word")]
        [Required(ErrorMessage = "Amount FC in word is required.")]
        public string AmountFcinWord { get; set; }

        [Display(Name = "Debit Amount FC")]
        [Required(ErrorMessage = "Debit Amount FC is required.")]
        [RegularExpression(RegexHelper.DecimalOnly, ErrorMessage = "Decimal only.")]
        public decimal DebitAmountFc { get; set; }

        [Display(Name = "Debit Amount")]
        [Required(ErrorMessage = "Debit Amount is required.")]
        [RegularExpression(RegexHelper.DecimalOnly, ErrorMessage = "Decimal only.")]
        public decimal DebitAmount { get; set; }

        [Display(Name = "Credit Amount FC")]
        [Required(ErrorMessage = "Credit Amount FC is required.")]
        [RegularExpression(RegexHelper.DecimalOnly, ErrorMessage = "Decimal only.")]
        public decimal CreditAmountFc { get; set; }

        [Display(Name = "Credit Amount")]
        [Required(ErrorMessage = "Credit Amount is required.")]
        [RegularExpression(RegexHelper.DecimalOnly, ErrorMessage = "Decimal only.")]
        public decimal CreditAmount { get; set; }

        public int StatusId { get; set; }

        public int CompanyId { get; set; }

        public int FinancialYearId { get; set; }

        public int MaxNo { get; set; }

        public int VoucherStyleId { get; set; }

        //####

        public string CurrencyName { get; set; }
        public string StatusName { get; set; }
        public string PreparedByName { get; set; }

    }
}