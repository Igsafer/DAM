using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPV_IgnacioSanz.BackEnd.Modelo;

namespace TPV_IgnacioSanz.BackEnd.Servicios
{
    public class ServicioTipoProducto : ServicioGenerico<tipoproducto>
    {
        private DbContext contexto;
        public ServicioTipoProducto(DbContext context) : base(context)
        {
            contexto = context;
        }


    }
}
