﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using ERP.Models.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ERP.DataAccess.EntityModels
{
    public partial class CreditNoteDetailModel
    {

        public int CreditNoteDetId { get; set; }

        public int? CreditNoteId { get; set; }
        
        [Display(Name = "Sr No.")]
        [Required(ErrorMessage = "Sr No. is required.")]
        [RegularExpression(RegexHelper.NumericOnly, ErrorMessage = "Numbers only.")]
        public int? SrNo { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
        public string Description { get; set; }

        [Display(Name = "UOM")]
        [Required(ErrorMessage = "UOM is required.")]
        public int? UnitOfMeasurementId { get; set; }

        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "Quantity is required.")]
        public decimal? Quantity { get; set; }

        [Display(Name = "Per Unit")]
        [Required(ErrorMessage = "Per Unit is required.")]
        //[RegularExpression(RegexHelper.DecimalOnly, ErrorMessage = "Numbers only.")]
        public int? PerUnit { get; set; }

        [Display(Name = "Unit Price")]
        [Required(ErrorMessage = "Unit Price is required.")]
        //[RegularExpression(RegexHelper.DecimalOnly, ErrorMessage = "Numbers only.")]
        public decimal? UnitPrice { get; set; }

        [Display(Name = "Gross Amount FC")]
        public decimal? GrossAmountFc { get; set; }

        [Display(Name = "Gross Amount")]
        public decimal? GrossAmount { get; set; }

        [Display(Name = "Tax Amount")]
        public decimal? TaxAmount { get; set; }

        [Display(Name = "Tax Amount FC")]
        public decimal? TaxAmountFc { get; set; }

        [Display(Name = "Net Amount FC")]
        public decimal? NetAmountFc { get; set; }

        [Display(Name = "Net Amount")]
        public decimal? NetAmount { get; set; }

        [Display(Name = "Unit Of Measurement Name")]
        public string UnitOfMeasurementName { get; set; }

    }
}