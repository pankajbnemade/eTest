using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Admin
{
    public class ApplicationRoleModel
    {
        public virtual int Id { get; set; }
        
        [Required]
        [Display(Name = "Role")]
        public virtual string Name { get; set; }
        
        public virtual string NormalizedName { get; set; }

        public virtual string ConcurrencyStamp { get; set; }
    }
}
