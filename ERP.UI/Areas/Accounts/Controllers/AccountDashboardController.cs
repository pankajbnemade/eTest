﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Accounts.Controllers
{
    public class AccountDashboardController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() =>
            {
                return View();
            });
        }
    }
}
