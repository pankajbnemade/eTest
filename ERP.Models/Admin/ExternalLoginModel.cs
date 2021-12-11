using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Models.Admin
{
    public class ExternalLoginModel
    {
        [Required]
            [EmailAddress]
            public string Email { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ReturnUrl { get; set; }

        //[TempData]
        public string ErrorMessage { get; set; }
    }
}
