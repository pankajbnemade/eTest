using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Models.Admin
{
    public class ConfirmEmailModel
    {
        [TempData]
        public string StatusMessage { get; set; }
    }
}
