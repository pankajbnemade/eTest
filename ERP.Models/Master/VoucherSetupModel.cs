using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Master
{
    public class VoucherSetupModel
    {
        public int VoucherSetupId { get; set; }

        [Required(ErrorMessage = "Voucher Setup Name is required.")]
        [Display(Name = "Voucher Setup Name")]
        public string VoucherSetupName { get; set; }

        [Required(ErrorMessage = "Module is required.")]
        [Display(Name = "Module")]
        public int? ModuleId { get; set; }

        [Required(ErrorMessage = "Is Active is required.")]
        [Display(Name = "Is Active")]
        public sbyte? IsActive { get; set; }

        //####
        [Display(Name = "Module Name")]
        public string ModuleName { get; set; }

        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }
    }
}
