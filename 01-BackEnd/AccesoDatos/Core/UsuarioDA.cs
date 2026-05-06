using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Entidades.Core;
using System.IO;

namespace AccesoDatos.Core;

public class UsuarioDA
{
    private readonly string _connectionString;

    public UsuarioDA()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        _connectionString = configuration.GetConnectionString("SQL") ?? string.Empty;
    }

    public List<Usuario> ListaUsuarios()
    {
        List<Usuario> lista = new List<Usuario>();

        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            using (SqlCommand comando = new SqlCommand("paUsuarioLista", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                conexion.Open();

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Usuario usuario = new Usuario();
                        usuario.IdUsuario = reader.GetInt32(0);
                        usuario.CodUsuario = reader.GetString(1);
                        usuario.Nombres = reader.GetString(2);
                        lista.Add(usuario);
                    }
                }
            }
        }
        return lista;
    }

    public void InsertarUsuario(Usuario usuario)
    {
        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            using (SqlCommand comando = new SqlCommand("paUsuario_insertar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@CodUsuario", usuario.CodUsuario);
                comando.Parameters.AddWithValue("@Clave", usuario.Clave);
                comando.Parameters.AddWithValue("@Nombres", usuario.Nombres);
                
                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }
    }

    public Usuario? BuscarUsuario(int id)
    {
        Usuario usuario = default!;
        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            using (SqlCommand comando = new SqlCommand("paUsuarioBuscar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@IdUsuario", id);
                conexion.Open();
                
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new Usuario();
                        usuario.IdUsuario = reader.GetInt32(0);
                        usuario.CodUsuario = reader.GetString(1);
                        usuario.Nombres = reader.GetString(2);
                    }
                }
            }
        }
        return usuario;
    }

    public void ActualizarUsuario(Usuario usuario)
    {
        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            using (SqlCommand comando = new SqlCommand("paUsuarioActualizar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                comando.Parameters.AddWithValue("@CodUsuario", usuario.CodUsuario);
                comando.Parameters.AddWithValue("@Nombres", usuario.Nombres);
                comando.Parameters.AddWithValue("@Clave", usuario.Clave ?? (object)DBNull.Value);
                
                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }
    }

    public void EliminarUsuario(int id)
    {
        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            using (SqlCommand comando = new SqlCommand("paUsuarioEliminar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@IdUsuario", id);
                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }
    }

    // ✅ Método para LOGIN
    public Usuario? BuscarUsuarioPorCredenciales(string codUsuario, byte[] clave)
    {
        Usuario? usuario = null;
        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            using (SqlCommand comando = new SqlCommand("paUsuario_BuscaCodUserClave", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@CodUsuario", codUsuario);
                comando.Parameters.AddWithValue("@Clave", clave);
                conexion.Open();

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new Usuario();
                        usuario.IdUsuario = reader.GetInt32(0);
                        usuario.CodUsuario = reader.GetString(1);
                        usuario.Nombres = reader.GetString(2);
                        usuario.Rol = reader.IsDBNull(3) ? null : reader.GetString(3);
                    }
                }
            }
        }
        return usuario;
    }
}