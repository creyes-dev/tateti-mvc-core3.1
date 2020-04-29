using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using tateti.Logging;
using tateti.Models;
using tateti.Services;
using Xunit;

namespace tateti.testing
{
    public class UsuarioServicioTests
    {
        [Theory]
        [InlineData("test@test.com", "test", "test", "test123!")]
        [InlineData("test1@test.com", "test1", "test1", "test123!")]
        [InlineData("test2@test.com", "test2", "test2", "test123!")]
        public async Task DeberiaAgregarUsuario(string email, string nombre, string apellido, string password)
        {
            var usuarioModel = new UsuarioModel
            {
                Email = email,
                Nombre = nombre,
                Apellido = apellido,
                Password = password
            };

            var usuarioService = new UsuarioServicio();
            var nuevoUsuario = await usuarioService.RegistrarUsuario(usuarioModel);
            Assert.True(nuevoUsuario);
        }

    }
}
