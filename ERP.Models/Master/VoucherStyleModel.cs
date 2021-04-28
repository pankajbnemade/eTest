using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Master
{
    public class VoucherStyleModel
    {
        public int VoucherStyleId { get; set; }

        [Required(ErrorMessage = "Voucher Style Name is required.")]
        [Display(Name = "Voucher Style Name")]
        public string VoucherStyleName { get; set; }

        //####
        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }
    }
}
