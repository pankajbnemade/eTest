using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.DataAccess.Entity
{
    public class ApplicationIdentityUser : IdentityUser<int>
    {
        //[Required]
        //public int EmployeeId { get; set; }

        //public virtual State State { get; set; }
    }
}
