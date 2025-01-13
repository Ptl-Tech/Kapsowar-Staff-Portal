using Microsoft.AspNetCore.Mvc;

namespace App.Server.src.Modules.SupplierPortal.Controllers
{
    public class GeneralController : Controller
    {
        public IActionResult SupplierRegistration()
        {
            return View();
        }
    }
}
