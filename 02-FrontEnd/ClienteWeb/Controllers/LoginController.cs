using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Entidades.Core;
using LogicaNegocio.Core;
using Microsoft.AspNetCore.Http;

namespace ClienteWeb.Controllers;

public class LoginController : Controller
{
    // GET: Login/Index
    public IActionResult Index()
    {
        return View();
    }

    // POST: Login/Index
    [HttpPost]
    public IActionResult Index(string codUsuario, string claveTexto)
    {
        if (string.IsNullOrEmpty(codUsuario) || string.IsNullOrEmpty(claveTexto))
        {
            ViewBag.Error = "Ingrese usuario y contraseña";
            return View();
        }

        try
        {
            // Encriptar la contraseña ingresada
            byte[] claveEncriptada = EncriptarClave(claveTexto);

            // Buscar usuario en la BD
            Usuario? usuario = new LogicaNegocio.Core.UsuarioLN().BuscarUsuarioPorCredenciales(codUsuario, claveEncriptada);

            if (usuario != null)
            {
                // Crear la sesión (cookie)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.CodUsuario),
                    new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                    new Claim("Nombres", usuario.Nombres)
                };
                if (!string.IsNullOrEmpty(usuario.Rol))
                {
                 claims.Add(new Claim(ClaimTypes.Role, usuario.Rol));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddHours(8)
                };

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                    new ClaimsPrincipal(claimsIdentity), authProperties);

                // Guardar usuario en variable de sesión
                HttpContext.Session.SetString("UsuarioNombre", usuario.Nombres);
                HttpContext.Session.SetInt32("UsuarioId", usuario.IdUsuario);

                return RedirectToAction("Index", "Usuario");
            }
            else
            {
                ViewBag.Error = "Usuario o contraseña incorrectos";
                return View();
            }
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al iniciar sesión: " + ex.Message;
            return View();
        }
    }

    // Cerrar sesión
    public IActionResult Logout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Login");
    }

    // Método para encriptar contraseña
    private byte[] EncriptarClave(string claveTexto)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            return sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(claveTexto));
        }
    }
}