using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using tateti.Opciones;

namespace tateti.Services
{
    public class EmailServicio : IEmailServicio
    {
        private EmailOpciones _emailOpciones;

        public EmailServicio(IOptions<EmailOpciones> opciones)
        {
            _emailOpciones = opciones.Value;
        }

        public Task EnviarEmail(string emailDestino, string asunto, string mensaje)
        {
            // using(var client = new SmtpClient(_emailOpciones.)
            using(var client = new SmtpClient(_emailOpciones.MailServer, int.Parse(_emailOpciones.MailPort)))
            {
                if (bool.Parse(_emailOpciones.UseSSL) == true) client.EnableSsl = true;

                if (!string.IsNullOrEmpty(_emailOpciones.UserId))
                    client.Credentials = new NetworkCredential(_emailOpciones.UserId, _emailOpciones.Password);
                
                // TODO: Envío de mail
                // client.Send(new MailMessage("ejemplo@ejemplo.com", emailDestino, asunto, mensaje));
            }
            return Task.CompletedTask;
        }

    }
}
