using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ClienteWeb.Models;
using Entidades.Core;
using LogicaNegocio.Core;

namespace ClienteWeb.Controllers;

public class HomeController : Controller
{
    public IActionResult GetMenu()
    {
    var opciones = new OpcionLN().ListaOpciones();
    return PartialView("_MenuPartial", opciones);
    }   
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
