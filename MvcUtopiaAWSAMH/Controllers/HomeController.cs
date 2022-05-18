using Microsoft.AspNetCore.Mvc;
using MvcUtopiaAWSAMH.Filters;
using MvcUtopiaAWSAMH.Services;
using NugetUtopia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcUtopiaAWSAMH.Controllers
{
    public class HomeController : Controller
    {
        private ServiceApiUtopia service;
        public HomeController(ServiceApiUtopia service)
        {
            this.service = service;
        }

        [AuthorizeUsuarios]
        public IActionResult GoToHome()
        {

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Index()
        {
            List<Juego> juegos = await this.service.GetJuegosAsync();
            return View(juegos);
        }

        public IActionResult SobreNosotros()
        {
            return View();
        }
    }
}
