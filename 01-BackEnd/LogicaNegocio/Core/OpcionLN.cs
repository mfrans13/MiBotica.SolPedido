using Entidades.Core;
using AccesoDatos.Core;

namespace LogicaNegocio.Core;

public class OpcionLN
{
    public List<Opcion> ListaOpciones()
    {
        return new OpcionDA().ListaOpciones();
    }
}