using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcUtopiaAWSAMH.Services;
using NugetUtopia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcUtopiaAWSAMH.Controllers
{
    public class ManageController : Controller
    {
        private ServiceApiUtopia service;

        public ManageController(ServiceApiUtopia service)
        {
            this.service = service;
        }

        #region Registro
        //Metodo que devuelve la vista con el formulario de registro
        public IActionResult Register()
        {
            return View();
        }

        //Metodo que ejecuta el form Register
        [HttpPost]
        public async Task<IActionResult> Register(string nombre, string email, string password, IFormFile imagen)
        {
            string filename = imagen.FileName;
            int idusuario = await this.service.RegistrarUsuarioAsync(nombre, email, password, filename);
            filename = idusuario + "_" + filename;

            using (Stream stream = imagen.OpenReadStream())
            {
                await this.service.UploadFile(stream, filename, "users");
            }
            //string asunto = "Bienvenido a Utopia";
            //string mensaje = "Hola " + nombre + ". Te mandamos este correo para informarte de que te has registrado con éxito en la página web de Utopía.";

            //var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.USEast1);
            //Destination destination = new Destination();
            //destination.ToAddresses = new List<string> { email };
            //Message message = new Message();
            //message.Subject = new Content(asunto);
            //Body cuerpo = new Body();
            //cuerpo.Html = new Content(mensaje);
            //cuerpo.Text = new Content(mensaje);
            //message.Body = cuerpo;
            //SendEmailRequest request = new SendEmailRequest();
            //request.Source = "alvaro.moya@tajamar365.com";
            //request.Destination = destination;
            //request.Message = message;
            //SendEmailResponse response = await client.SendEmailAsync(request);


            return RedirectToAction("GoToHome", "Home");
        }
        #endregion

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(string email, string password)
        {
            string token = await this.service.GetToken(email, password);
            if (token == null)
            {
                ViewData["MENSAJE"] = "Email/Password incorrectos";
                return View();
            }
            else
            {
                Usuario usuario = await this.service.GetPerfilUsuarioAsync(token);
                HttpContext.Session.SetString("TOKEN", token);
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.Name, usuario.Nombre));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Role, usuario.Rol));
                if (usuario.Rol == "admin")
                {
                    identity.AddClaim(new Claim("Administrador", "Soy admin"));
                }
                identity.AddClaim(new Claim("Email", usuario.Email));
                identity.AddClaim(new Claim("Imagen", usuario.Imagen));
                identity.AddClaim(new Claim("TOKEN", token));
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                });
                return RedirectToAction("Index", "Home");
            }

        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("TOKEN");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ErrorAcceso()
        {
            return View();
        }
    }
}
