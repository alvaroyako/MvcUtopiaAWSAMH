using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
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
    public class ReservasController : Controller
    {
        private ServiceApiUtopia service;

        public ReservasController(ServiceApiUtopia service)
        {
            this.service = service;
        }

        #region Lado del Cliente
        [AuthorizeUsuarios]
        public IActionResult Index()
        {
            return View();
        }

        //Ejecuta el form para crear una reserva 
        //desde el lado del cliente. Ademas envia un correo de confirmacion
        [HttpPost]
        public async Task<IActionResult> Index(string nombre, string telefono, string email, int personas, DateTime fecha)
        {
            Reserva reserva = new Reserva();
            reserva.Nombre = nombre;
            reserva.Telefono = telefono;
            reserva.Email = email;
            reserva.Personas = personas;
            reserva.Fecha = fecha.ToShortDateString();
            reserva.Hora = fecha.ToShortTimeString();

            await this.service.CrearReservaAsync(reserva);
            //string asunto = "Reserva Utopia";
            //string mensaje = "<p>Hola " + nombre + "! Los datos de tu reserva son los siguientes:</p> <p> Día: " + fecha.ToShortDateString() + "</p><p>Hora: " + fecha.ToShortTimeString() + "</p><p> Número de asistentes: " + personas + "</p><p> Teléfono de contacto: " + telefono + "</p><p> Recuerda enseñar este correo en el local para verificar la reserva.</p><p> Esperemos que pases un gran día en Utopia! </p>";

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

            //ViewData["CORREO"] = "<div><p>Hemos enviado un correo al email proporcionado. Por favor revisa tu bandeja de entrada y en caso de no haber ningun mensaje, mira en la carpeta spam</p></div>";

            return View();
        }
        #endregion

        #region Lado del Admin
        [AuthorizeUsuarios(Policy = "AdminOnly")]
        public IActionResult CrearReserva()
        {
            return View();
        }

        //Ejecuta el form para crear una reserva 
        //desde el panel del admin
        [HttpPost]
        public async Task<IActionResult> CrearReserva(string nombre, string telefono, string email, int personas, DateTime fecha)
        {
            Reserva reserva = new Reserva();
            reserva.Nombre = nombre;
            reserva.Telefono = telefono;
            reserva.Email = email;
            reserva.Personas = personas;
            reserva.Fecha = fecha.ToShortDateString();
            reserva.Hora = fecha.ToShortTimeString();

            await this.service.CrearReservaAsync(reserva);
            return RedirectToAction("Index", "Admin");
        }

        //Metodo para eliminar reservas
        [AuthorizeUsuarios(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteReserva(string nombre)
        {
            string token = HttpContext.User.FindFirst("TOKEN").Value;
            await this.service.DeleteReservaAsync(nombre, token);
            return RedirectToAction("Index", "Admin");
        }

        //Metodo para cargar datos de la reserva en el form editar
        [AuthorizeUsuarios(Policy = "AdminOnly")]
        public async Task<IActionResult> EditarReserva(string nombre)
        {
            string token = HttpContext.User.FindFirst("TOKEN").Value;
            Reserva reserva = await this.service.FindReservaAsync(nombre);
            return View(reserva);
        }

        //Metodo que ejecuta el form para editar la reserva
        [HttpPost]
        public async Task<IActionResult> EditarReserva(string nombre, string telefono, string email, int personas, DateTime fecha)
        {
            string token = HttpContext.User.FindFirst("TOKEN").Value;

            Reserva reserva = new Reserva();
            reserva.Nombre = nombre;
            reserva.Telefono = telefono;
            reserva.Email = email;
            reserva.Personas = personas;
            reserva.Fecha = fecha.ToShortDateString();
            reserva.Hora = fecha.ToShortTimeString();

            await this.service.UpdateReservaAsync(reserva, token);
            //string asunto = "Reserva Utopia";
            //string mensaje = "Hola " + nombre + "! Hemos modificado tu reserva siendo este el resultado final:</p> <p> Día: " + fecha.ToShortDateString() + "</p><p>Hora: " + fecha.ToShortTimeString() + "</p><p> Número de asistentes: " + personas + "</p><p> Teléfono de contacto: " + telefono + "</p><p> Recuerda enseñar este nuevo correo en el local para verificar la reserva. Puedes eliminar el correo anterior si asi lo deseas</p><p> Esperemos que pases un gran día en Utopia! </p>";

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

            return RedirectToAction("Index", "Admin");
        }
        #endregion
    }
}
