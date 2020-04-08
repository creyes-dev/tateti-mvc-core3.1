using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tateti.Services;
using tateti.Models;

namespace tateti.Controllers
{
    public class InvitacionJuegoController : Controller
    {
        private IUsuarioServicio _servicio;

        public InvitacionJuegoController(IUsuarioServicio servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string email)
        {
            var invitacion = new InvitacionJuegoModel
            {
                InvitadoPor = email
            };

            return View(invitacion);
        }
    }
}