using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Master
{
    public class EmployeeModel
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Employee Code is required.")]
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Designation is required.")]
        [Display(Name = "Designation")]
        public int DesignationId { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

         [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        public int MaxNo { get; set; }

        //#####
        [Display(Name = "Designation")]
        public string DesignationName { get; set; }

        [Display(Name = "Department")]
        public string DepartmentName { get; set; }

         [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }

    }
}
