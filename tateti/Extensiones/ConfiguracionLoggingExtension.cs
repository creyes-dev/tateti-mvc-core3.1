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

            foreach(var provider in loggingOpciones.Proveedores)
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
