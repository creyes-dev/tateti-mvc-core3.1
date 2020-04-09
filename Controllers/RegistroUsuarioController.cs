using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tateti.Services;
using tateti.Models;

namespace tateti.Controllers
{
    public class RegistroUsuarioController : Controller
    {
        private IUsuarioServicio _servicio;

        public RegistroUsuarioController(IUsuarioServicio servicio)
        {
            _servicio = servicio;
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
            ViewBag.Email = email;
            return View();
        }

    }
}