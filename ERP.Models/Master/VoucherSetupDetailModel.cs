using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Master
{
    public class VoucherSetupDetailModel
    {
        public int VoucherSetupDetId { get; set; }

        [Required(ErrorMessage = "Voucher Setup is required.")]
        [Display(Name = "Voucher Setup")]
        public int VoucherSetupId { get; set; }

        [Display(Name = "Number Padding Character")]
        public string NoPad { get; set; }

        [Display(Name = "Number Pre String")]
        public string NoPreString { get; set; }

        [Display(Name = "Number Post String")]
        public string NoPostString { get; set; }

        [Display(Name = "Number Separator")]
        public string NoSeparator { get; set; }

        [Display(Name = "Format Text")]
        public string FormatText { get; set; }

        [Required(ErrorMessage = "Voucher Style is required.")]
        [Display(Name = "Voucher Style")]
        public int VoucherStyleId { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Financial Year is required.")]
        [Display(Name = "Financial Year")]
        public int FinancialYearId { get; set; }

        //####
        [Display(Name = "Voucher Setup Name")]
        public string VoucherSetupName { get; set; }

        [Display(Name = "Module Name")]
        public string ModuleName { get; set; }

        [Display(Name = "Voucher Style Name")]
        public string VoucherStyleName { get; set; }

        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }
    }
}
