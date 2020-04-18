using System;
using System.Collections.Generic;
using System.Text;

namespace tateti.Logging
{
    public class EntradaLog
    {
        public int IdEvento { get; internal set; }
        public string Mensaje { get; internal set; }

        public string NivelLog { get; internal set; }

        public DateTime FechaCreacion { get; set; }
    }
}
