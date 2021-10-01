﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using ERP.Models.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public partial class PaymentVoucherModel
    {
        public int PaymentVoucherId { get; set; }

        [Display(Name = "Voucher No")]
        [Required(ErrorMessage = "Vocuher No is required.")]
        public string VoucherNo { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Voucher Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Voucher Date is required.")]
        public DateTime? VoucherDate { get; set; }

        [Display(Name = "Account Ledger")]
        [Required(ErrorMessage = "Account Ledger is required.")]
        public int? AccountLedgerId { get; set; }

        [Display(Name = "Cash/Bank")]
        [Required(ErrorMessage = "Cash/Bank is required.")]
        public string TypeCorB { get; set; }

        [Display(Name = "Payment Type")]
        [Required(ErrorMessage = "Payment Type is required.")]
        public int? PaymentTypeId { get; set; }

        [Display(Name = "Currency")]
        [Required(ErrorMessage = "Currency is required.")]
        public int CurrencyId { get; set; }

        [Display(Name = "Exchange Rate")]
        [Required(ErrorMessage = "Exchange Rate is required.")]
        //[RegularExpression(RegexHelper.DecimalOnly, ErrorMessage = "Numbers only.")]
        [DisplayFormat(DataFormatString  = "{0:0.000000}", ApplyFormatInEditMode  = true)]
        public decimal ExchangeRate { get; set; }

        [Display(Name = "Cheque No")]
        [StringLength(250, ErrorMessage = "50 chars only.")]
        [Required(ErrorMessage = "Cheque No is required.")]
        public string ChequeNo { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Cheque Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Cheque Date is required.")]
        public DateTime? ChequeDate { get; set; }

        [Display(Name = "Cheque Amount FC")]
        [Required(ErrorMessage = "Cheque Amount FC is required.")]
        //[RegularExpression(RegexHelper.DecimalOnly, ErrorMessage = "Numbers only.")]
        public decimal? ChequeAmountFc { get; set; }

        [Display(Name = "Narration")]
        [StringLength(2000, ErrorMessage = "Narration cannot exceed 2000 characters.")]
        public string Narration { get; set; }

        [Display(Name = "Amount FC")]
        //[Required(ErrorMessage = "Amount FC is required.")]
        //[RegularExpression(RegexHelper.DecimalOnly, ErrorMessage = "Numbers only.")]
        public decimal? AmountFc { get; set; }

        [Display(Name = "Amount")]
        //[Required(ErrorMessage = "Amount is required.")]
        //[RegularExpression(RegexHelper.DecimalOnly, ErrorMessage = "Numbers only.")]
        public decimal? Amount { get; set; }

        [Display(Name = "Amount FC in word")]
        //[Required(ErrorMessage = "Amount FC in word is required.")]
        public string AmountFcinWord { get; set; }

        public int? StatusId { get; set; }
        public int CompanyId { get; set; }
        public int FinancialYearId { get; set; }
        public int? MaxNo { get; set; }
        public int? VoucherStyleId { get; set; }

        //####

        public string AccountLedgerName { get; set; }
        public string TypeCorBName { get; set; }
        public string PaymentTypeName { get; set; }
        public string CurrencyName { get; set; }
        public string StatusName { get; set; }
        public string PreparedByName { get; set; }

        [Display(Name = "Closing Balance")]
        public decimal? ClosingBalance { get; set; }

    }
}