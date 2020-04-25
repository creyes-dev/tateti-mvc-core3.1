using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using tateti.Helpers;

namespace tateti.Services
{
    public class EmailRenderPlantillaServicio : IEmailRenderPlantillaServicio
    {
        private IWebHostEnvironment _hostingEnvironment;
        private IConfiguration _configuracion;
        private IHttpContextAccessor _httpContextAccessor;

        public EmailRenderPlantillaServicio(IWebHostEnvironment entorno, IConfiguration configuracion,
            IHttpContextAccessor accesorContexto)
        {
            _hostingEnvironment = entorno;
            _configuracion = configuracion;
            _httpContextAccessor = accesorContexto;
        }

        public async Task<string> RenderearTemplate<T>(string nombrePlantilla, T model, string host) where T : class
        {
            var html = await new EmailViewerHelper().RenderTemplate(nombrePlantilla, _hostingEnvironment, _configuracion, _httpContextAccessor, model);
            var targetDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Emails");

            if (!Directory.Exists(targetDir))
                Directory.CreateDirectory(targetDir);
            string dateTime = DateTime.Now.ToString("ddMMHHyyHHmmss");
            var targetFileName = Path.Combine(targetDir, nombrePlantilla.Replace("\\", "_") + "." + dateTime + ".html");
            html = html.Replace("{ViewOnline}", $"{host.TrimEnd('/')}/Emails/{Path.GetFileName(targetFileName)}");
            html = html.Replace("{ServerUrl}", host);
            File.WriteAllText(targetFileName, html);
            return html;
        }

    }
}
