using Microsoft.AspNetCore.Mvc;
using Entidades.Core;
using LogicaNegocio.Core;

namespace ClienteWeb.Components;

public class MenuViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var opciones = new OpcionLN().ListaOpciones();
        return View(opciones);
    }
}