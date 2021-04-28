using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Master
{
    public class DepartmentModel
    {

        public int DepartmentId { get; set; }

         [Required(ErrorMessage = "Department is required.")]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }

        //####
        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }

    }
}
