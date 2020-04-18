using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace tateti.Logging
{
    public class LogeadorArchivo : ILogger
    {
        private string _nombreCategoria;
        private Func<string, LogLevel, bool> _filtro;
        private string _nombreArchivo;
        private LoggingArchivoManager _loggingManager;

        public LogeadorArchivo(string nombreCategoria,
            Func<string, LogLevel, bool> filtro, string nombreArchivo)
        {
            _nombreCategoria = nombreCategoria;
            _filtro = filtro;
            _nombreArchivo = nombreArchivo;
            _loggingManager = new LoggingArchivoManager(nombreArchivo);
        }

        public IDisposable BeginScope<IState>(IState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return (_filtro == null || _filtro(_nombreCategoria, logLevel));
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));

            var mensaje = formatter(state, exception);
            if (string.IsNullOrEmpty(mensaje)) return;
            if (exception != null) mensaje += "\n" + exception.ToString();

            var entrada = new EntradaLog
            {
                Mensaje = mensaje,
                IdEvento = eventId.Id,
                NivelLog = logLevel.ToString(),
                FechaCreacion = DateTime.UtcNow
            };
            _loggingManager.InsertarLog(entrada);
        }



    }
}
