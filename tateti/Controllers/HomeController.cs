using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace tateti.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var cultura = Request.HttpContext.Session.GetString("cultura");
            ViewBag.cultura = cultura;

            return View();
        }

        public IActionResult SetCultura(string cultura)
        {
            // La cultura es almacenada en una variable de sesión
            Request.HttpContext.Session.SetString("cultura", cultura);
            return RedirectToAction("Index");
        }
    }
}