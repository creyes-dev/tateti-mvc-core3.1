using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tateti.Opciones
{
    public class ProveedorLoggingOpciones
    {
        public string Nombre { get; set; }
        public string Parametros { get; set; }
        public int LogLevel { get; set; }
    }

    public class LoggingOpciones
    {
        public ProveedorLoggingOpciones[] Proveedores { get; set; }
    }
}
