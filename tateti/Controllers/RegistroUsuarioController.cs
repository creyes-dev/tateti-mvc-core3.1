using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using tateti.Services;
using tateti.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace tateti.Controllers
{
    public class RegistroUsuarioController : Controller
    {
        readonly IUsuarioServicio _servicio;
        readonly IEmailServicio _emailServicio;
        readonly ILogger<RegistroUsuarioController> _logger;

        public RegistroUsuarioController(IUsuarioServicio servicio, IEmailServicio emailServicio,
            ILogger<RegistroUsuarioController> logger)
        {
            _servicio = servicio;
            _emailServicio = emailServicio;
            _logger = logger;
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
            // El mail no ha sido confirmado
            _logger.LogInformation($"##Start##  Proceso de confirmación de correo para {email} ");
            var usuario = await _servicio.ObtenerUsuarioPorEmail(email);

            var urlAccion = new UrlActionContext
            {
                Action = "ConfirmacionCorreo",
                Controller = "RegistroUsuario",
                Values = new { email },
                Protocol = Request.Scheme,
                Host = Request.Host.ToString()
            };

            var emailRegistroUsuario = new UsuarioRegistroEmailModelo
            {
                Nombre = $"{usuario.Nombre} {usuario.Apellido}",
                Email = email,
                AccionUrl = Url.Action(urlAccion)
            };

            var emailRenderServicio = HttpContext.RequestServices.GetService<IEmailRenderPlantillaServicio>();
            // TODO: Renderear la vista
            // var mensaje = await emailRenderServicio.RenderearTemplate("PlantillasEmail/RegistroUsuarioMail", emailRegistroUsuario, Request.Host.ToString());
            var mensaje = "Mensaje que va dentro del mail"; 

            try
            {
                _emailServicio.EnviarEmail(email, "Confirmación de correo de Ta Te Ti", mensaje).Wait();
            }
            catch(Exception e)
            {
                
            }

            ViewBag.Email = email;
            return View();
        }

    }
}