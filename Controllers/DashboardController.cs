using Microsoft.AspNetCore.Mvc;
using JWToken.CustomAttributes;

namespace JWToken.Controllers
{

    [UnAuthorized]
    public class DashboardController : Controller
    {

        [Authorize(Roles.DIRECTOR, Roles.SUPERVISOR, Roles.ANALYST)]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles.DIRECTOR)]
        public IActionResult DirectorPage()
        {
            return View("DirectorPage");
        }

        [Authorize(Roles.SUPERVISOR)]
        public IActionResult SupervisorPage()
        {
            ViewBag.Message = "Permission controlled through action Attribute";
            return View("SupervisorPage");
        }

        [Authorize(Roles.ANALYST)]
        public IActionResult AnalystPage()
        {
            return View("AnalystPage");
        }

        public IActionResult SupervisorAnalystPage()
        {
            ViewBag.Message = "Permission controlled inside action method";
            if (this.HavePermission(Roles.SUPERVISOR))
                return View("SupervisorPage");

            if (this.HavePermission(Roles.ANALYST))
                return View("AnalystPage");

            return new RedirectResult("~/Dashboard/NoPermission");
        }

    }
}
