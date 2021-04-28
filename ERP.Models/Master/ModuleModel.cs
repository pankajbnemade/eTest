using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Master
{
    public class ModuleModel
    {
        public int ModuleId { get; set; }

        [Required(ErrorMessage = "Module Name is required.")]
        [Display(Name = "Module Name")]
        public string ModuleName { get; set; }

        [Required(ErrorMessage = "Is Active is required.")]
        [Display(Name = "Is Active")]
        public sbyte? IsActive { get; set; }

        //####
        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }
    }
}
