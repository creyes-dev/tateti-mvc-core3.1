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

                return Content($"El usuario {usuario.Nombre} {usuario.Apellido} ha sido registrado satisfactoriamente");
            }
            return View(usuario);
        }

    }
}