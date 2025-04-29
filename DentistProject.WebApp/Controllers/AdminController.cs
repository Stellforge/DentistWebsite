using Microsoft.AspNetCore.Mvc;

namespace DisciProjesi.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult AdminLogin()
        {
            return View();
        }
    }
}