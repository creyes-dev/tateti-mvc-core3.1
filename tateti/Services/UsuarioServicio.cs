using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using tateti.Models;

namespace tateti.Services
{
    public class UsuarioServicio : IUsuarioServicio
    {
        // ConcurrentBag de usuarios, colección de usuarios que es segura 
        // en cuanto a la concurrencia de hilos
        private static ConcurrentBag<UsuarioModel> _usuarios;

        public UsuarioServicio()
        {
            _usuarios = new ConcurrentBag<UsuarioModel>();
        }

        public Task<bool> RegistrarUsuario(UsuarioModel usuario)
        {
            _usuarios.Add(usuario);
            return Task.FromResult(true);
        }

        public Task<bool> EstaEnLinea(string nombre)
        {
            return Task.FromResult(true);
        }

        public Task<UsuarioModel> ObtenerUsuarioPorEmail(string email)
        {
            return Task.FromResult(_usuarios.FirstOrDefault(
                u => u.Email == email));
        }
        public Task ActualizarUsuario(UsuarioModel usuario)
        {
            _usuarios = new ConcurrentBag<UsuarioModel>(_usuarios.Where
                (u => u.Email != usuario.Email))
            {
                usuario
            };
            return Task.CompletedTask;
        }

    }
}
