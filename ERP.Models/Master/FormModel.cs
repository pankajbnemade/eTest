using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Master
{
    public class FormModel
    {
        public int FormId { get; set; }

        [Required(ErrorMessage = "Form Name is required.")]
        [Display(Name = "Form Name")]
        public string FormName { get; set; }

        [Required(ErrorMessage = "Module is required.")]
        [Display(Name = "Module")]
        public int? ModuleId { get; set; }

        //#####
        [Display(Name = "Module")]
        public string ModuleName { get; set; }

        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }
    }
}
