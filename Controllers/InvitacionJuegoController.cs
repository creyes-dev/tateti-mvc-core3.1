using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
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
            HttpContext.Session.SetString("email", email);
            return View(invitacion);
        }

        [HttpPost]
        public IActionResult Index(InvitacionJuegoModel invitacion)
        {
            return Content(_localizador["MensajeConfirmacionInvitacionJuego", invitacion.EmailDestino]);
        }

    }
}