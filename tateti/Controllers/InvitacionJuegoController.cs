using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.DependencyInjection;
using tateti.Services;
using tateti.Models;

namespace tateti.Controllers
{
    public class InvitacionJuegoController : Controller
    {
        private IUsuarioServicio _servicio;
        private IStringLocalizer<InvitacionJuegoController> _localizador;

        public InvitacionJuegoController(IUsuarioServicio servicio, IStringLocalizer<InvitacionJuegoController> localizador)
        {
            _servicio = servicio;
            _localizador = localizador;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string email)
        {
            var invitacion = new InvitacionJuegoModel
            {
                InvitadoPor = email
            };
            HttpContext.Session.SetString("Email", email);
            return View(invitacion);
        }

        //[HttpPost]
        //public IActionResult Index(InvitacionJuegoModel invitacion)
        //{
        //    return Content(_localizador["MensajeConfirmacionInvitacionJuego", invitacion.EmailDestino]);
        //}

            
        [HttpPost]
        public IActionResult Index(InvitacionJuegoModel invitacion, [FromServices] IEmailServicio emailServicio)
        {
            // Obtener el servicio que maneja la invitacion de juegos que es un singleton
            var invitacionServicio = Request.HttpContext.RequestServices.GetService<IInvitacionJuegoServicio>();
            
            if (ModelState.IsValid)
            {
                emailServicio.EnviarEmail(invitacion.EmailDestino,
                    _localizador["Invitación para jugar al Ta te ti"],
                    _localizador[$"Hola, has sido invitado para jugar al ta te ti por {0}. para unirse al juego, por favor haga click aquí {1}", invitacion.InvitadoPor,
                        Url.Action("ConfirmacionInvitacionJuego", "InvitacionJuego",
                        new { invitacion.InvitadoPor,
                            invitacion.EmailDestino }, Request.Scheme, Request.Host.ToString())]);

                var invitacionAgregada = invitacionServicio.AgregarInvitacion(invitacion).Result;
                return RedirectToAction("ConfirmacionInvitacionJuego", new { id = invitacionAgregada.Id });
            }
            return View(invitacion);
        }

        [HttpGet]
        public IActionResult ConfirmacionInvitacionJuego(Guid id, [FromServices] IInvitacionJuegoServicio invitacionJuegoServicio)
        {
            var invitacion = invitacionJuegoServicio.ObtenerInvitacion(id).Result;
            return View(invitacion);
        }

    }
}