using Microsoft.AspNetCore.Mvc;
using MvcUtopiaAWSAMH.Extensions;
using MvcUtopiaAWSAMH.Filters;
using MvcUtopiaAWSAMH.Services;
using NugetUtopia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcUtopiaAWSAMH.Controllers
{
    public class CarritoController : Controller
    {
        private ServiceApiUtopia service;

        public CarritoController(ServiceApiUtopia service)
        {
            this.service = service;
        }

        [AuthorizeUsuarios]
        public IActionResult ListaJuegos(int? id)
        {
            if (id != null)
            {
                List<int> carrito;
                if (HttpContext.Session.GetObject<List<int>>("CARRITO") == null)
                {
                    carrito = new List<int>();
                }
                else
                {
                    carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
                }
                if (carrito.Contains(id.Value) == false)
                {
                    carrito.Add(id.Value);
                    HttpContext.Session.SetObject("CARRITO", carrito);
                }
            }
            return RedirectToAction("Index", "Juegos");
        }

        [AuthorizeUsuarios]
        public IActionResult ListaJuegosHome(int? id)
        {
            if (id != null)
            {
                List<int> carrito;
                if (HttpContext.Session.GetObject<List<int>>("CARRITO") == null)
                {
                    carrito = new List<int>();
                }
                else
                {
                    carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
                }
                if (carrito.Contains(id.Value) == false)
                {
                    carrito.Add(id.Value);
                    HttpContext.Session.SetObject("CARRITO", carrito);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> Carrito(int? id)
        {
            List<int> carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
            if (carrito == null)
            {
                return View();
            }
            else
            {
                if (id != null)
                {
                    carrito.Remove(id.Value);
                    HttpContext.Session.SetObject("CARRITO", carrito);
                }

                List<Juego> juegos = new List<Juego>();
                foreach (int idjuego in carrito)
                {
                    Juego juego = await this.service.FindJuegoAsync(idjuego);
                    juegos.Add(juego);
                }

                return View(juegos);
            }
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> CompraCarrito()
        {
            string token = HttpContext.User.FindFirst("TOKEN").Value;
            List<int> idsjuegos = HttpContext.Session.GetObject<List<int>>("CARRITO");
            int idusuario = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            int idcompra = await this.service.GetMaxIdCompra();
            foreach (int id in idsjuegos)
            {
                Juego juego = await this.service.FindJuegoAsync(id);

                Compra compra = new Compra();
                compra.IdCompra = idcompra;
                compra.IdUsuario = idusuario;
                compra.Nombre = juego.Nombre;

                await this.service.CrearCompraAsync(compra, token);
            }
            HttpContext.Session.Remove("CARRITO");
            return RedirectToAction("DetallesUsuario", "Usuarios");
        }
    }
}
