using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using tateti.Logging;

namespace tateti.Extensiones
{
    public static class LoggingArchivoExtension
    {
        const long bytesTamanioArchivoPorDefecto = 1024 * 1024 * 1024;
        const int ConteoLimiteArchivoRetenido = 31;

        public static ILoggingBuilder AddFile(this ILoggingBuilder loggerBuilder, IConfigurationSection configuracion)
        {
            if (loggerBuilder == null) throw new ArgumentNullException(nameof(loggerBuilder));
            if (configuracion == null) throw new ArgumentNullException(nameof(configuracion));
            var nivelMinimo = LogLevel.Information;
            var levelSection = configuracion["Logging:LogLevel"];
            if (!string.IsNullOrWhiteSpace(levelSection))
            {
                if(!Enum.TryParse(levelSection, out nivelMinimo))
                {
                    System.Diagnostics.Debug.WriteLine("El minimo nivel '{0} es invalido", levelSection);
                    nivelMinimo = LogLevel.Information;
                }
            }

            return loggerBuilder.AddFile(configuracion["Logging:FilePath"], (category, logLevel) => (logLevel >= nivelMinimo), nivelMinimo);

            // return loggerBuilder.AddFile(configuracion);
        }

        public static ILoggingBuilder AddFile(this ILoggingBuilder loggerBuilder, string rutaArchivo, 
            Func<string, LogLevel, bool> filter, LogLevel nivelMinimo = LogLevel.Information)
        {
            if (String.IsNullOrEmpty(rutaArchivo)) throw new ArgumentNullException(nameof(rutaArchivo));
            var infoArchivo = new System.IO.FileInfo(rutaArchivo);

            if (!infoArchivo.Directory.Exists)
                infoArchivo.Directory.Create();

            loggerBuilder.AddProvider(new ProveedorLoggingArchivo(filter, rutaArchivo));

            return loggerBuilder;
        }

        public static ILoggingBuilder AddFile(this ILoggingBuilder
        loggerBuilder, string filePath, LogLevel minimumLevel = LogLevel.Information)
        {
            if (String.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));
            var fileInfo = new System.IO.FileInfo(filePath);
            if (!fileInfo.Directory.Exists) fileInfo.Directory.Create();
            loggerBuilder.AddProvider(new ProveedorLoggingArchivo((category, logLevel) => (logLevel >= minimumLevel), filePath));
            return loggerBuilder;
        }

    }
}
