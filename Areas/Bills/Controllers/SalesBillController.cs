using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webApp00.Areas.Bills.Controllers
{

    [Area("Bills")]
    public class SalesBillController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
