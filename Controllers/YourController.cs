using JWToken.CustomAttributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWToken.Controllers
{
    [Authorize(Roles.DIRECTOR, Roles.SUPERVISOR, Roles.ANALYST)]
    public class YourController : Controller
    {
        public IActionResult Action1()
        {
            return View();
        }

        public IActionResult Action2()
        {
            return View();
        }

        public IActionResult Action3()
        {
            return View();
        }

    }
}
