using Microsoft.AspNetCore.Mvc;
using Entidades.Core;
using LogicaNegocio.Core;

namespace ClienteWeb.Controllers;

public class UsuarioController : Controller
{
    // GET: Usuario/Index
    public IActionResult Index()
    {
        List<Usuario> lista = new UsuarioLN().ListaUsuarios();
        return View(lista);
    }

    // GET: Usuario/Create
    public IActionResult Create()
    {
        Usuario usuario = new Usuario();
        return View(usuario);
    }

    // POST: Usuario/Create
    [HttpPost]
    public IActionResult Create(Usuario usuario)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Encriptar la contraseña
                usuario.Clave = EncriptarClave(usuario.ClaveTexto);
                new UsuarioLN().InsertarUsuario(usuario);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(usuario);
            }
        }
        return View(usuario);
    }
    // GET: Usuario/Edit/5
public IActionResult Edit(int id)
{
    Usuario? usuario = new UsuarioLN().BuscarUsuario(id);
    if (usuario == null)
    {
        return NotFound();
    }
    return View(usuario);
}

// POST: Usuario/Edit/5
[HttpPost]
public IActionResult Edit(int id, Usuario usuario)
{
    if (id != usuario.IdUsuario)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            // Si se ingresó nueva contraseña, la encripta
            if (!string.IsNullOrEmpty(usuario.ClaveTexto))
            {
                usuario.Clave = EncriptarClave(usuario.ClaveTexto);
            }
            
            new UsuarioLN().ActualizarUsuario(usuario);
            return RedirectToAction("Index");
        }
        catch
        {
            return View(usuario);
        }
    }
    return View(usuario);
}

// GET: Usuario/Delete/5
public IActionResult Delete(int id)
{
    Usuario? usuario = new UsuarioLN().BuscarUsuario(id);
    if (usuario == null)
    {
        return NotFound();
    }
    return View(usuario);
}

// POST: Usuario/Delete/5
[HttpPost, ActionName("Delete")]
public IActionResult DeleteConfirmed(int id)
{
    new UsuarioLN().EliminarUsuario(id);
    return RedirectToAction("Index");
}

    // Método para encriptar contraseña usando SHA256
    private byte[] EncriptarClave(string claveTexto)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            return sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(claveTexto));
        }
    }
}