namespace Entidades.Core;

public class Usuario
{
    public int IdUsuario { get; set; }
    public string CodUsuario { get; set; } = "";
    public byte[] Clave { get; set; } = new byte[0];
    public string Nombres { get; set; } = "";
    
    // Adicional (no está en la BD)
    public string ClaveTexto { get; set; } = "";
    public string? Rol { get; set; }
}