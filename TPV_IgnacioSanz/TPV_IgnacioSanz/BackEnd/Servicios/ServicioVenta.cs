using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPV_IgnacioSanz.BackEnd.Modelo;

namespace TPV_IgnacioSanz.BackEnd.Servicios
{
    public class ServicioVenta : ServicioGenerico<venta>
    {
        private DbContext contexto;

        public ServicioVenta(DbContext context) : base(context)
        {
            contexto = context;
        }

        public int getLastId()
        {
            venta venta = contexto.Set<venta>().OrderByDescending(v => v.codigo).FirstOrDefault();
            return venta.codigo;
        }

        public List<string> getTiposCobro()
        {
            return new List<string> { "Tarjeta", "Efectivo", "Bizum" };
        }
    }
}
