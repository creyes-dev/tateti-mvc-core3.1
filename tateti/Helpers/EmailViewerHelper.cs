using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using tateti.ViewEngines;

namespace tateti.Helpers
{
    public class EmailViewerHelper
    {
        IWebHostEnvironment _entornoHosting;
        IConfiguration _configuracionRoot;
        IHttpContextAccessor _httpContextAccessor;

        public async Task<string> RenderTemplate<T>(string plantilla, IWebHostEnvironment entorno, 
            IConfiguration configuracion, IHttpContextAccessor httpContexto, T modelo) where T : class
        {
            _entornoHosting = entorno;
            _configuracionRoot = configuracion;
            _httpContextAccessor = httpContexto;
            var renderer = httpContexto.HttpContext.RequestServices.GetRequiredService<IEmailViewEngine>();

            return await renderer.RenderEmailToString<T>(plantilla, modelo);
        }
    }
}
