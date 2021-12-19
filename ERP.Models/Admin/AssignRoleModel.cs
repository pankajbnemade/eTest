using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Admin
{
    public class AssignRoleModel
    {
        [Required]
        [Display(Name = "User Name")]
        public int UserId { get; set; }

        public string Email { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public int RoleId { get; set; }

        public string RoleName { get; set; }
    }
}
