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
            if(contexto.Request.Path.Equals(
                "/ChequearEstadoConfirmacionEmail"))
            {
                // Si la dirección del request se dirige a 
                // ChequearEstadoConfirmacionEmail entonces llamar al método
                // responsable de recibir la solicitud
                await ProcesarConfirmacionEmail(contexto);
            }
            else
            {
                await _siguiente?.Invoke(contexto);
            }
        }

        private async Task ProcesarConfirmacionEmail(HttpContext contexto)
        {
            var email = contexto.Request.Query["Email"];
            var usuario = await _usuarioService.ObtenerUsuarioPorEmail(email);

            if(string.IsNullOrEmpty(email))
            {
                await contexto.Response.WriteAsync("BadRequest: Email es requerido");
            } 
            else
            {
                if(usuario.MailEstaConfirmado) 
                { 
                    await contexto.Response.WriteAsync("OK"); 
                }
                else
                {
                    await contexto.Response.WriteAsync("EsperandoPorConfirmacionEmail");
                    usuario.MailEstaConfirmado = true;
                    usuario.FechaConfirmacionEmail = DateTime.Now;
                    _usuarioService.ActualizarUsuario(usuario).Wait(); // Con wait esperamos a que el Task se complete
                }
            }
        }

        /*
        
        public async Task Invoke(HttpContext context)
        {
             if(context.Request.Path.Equals("/ChequearEstadoConfirmacionEmail"))
            {
                await ProcesarConfirmacionEmail(context);
            }
            else
            {
                await _next?.
            }
        }
    */

    }
}
