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

        [Display(Name = "Cash/Bank")]
        [Required(ErrorMessage = "Cash/Bank is required.")]
        public string TypeCorB { get; set; }
        
        [Display(Name = "Account")]
        [Required(ErrorMessage = "Account is required.")]
        public int AccountLedgerId { get; set; }

        [Display(Name = "Payment Type")]
        [Required(ErrorMessage = "Payment Type is required.")]
        public int PaymentTypeId { get; set; }

        [Display(Name = "Currency")]
        [Required(ErrorMessage = "Currency is required.")]
        public int CurrencyId { get; set; }

        [Display(Name = "Exchange Rate")]
        [Required(ErrorMessage = "Exchange Rate is required.")]
        [RegularExpression(RegexHelper.DecimalOnly6Digit, ErrorMessage = "Up to 6 Decimal only.")]
        [DisplayFormat(DataFormatString = "{0:0.000000}", ApplyFormatInEditMode = true)]
        public decimal ExchangeRate { get; set; }

        [Display(Name = "Cheque/Trans. No")]
        [StringLength(250, ErrorMessage = "50 chars only.")]
        [Required(ErrorMessage = "Cheque/Trans. No is required.")]
        public string ChequeNo { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Cheque/Trans. Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Cheque/Trans. Date is required.")]
        public DateTime? ChequeDate { get; set; }

        [Display(Name = "Cheque/Trans. Amount FC")]
        [Required(ErrorMessage = "Cheque/Trans. Amount FC is required.")]
        [RegularExpression(RegexHelper.DecimalOnly, ErrorMessage = "Decimal only.")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal ChequeAmountFc { get; set; }

        [Display(Name = "Narration")]
        [StringLength(2000, ErrorMessage = "Narration cannot exceed 2000 characters.")]
        public string Narration { get; set; }

        [Display(Name = "Amount FC")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal AmountFc { get; set; }

        [Display(Name = "Amount")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal Amount { get; set; }

        [Display(Name = "Amount FC in word")]
        public string AmountFcInWord { get; set; }

        [Display(Name = "Status")]
        public int StatusId { get; set; }

        public int CompanyId { get; set; }

        public int FinancialYearId { get; set; }

        public int MaxNo { get; set; }

        public int VoucherStyleId { get; set; }

        //####

        public string AccountLedgerName { get; set; }
        public string TypeCorBName { get; set; }
        public string PaymentTypeName { get; set; }
        public string CurrencyCode { get; set; }
        public string StatusName { get; set; }
        public string PreparedByName { get; set; }

        [Display(Name = "Closing Balance")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal ClosingBalance { get; set; }

        public int NoOfLineItems { get; set; }

    }
}