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
    public class UsuariosController : Controller
    {
        private ServiceApiUtopia service;
        public UsuariosController(ServiceApiUtopia service)
        {
            this.service = service;
        }


        public IActionResult Index()
        {
            return View();
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> DetallesUsuario()
        {
            string token = HttpContext.User.FindFirst("TOKEN").Value;
            Usuario usuario =
                await this.service.GetPerfilUsuarioAsync(token);
            List<Compra> compras = await this.service.BuscarComprasAsync(usuario.IdUsuario, token);

            ViewData["COMPRAS"] = compras;
            string vista = "";
            foreach (Compra compra in compras)
            {
                vista +=
                    "<li class='list-group-item'><img src='https://storageproyectoamh.blob.core.windows.net/juegos/" + this.service.FindJuegoNombreAsync(compra.Nombre).Result.Foto + "' style='width: 100px; height: 100px'/><p>"
                    + compra.Nombre +
                    "</p></li>";

            }
            ViewData["COMPRASHTML"] = vista;

            return View(usuario);
        }

    }
}
