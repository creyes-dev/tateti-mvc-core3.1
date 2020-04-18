using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace tateti.Logging
{
    public class LoggingArchivoManager
    {
        private string _nombreArchivo;

        public LoggingArchivoManager(string nombreArchivo)
        {
            _nombreArchivo = nombreArchivo;
        }

        static ReaderWriterLock locker = new ReaderWriterLock();

        public void InsertarLog(EntradaLog entradaLog)
        {
            var directorio = System.IO.Path.GetDirectoryName(_nombreArchivo);

            if (!System.IO.Directory.Exists(directorio))
                System.IO.Directory.CreateDirectory(directorio);

            try
            {
                locker.AcquireWriterLock(int.MaxValue);
                System.IO.File.AppendAllText(_nombreArchivo, $"{entradaLog.IdEvento} {entradaLog.NivelLog} {entradaLog.Mensaje}" + Environment.NewLine);
            }
            finally
            {
                locker.ReleaseWriterLock();
            }
        }

    }
}
