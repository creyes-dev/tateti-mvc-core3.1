using System.Threading.Tasks;
using tateti.Models;

namespace tateti.Services
{
    public interface IUsuarioServicio
    {
        Task<bool> EstaEnLinea(string nombre);
        Task<bool> RegistrarUsuario(UsuarioModel usuario);
        Task<UsuarioModel> ObtenerUsuarioPorEmail(string email);
        Task ActualizarUsuario(UsuarioModel usuario);
    }
}