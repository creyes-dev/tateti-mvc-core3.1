using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tateti.Models
{
    public class SesionJuegoModel
    {
        public Guid Id { get; set; }
        public Guid IdUsuario1 { get; set; }
        public Guid IdUsuario2 { get; set; }
        public UsuarioModel Jugador1 { get; set; }
        public UsuarioModel Jugador2 { get; set; }
        public IEnumerable<TurnoModel> Turnos { get; set; }
        public UsuarioModel Ganador { get; set; }
        public UsuarioModel JugadorActivo { get; set; }
        public Guid IdGanador { get; set; }
        public Guid IdJugadorActivo { get; set; }
        public bool TurnoFinalizado { get; set; }
    }
}
