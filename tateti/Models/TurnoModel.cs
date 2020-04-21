using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tateti.Models
{
    public class TurnoModel
    {
        public Guid Id { get; set; }
        public Guid IdUsuario { get; set; }
        public UsuarioModel Usuario { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
