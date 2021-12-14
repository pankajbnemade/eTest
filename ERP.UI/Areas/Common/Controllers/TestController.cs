using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Common.Controllers
{
    [Area("Common")]
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
