using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tateti.Models;

namespace tateti.Services
{
    public class UsuarioServicio : IUsuarioServicio
    {
        public Task<bool> RegistrarUsuario(UsuarioModel usuario)
        {
            return Task.FromResult(true);
        }

        public Task<bool> EstaEnLinea(string nombre)
        {
            return Task.FromResult(true);
        }

    }
}
