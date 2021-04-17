using ERP.Models.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Models.Accounts
{
    public class SalesInvoiceDetailModel
    {
        public int InvoiceDetId { get; set; }
        public int? InvoiceId { get; set; }

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
        [RegularExpression(RegexHelper.NumericOnly, ErrorMessage = "Numbers only.")]
        public int? PerUnit { get; set; }

        [Display(Name = "Unit Price")]
        [Required(ErrorMessage = "Unit Price is required.")]
        public decimal? UnitPrice { get; set; }
        public decimal? GrossAmountFc { get; set; }
        public decimal? GrossAmount { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? TaxAmountFc { get; set; }
        public decimal? NetAmountFc { get; set; }
        public decimal? NetAmount { get; set; }

        //####
        public string UnitOfMeasurementName { get; set; }

    }
}
