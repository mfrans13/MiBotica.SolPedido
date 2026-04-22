namespace Entidades.Core;

public class Opcion
{
    public int IdOpcion { get; set; }
    public string NombreOpcion { get; set; } = "";
    public string UrlOpcion { get; set; } = "";
    public string RutaImagen { get; set; } = "";
    public int NroOrden { get; set; }
    public int IdOpcionRef { get; set; }
    
    // Adicionales para navegación
    public string Area { get; set; } = "";
    public string Controladora { get; set; } = "";
    public string Accion { get; set; } = "";
}