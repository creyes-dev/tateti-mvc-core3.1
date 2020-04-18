using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tateti.Opciones;

namespace tateti.Extensiones
{
    public static class ConfiguracionLoggingExtension
    {
        public static ILoggingBuilder AddLoggingConfiguration(this ILoggingBuilder loggingBuilder, 
            IConfiguration configuracion)
        {
            var loggingOpciones = new LoggingOpciones();
            configuracion.GetSection("Logging").Bind(loggingOpciones);

            // TODO: Confusión. Agregar aquí un proveedor, porque no se en donde se agrega!
            ProveedorLoggingOpciones opciones = new ProveedorLoggingOpciones();
            opciones.Nombre = "file";
            opciones.LogLevel = 1;

            ProveedorLoggingOpciones[] proveedorOpciones = new ProveedorLoggingOpciones[] { opciones };
            loggingOpciones.Proveedores = proveedorOpciones;

            foreach (var provider in loggingOpciones.Proveedores)
            {
                switch(provider.Nombre.ToLower())
                {
                    case "console": { loggingBuilder.AddConsole(); break; }
                    case "file": { string rutaArchivo = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "logs", $"tateti_{System.DateTime.Now.ToString("ddMMyyHHmm")}.log");
                            loggingBuilder.AddFile(rutaArchivo, (LogLevel)provider.LogLevel);
                            break;
                        }
                    default: { break;  }
                }
            }
            return loggingBuilder;
        }
    }
}
