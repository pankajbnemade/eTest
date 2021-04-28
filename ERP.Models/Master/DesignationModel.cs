using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Master
{
    public class DesignationModel
    {
        public int DesignationId { get; set; }

        [Required(ErrorMessage = "Designation is required.")]
        [Display(Name = "Designation Name")]
        public string DesignationName { get; set; }

        //####
        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }
    }
}
