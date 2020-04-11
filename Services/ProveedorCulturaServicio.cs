using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace tateti.Services
{
    public class ProveedorCulturaServicio : RequestCultureProvider
    {
        private static readonly char[] _separadorCookies = new[] { '|' };
        private static readonly string _prefijoCultura = "c=";
        private static readonly string _uiPrefijoCultura = "uic=";

        public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (ObtenerCulturaPorQueryString(httpContext, out string culture))
                return new ProviderCultureResult(culture, culture);

            else if (ObtenerCulturaPorCookie(httpContext, out culture))
                return new ProviderCultureResult(culture, culture);

            else if (ObtenerCulturaPorSession(httpContext, out culture))
                return new ProviderCultureResult(culture, culture);

            return await NullProviderCultureResult;
        }

        private bool ObtenerCulturaPorQueryString(HttpContext contexto, out string cultura)
        {
            if (contexto == null)
            {
                throw new ArgumentNullException(nameof(contexto));
            }

            var request = contexto.Request;
            if (!request.QueryString.HasValue)
            {
                cultura = null;
                return false;
            }

            cultura = request.Query["cultura"];
            return true;
        }

        private bool ObtenerCulturaPorCookie(HttpContext contexto, out string culture)
        {
            if (contexto == null)
            {
                throw new ArgumentNullException(nameof(contexto));
            }
            var cookie = contexto.Request.Cookies["cultura"];
            if (string.IsNullOrEmpty(cookie))
            {
                culture = null;
                return false;
            }
            culture = ParsearCookie(cookie);
            return !string.IsNullOrEmpty(culture);
        }

        public static string ParsearCookie(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            var partes = value.Split(_separadorCookies, StringSplitOptions.RemoveEmptyEntries);
            
            if (partes.Length != 2) return null;
            var nombreCulturaPotencial = partes[0];
            var nombreUICulturaPotencial = partes[1];
            
            if (!nombreCulturaPotencial.StartsWith(_prefijoCultura) || !nombreUICulturaPotencial.StartsWith(_uiPrefijoCultura)) return null;
            
            var nombreCultura = nombreCulturaPotencial.Substring(_prefijoCultura.Length);
            var uiCulturaNombre = nombreUICulturaPotencial.Substring(_uiPrefijoCultura.Length);

            if (nombreCultura == null && uiCulturaNombre == null) return null;
            if (nombreCultura != null && uiCulturaNombre == null) uiCulturaNombre = nombreCultura;
            if (nombreCultura == null && uiCulturaNombre != null) nombreCultura = uiCulturaNombre;
            return nombreCultura;
        }

        private bool ObtenerCulturaPorSession(HttpContext httpContext, out string culture)
        {
            culture = httpContext.Session.GetString("cultura");
            return !string.IsNullOrEmpty(culture);
        }

    }
}
