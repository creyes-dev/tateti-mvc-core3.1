using System;
using System.Threading.Tasks;
using tateti.Models;

namespace tateti.Services
{
    public interface ISesionJuegoServicio
    {
        Task<SesionJuegoModel> ObtenerSesionJuego(Guid idSesionJuego);
        Task<SesionJuegoModel> CrearSesionJuego(Guid idInvitacion, string emailInvitadoPor, string invitadoEmail);
        Task<SesionJuegoModel> AgregarTurno(Guid id, string email, int x, int y);
    }
}