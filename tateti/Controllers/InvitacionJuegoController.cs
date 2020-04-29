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
                InvitadoPor = email,
                Id = Guid.NewGuid()
            };

            Request.HttpContext.Session.SetString("Email", email);
            var usuario = await _servicio.ObtenerUsuarioPorEmail(email);
            Request.HttpContext.Session.SetString("NombreCompleto", $"{ usuario.Nombre} { usuario.Apellido}");
            
            return View(invitacion);
        }
                    
        [HttpPost]
        public async Task<IActionResult> Index(InvitacionJuegoModel invitacion, [FromServices] IEmailServicio emailServicio)
        {
            // Obtener el servicio que maneja la invitacion de juegos que es un singleton
            var invitacionServicio = Request.HttpContext.RequestServices.GetService<IInvitacionJuegoServicio>();

            if (ModelState.IsValid)
            {
                try
                {
                    var invitacionModel = new InvitacionEmailModel
                    {
                        Nombre = $"{invitacion.EmailDestino}",
                        InvitadoPor = await _servicio.ObtenerUsuarioPorEmail(invitacion.InvitadoPor),
                        ConfirmacionUrl = Url.Action("ConfirmacionInvitacionJuego", "InvitacionJuego", new { id = invitacion.Id }, Request.Scheme, Request.Host.ToString()),
                        FechaInvitacion = invitacion.FechaConfirmacion
                    };

                    var servicioRenderEmail = HttpContext.RequestServices.GetService<IEmailRenderPlantillaServicio>();
                    var mensaje = await servicioRenderEmail.RenderearTemplate<InvitacionEmailModel>("PlantillasEmail/InvitacionEmail",
                        invitacionModel, Request.Host.ToString());

                    await emailServicio.EnviarEmail(invitacion.EmailDestino, _localizador["Invitación para jugar al Ta te ti"], mensaje);
                }
                catch
                {
                }

                var invitacionAgregada = invitacionServicio.AgregarInvitacion(invitacion).Result;
                return RedirectToAction("ConfirmacionInvitacionJuego", new { id = invitacion.Id });
            }
            return View(invitacion);
        }

        [HttpGet]
        public IActionResult ConfirmarInvitacionJuego(Guid id, [FromServices] IInvitacionJuegoServicio invitacionJuegoServicio)
        {
            var gameInvitation = invitacionJuegoServicio.ObtenerInvitacion(id).Result;
            gameInvitation.EstaConfirmado = true;
            gameInvitation.FechaConfirmacion = DateTime.Now;
            invitacionJuegoServicio.ActualizarInvitacion(gameInvitation);
            return RedirectToAction("Index", "SesionJuego", new { id = id });
        }

        [HttpGet]
        public IActionResult ConfirmacionInvitacionJuego(Guid id, [FromServices] IInvitacionJuegoServicio invitacionJuegoServicio)
        {
            var invitacion = invitacionJuegoServicio.ObtenerInvitacion(id).Result;
            return View(invitacion);
        }

    }
}