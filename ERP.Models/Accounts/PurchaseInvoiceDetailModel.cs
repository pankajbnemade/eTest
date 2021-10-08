using ERP.Models.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class PurchaseInvoiceDetailModel
    {
        public int PurchaseInvoiceDetId { get; set; }

        public int PurchaseInvoiceId { get; set; }

        [Display(Name = "Sr No.")]
        [Required(ErrorMessage = "Sr No. is required.")]
        [RegularExpression(RegexHelper.NumericOnly, ErrorMessage = "Numbers only.")]
        public int SrNo { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
        public string Description { get; set; }

        [Display(Name = "UOM")]
        [Required(ErrorMessage = "UOM is required.")]
        public int UnitOfMeasurementId { get; set; }

        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "Quantity is required.")]
        [RegularExpression(RegexHelper.DecimalOnly, ErrorMessage = "Up to 2 Decimal only.")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Quantity { get; set; }

        [Display(Name = "Per Unit")]
        [Required(ErrorMessage = "Per Unit is required.")]
        [RegularExpression(RegexHelper.NumericOnly, ErrorMessage = "Number only.")]
        public int PerUnit { get; set; }

        [Display(Name = "Unit Price")]
        [Required(ErrorMessage = "Unit Price is required.")]
        [RegularExpression(RegexHelper.DecimalOnly, ErrorMessage = "Decimal only.")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Gross Amount FC")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal GrossAmountFc { get; set; }

        [Display(Name = "Gross Amount")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal GrossAmount { get; set; }

        [Display(Name = "Tax Amount FC")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal TaxAmountFc { get; set; }
        
        [Display(Name = "Tax Amount")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal TaxAmount { get; set; }

        [Display(Name = "Net Amount FC")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal NetAmountFc { get; set; }

        [Display(Name = "Net Amount")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal NetAmount { get; set; }
        //####
        [Display(Name = "UOM")]
        public string UnitOfMeasurementName { get; set; }

        public bool IsTaxDetVisible { get; set; }
    }
}
