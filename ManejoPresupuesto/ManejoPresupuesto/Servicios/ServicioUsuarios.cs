

namespace ManejoPresupuesto.Servicios
{

 public  interface IServicioUsuarios
    {
        int OptenerUsuarioId();
    }
    public class ServicioUsuarios : IServicioUsuarios
    {
        public int OptenerUsuarioId()
        {
            return 1;
        }

    }
}