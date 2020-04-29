using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tateti.Services;
using Microsoft.Extensions.DependencyInjection;

namespace tateti.Controllers
{
    public class SesionJuegoController : Controller
    {
        private ISesionJuegoServicio _sesionJuegoServicio;

        public SesionJuegoController(ISesionJuegoServicio sesionJuegoServicio)
        {
            _sesionJuegoServicio = sesionJuegoServicio;
        }

        public async Task<ActionResult> Index(Guid id)
        {
            var sesion = await _sesionJuegoServicio.ObtenerSesionJuego(id);

            if(sesion == null)
            {
                var servicioInvitacionJuego = Request.HttpContext.RequestServices.GetService<IInvitacionJuegoServicio>();
                var invitacion = await servicioInvitacionJuego.ObtenerInvitacion(id);

                // sesion = await 
                sesion = await _sesionJuegoServicio.CrearSesionJuego(invitacion.Id, invitacion.InvitadoPor, invitacion.EmailDestino);
            }

            return View(sesion);
        }

        public async Task<IActionResult> SetPosicion(Guid id, string email, int x, int y)
        {
            var sesionJuego = await _sesionJuegoServicio.ObtenerSesionJuego(id);
            await _sesionJuegoServicio.AgregarTurno(sesionJuego.Id, email, x, y);
            return View("Index", sesionJuego);
        }
    }
}