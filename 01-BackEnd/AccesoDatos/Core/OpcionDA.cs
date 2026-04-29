using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Entidades.Core;
using System.IO;

namespace AccesoDatos.Core;

public class OpcionDA
{
    private readonly string _connectionString;

    public OpcionDA()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        _connectionString = configuration.GetConnectionString("SQL") ?? string.Empty;
    }

    public List<Opcion> ListaOpciones()
    {
        List<Opcion> lista = new List<Opcion>();

        using (SqlConnection conexion = new SqlConnection(_connectionString))
        {
            using (SqlCommand comando = new SqlCommand("paOpcionLista", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                conexion.Open();

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Opcion opcion = new Opcion();
                        opcion.IdOpcion = reader.GetInt32(0);
                        opcion.NombreOpcion = reader.GetString(1);
                        opcion.UrlOpcion = reader.GetString(2);
                        opcion.RutaImagen = reader.GetString(3);
                        opcion.NroOrden = reader.GetInt32(4);
                        opcion.IdOpcionRef = reader.GetInt32(5);
                        lista.Add(opcion);
                    }
                }
            }
        }
        return lista;
    }
}