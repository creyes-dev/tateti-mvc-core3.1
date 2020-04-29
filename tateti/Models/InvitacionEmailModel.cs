using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tateti.Models
{
    public class InvitacionEmailModel
    {
        public string Nombre { get; set; }
        public UsuarioModel InvitadoPor { get; set; }
        public DateTime FechaInvitacion { get; set; }
        public string ConfirmacionUrl { get; set; }
    }
}
