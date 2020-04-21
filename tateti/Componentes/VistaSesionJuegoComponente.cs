using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tateti.Services;

namespace tateti.Componentes
{
    [ViewComponent(Name = "SesionJuego")]
    public class VistaSesionJuegoComponente : ViewComponent
    {
        ISesionJuegoServicio _servicio;

        public VistaSesionJuegoComponente(ISesionJuegoServicio servicio)
        {
            _servicio = servicio;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid idSesionJuego)
        {
            var sesion = await _servicio.ObtenerSesionJuego(idSesionJuego);
            return View(sesion);
        }

    }
}
