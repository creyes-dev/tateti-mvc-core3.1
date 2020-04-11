using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using tateti.Services;
using tateti.Extensiones;

namespace tateti
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // (*DirBro) services.AddDirectoryBrowser();     // Servicio de asp que permite a los usuarios ver carpetas y archivos
            services.AddControllersWithViews();
            services.AddSingleton<IUsuarioServicio, UsuarioServicio>();
            services.AddRouting();  // Servicio que se usa para trabajar con enrutamientos
            services.AddSession(obj => { // uso de variables de sesi�n
                obj.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            services.AddLocalization(opciones => opciones.ResourcesPath = "Localizacion");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Home/error");

            // (*DirBro) app.UseDirectoryBrowser();  // Hemos agregado el Middleware de asp que permite a los usuarios ver carpetas y archivos
            
            // app.UsarMiddlewareComunicacion(); // ???

            app.UseStaticFiles();   // Middleware de asp que permite que los archivos est�ticos sean p�blicos y cualquiera pueda accederlos
            app.UseSession();       // Activar el middleware de manejo de variables de sesi�n 
            
            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                // endpoints.MapRazorPages();  // ???
                endpoints.MapControllers(); // ?
            });

            /*
            // La ruta /RegistroUsuario/Index es rescrita a /NuevoUsuario
            var opciones = new RewriteOptions()
                .AddRewrite("NuevoUsuario", "/RegistroUsuario/Index", false);
            app.UseRewriter(opciones);

            // (Enrutamiento, agregamos la nueva ruta CrearUsuario)

            var constructorRutas = new RouteBuilder(app);

            constructorRutas.MapGet("CrearUsuario", contexto =>
            {
                var usuario = contexto.Request.Query["Nombre"];
                var apellido = contexto.Request.Query["Apellido"];
                var email = contexto.Request.Query["Email"];
                var password = contexto.Request.Query["Email"];
                var servicio = contexto.RequestServices.GetService<IUsuarioServicio>();
                return contexto.Response.WriteAsync($"Usuario {usuario} {apellido} fue satisfactoriamente creado.");
            });
            
            var nuevaRuta = constructorRutas.Build();
            app.UseRouter(nuevaRuta);
            */

            app.UseWebSockets();

            app.UsarMiddlewareComunicacion(); // ?

            // Cultura
            var culturasSoportadas = CultureInfo.GetCultures(CultureTypes.AllCultures);
            var opcionesLocalizacion = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("es-AR"), // en-US
                SupportedCultures = culturasSoportadas,
                SupportedUICultures = culturasSoportadas
            };

            opcionesLocalizacion.RequestCultureProviders.Clear();
            opcionesLocalizacion.RequestCultureProviders.Add(new ProveedorCulturaServicio());
            app.UseRequestLocalization(opcionesLocalizacion);

            // Manejo de errores

            app.UseStatusCodePages("text/plain", "HTTP Error - Status Code: {0}"); // Manejar los errores muy basicamente
            app.UseStatusCodePagesWithRedirects("/error/{0}"); // Como enviar un mensaje de error que indica que el recurso ha sido movido temporalmente
            app.UseStatusCodePagesWithReExecute("/error/{0}"); // Como devolver un codigo http original al cliente y redirigirlo a una p�gina de usuario espec�fica

            /*
             * 
             *  Deshabilitar p�ginas de codigo de status http para request especificos 
                var statusCodePagesFeature =
                context.Features.Get<IStatusCodePagesFeature>();
                if (statusCodePagesFeature != null)
                {
                statusCodePagesFeature.Enabled = false;
                }
            */
        }
    }
}
