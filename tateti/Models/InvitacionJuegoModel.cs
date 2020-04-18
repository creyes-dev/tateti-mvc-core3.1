using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tateti.Models
{
    public class InvitacionJuegoModel
    {
        public Guid Id { get; set; }
        public string EmailDestino { get; set; }
        public string InvitadoPor { get; set; }
        public bool EstaConfirmado { get; set; }
        public DateTime FechaConfirmacion { get; set; }
    }
}
