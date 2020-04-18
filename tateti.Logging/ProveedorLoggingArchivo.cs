using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace tateti.Logging
{
    public class ProveedorLoggingArchivo : ILoggerProvider 
    {
        private readonly Func<string, LogLevel, bool> _filtro;
        private string _nombreArchivo;

        public ProveedorLoggingArchivo(Func<string, LogLevel, bool> filtro, string nombreArchivo)
        {
            _filtro = filtro;
            _nombreArchivo = nombreArchivo;
        }

        public ILogger CreateLogger(string nombreCategoria)
        {
            return new LogeadorArchivo(nombreCategoria, _filtro, _nombreArchivo);
        }

        public void Dispose() {  }
    }
}
