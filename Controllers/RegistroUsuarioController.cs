using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using tateti.Services;
using tateti.Models;

namespace tateti.Controllers
{
    public class RegistroUsuarioController : Controller
    {
        readonly IUsuarioServicio _servicio;
        readonly IEmailServicio _emailServicio; 

        public RegistroUsuarioController(IUsuarioServicio servicio, IEmailServicio emailServicio)
        {
            _servicio = servicio;
            _emailServicio = emailServicio;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UsuarioModel usuario)
        {
            if (ModelState.IsValid)
            {
                // El método es asíncrono pero acá se solicita que se espere hasta
                // que el método se ejecute para poder continuar
                await _servicio.RegistrarUsuario(usuario);
                return RedirectToAction(nameof(ConfirmacionCorreo), new { usuario.Email });
            }
            else
            {
                return View(usuario);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmacionCorreo(string email)
        {
            var usuario = await _servicio.ObtenerUsuarioPorEmail(email);

            if (usuario?.MailEstaConfirmado == true)
            {
                return RedirectToAction("Index", "InvitacionJuego", new { email = email });
            }
            else
            {
                // El mail no ha sido confirmado
                var urlAccion = new UrlActionContext
                {
                    Action = "ConfirmacionCorreo",
                    Controller = "RegistroUsuario",
                    Values = new { email },
                    Protocol = Request.Scheme,
                    Host = Request.Host.ToString()
                };

                var mensaje = $"Gracias por registrarte en nuestro sitio, por favor haz click aquí para confirmar" + $"{ Url.Action(urlAccion) }";

                try
                {
                    _emailServicio.EnviarEmail(email, "Confirmación de correo de Ta Te Ti", mensaje).Wait();
                }
                catch (Exception e)
                {

                }
            }

            ViewBag.Email = email;
            return View();
        }

    }
}