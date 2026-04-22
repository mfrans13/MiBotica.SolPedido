using Entidades.Core;
using AccesoDatos.Core;

namespace LogicaNegocio.Core;

public class UsuarioLN
{
    public List<Usuario> ListaUsuarios()
    {
        return new UsuarioDA().ListaUsuarios();
    }

    public void InsertarUsuario(Usuario usuario)
    {
        new UsuarioDA().InsertarUsuario(usuario);
    }

    public Usuario BuscarUsuario(int id)
    {
        return new UsuarioDA().BuscarUsuario(id);
    }

    public void ActualizarUsuario(Usuario usuario)
    {
        new UsuarioDA().ActualizarUsuario(usuario);
    }

    public void EliminarUsuario(int id)
    {
        new UsuarioDA().EliminarUsuario(id);
    }
}