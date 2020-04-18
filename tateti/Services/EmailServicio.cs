using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using tateti.Opciones;

namespace tateti.Services
{
    public class EmailServicio : IEmailServicio
    {
        private EmailOpciones _emailOpciones;
        readonly ILogger<EmailServicio> _logger;

        public EmailServicio(IOptions<EmailOpciones> opciones, ILogger<EmailServicio> logger)
        {
            _emailOpciones = opciones.Value;
            _logger = logger;
        }

        public Task EnviarEmail(string emailDestino, string asunto, string mensaje)
        {
            try
            {
                _logger.LogInformation($"##Comenzar EnviarMail## Envio de mail a {emailDestino}");

                // using(var client = new SmtpClient(_emailOpciones.)
                using (var client = new SmtpClient(_emailOpciones.MailServer, int.Parse(_emailOpciones.MailPort)))
                {
                    if (bool.Parse(_emailOpciones.UseSSL) == true) client.EnableSsl = true;

                    if (!string.IsNullOrEmpty(_emailOpciones.UserId))
                        client.Credentials = new NetworkCredential(_emailOpciones.UserId, _emailOpciones.Password);

                    // TODO: Envío de mail
                    // client.Send(new MailMessage("ejemplo@ejemplo.com", emailDestino, asunto, mensaje));
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"No se pudo enviar el mail {ex}");
            }

            return Task.CompletedTask;
        }

    }
}
