using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using tateti.Services;

namespace tateti.Middleware
{
    public class ComunicacionMiddleware
    {
        private readonly RequestDelegate _siguiente;
        private readonly IUsuarioServicio _usuarioService;

        public ComunicacionMiddleware(RequestDelegate siguiente, IUsuarioServicio servicio)
        {
            _siguiente = siguiente;
            _usuarioService = servicio;
        }

        public async Task Invoke(HttpContext contexto)
        {
            await _siguiente.Invoke(contexto);
            return;
        }

    }
}
