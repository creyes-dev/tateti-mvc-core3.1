using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.WebSockets;
using tateti.Services;
using tateti.Models;
using Newtonsoft.Json;

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
            if (contexto.WebSockets.IsWebSocketRequest)
            {
                // Es una solicitud por websocket, recibir el json recibido
                var webSocket = await contexto.WebSockets.AcceptWebSocketAsync();
                var tokenCancelacion = contexto.RequestAborted;
                var json = await RecibirStringAsync(webSocket, tokenCancelacion);
                var comando = JsonConvert.DeserializeObject<dynamic>(json);

                switch (comando.Operation.ToString())
                {
                    case "ChequearEstadoConfirmacionEmail":
                        {
                            // Procesar la confirmación del mail usando websocket
                            // por medio de websocket tan pronto como sea posible
                            await ProcesarConfirmacionEmail(contexto, webSocket, tokenCancelacion, comando.Parameters.ToString());
                            break;
                        }
                }
            }
            else
            {
                // Es una solicitud por un request http
                if (contexto.Request.Path.Equals("/ChequearEstadoConfirmacionEmail"))
                {
                    // Procesar la confirmación del mail sin usar websocket
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
        }

        // Procesar confirmación de mail sin usar websocket
        private async Task ProcesarConfirmacionEmail(HttpContext contexto)
        {
            var email = contexto.Request.Query["Email"];

            if (string.IsNullOrEmpty(email))
            {
                await contexto.Response.WriteAsync("BadRequest: Email es requerido");
            }
            else
            {
                var usuario = await _usuarioService.ObtenerUsuarioPorEmail(email);

                if (usuario.MailEstaConfirmado)
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

        // Procesar confirmación de mail por websocket
        public async Task ProcesarConfirmacionEmail(HttpContext context, WebSocket currentSocket, CancellationToken tokenCancelacion, string email)
        {
            UsuarioModel usuario = await _usuarioService.ObtenerUsuarioPorEmail(email);
            
            while (!tokenCancelacion.IsCancellationRequested &&
            !currentSocket.CloseStatus.HasValue &&
            usuario?.MailEstaConfirmado == false)
            {
                if (usuario.MailEstaConfirmado)
                {
                    await EnviarStringAsync(currentSocket, "OK. El Mail ya se encuentra confirmado", tokenCancelacion);
                }
                else
                {
                    usuario.MailEstaConfirmado = true;
                    usuario.FechaConfirmacionEmail = DateTime.Now;
                    await _usuarioService.ActualizarUsuario(usuario);
                    await EnviarStringAsync(currentSocket, "OK, el mail ha sido confirmado", tokenCancelacion);
                }

                Task.Delay(500).Wait();
                usuario = await _usuarioService.ObtenerUsuarioPorEmail(email);
            }
        }

        // Enviar string por medio de WebSocket
        private static Task EnviarStringAsync(WebSocket socket, string texto, CancellationToken ct = default(CancellationToken))
        {
            // Codificar el texto a enviar
            var buffer = Encoding.UTF8.GetBytes(texto);
            // Estructurar el texto en un array de bytes
            var segmento = new ArraySegment<byte>(buffer);
            // Enviar el texto por medio de websocket
            return socket.SendAsync(segmento, WebSocketMessageType.Text, true, ct);
        }

        // Recibir string por medio de una comunicación con WEbSockets
        private static async Task<string> RecibirStringAsync(WebSocket socket, CancellationToken tokenCancelacion = default(CancellationToken))
        {
            // Buffer almacenará el mensaje completo recibido en bloques de bytes
            var buffer = new ArraySegment<byte>(new byte[8192]);
            
            using (var texto = new MemoryStream())
            {
                WebSocketReceiveResult mensajeTexto;

                // Armar el array de bytes correspondientes al mensaje recibido por WebSockets
                do
                {
                    tokenCancelacion.ThrowIfCancellationRequested();
                    mensajeTexto = await socket.ReceiveAsync(buffer, tokenCancelacion);
                    // Ir escribiendo en el texto el array de bytes actual
                    texto.Write(buffer.Array, buffer.Offset, mensajeTexto.Count);
                } while (!mensajeTexto.EndOfMessage);

                texto.Seek(0, SeekOrigin.Begin);
                if (mensajeTexto.MessageType != WebSocketMessageType.Text) throw new Exception("Mensaje inesperado");

                using(var lector = new StreamReader(texto, Encoding.UTF8))
                {
                    return await lector.ReadToEndAsync();
                }
            }
        }
    }
}
