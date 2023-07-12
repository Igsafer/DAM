using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPV_IgnacioSanz.BackEnd.Modelo;

namespace TPV_IgnacioSanz.BackEnd.Servicios
{
    public class ServicioCliente : ServicioGenerico<cliente>
    {
        private DbContext contexto;

        public ServicioCliente(DbContext context) : base(context)
        {
            contexto = context;
        }

        public int getLastId()
        {
            cliente cli = contexto.Set<cliente>().OrderByDescending(c => c.codigo).FirstOrDefault();
            return cli.codigo;
        }
    }
}
