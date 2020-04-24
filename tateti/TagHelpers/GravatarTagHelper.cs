using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace tateti.TagHelpers
{
    public class GravatarTagHelper : TagHelper
    {
        private ILogger<GravatarTagHelper> _logger;

        public GravatarTagHelper(ILogger<GravatarTagHelper> logger)
        {
            _logger = logger;
        }

        public string Email { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            byte[] foto = null;

            if (ChequearEstaConectado())
            {
                foto = ObtenerFoto(Email);
            }
            else
            {
                foto = File.ReadAllBytes(Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot", "images", "sin-foto.png"));
            }
        }

        private bool ChequearEstaConectado()
        {
            try
            {
                using (var httpCliente = new HttpClient())
                {
                    var respuestaGravatar = httpCliente.GetAsync(
                        "http://www.gravatar.com/avatar/").Result;
                    return (respuestaGravatar.IsSuccessStatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"No se puede chequear el servicio Gravatar. Estado de servicio: " + ex);
                return false;
            }
        }

        private byte[] ObtenerFoto(string email)
        {
            var httpCliente = new HttpClient();
            return httpCliente.GetByteArrayAsync(
                new Uri($"http://www.gravatar.com/avatar/ { HashEmail(email) }")).Result;
        }

        private static string HashEmail(string email)
        {
            var md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(
                Encoding.ASCII.GetBytes(email.ToLower()));
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                stringBuilder.Append(data[i].ToString("x2"));
            }
            return stringBuilder.ToString();
        }

    }

}
