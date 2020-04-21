using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using tateti.Models;

namespace tateti.Services
{
    public class SesionJuegoServicio : ISesionJuegoServicio
    {
        private static ConcurrentBag<SesionJuegoModel> _sesiones;
        private IUsuarioServicio _usuarioServicio;

        public SesionJuegoServicio(IUsuarioServicio usuarioServicio)
        {
            _sesiones = new ConcurrentBag<SesionJuegoModel>();
            _usuarioServicio = usuarioServicio;
        }

        public Task<SesionJuegoModel> ObtenerSesionJuego(Guid idSesionJuego)
        {
            return Task.Run(() => _sesiones.FirstOrDefault(
                x => x.Id == idSesionJuego));
        }

        public async Task<SesionJuegoModel> CrearSesionJuego(Guid idInvitacion, string emailInvitadoPor, string invitadoEmail)
        {
            var invitadoPor = await _usuarioServicio.ObtenerUsuarioPorEmail(emailInvitadoPor);
            var jugadorInvitado = await _usuarioServicio.ObtenerUsuarioPorEmail(invitadoEmail);

            SesionJuegoModel sesion = new SesionJuegoModel
            {
                Jugador1 = invitadoPor,
                Jugador2 = jugadorInvitado,
                Id = idInvitacion,
                JugadorActivo = invitadoPor
            };

            _sesiones.Add(sesion);
            return sesion;
        }

        /*
                public async Task<GameSessionModel> CreateGameSession(Guid invitationId, string invitedByEmail, string invitedPlayerEmail)
        {
            var invitedBy = await _UserService.GetUserByEmail(invitedByEmail);
            var invitedPlayer = await _UserService.GetUserByEmail(invitedPlayerEmail);

            GameSessionModel session = new GameSessionModel
            {
                User1 = invitedBy,
                User2 = invitedPlayer,
                Id = invitationId,
                ActiveUser = invitedBy
            };

            _sessions.Add(session);
            return session;
        }

    */


    }
}
