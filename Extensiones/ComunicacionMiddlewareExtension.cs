using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using tateti.Middleware;

namespace tateti.Extensiones
{
    public static class ComunicacionMiddlewareExtension
    {
        // IAppllicationBuilder provee mecanismos para la configuración del 
        // pipeline del request de la aplicación
        public static IApplicationBuilder UsarMiddlewareComunicacion(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ComunicacionMiddleware>();
        }

    }
}
