using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPV_IgnacioSanz.BackEnd.Modelo;

namespace TPV_IgnacioSanz.BackEnd.Servicios
{
    public class ServicioProducto : ServicioGenerico<producto>
    {
        private DbContext contexto;

        public ServicioProducto(DbContext context) : base(context)
        {
            contexto = context;
        }

        public int getLastId()
        {
            producto pro = contexto.Set<producto>().OrderByDescending(p => p.codigo).FirstOrDefault();
            return pro.codigo;
        }


        public List<producto> listaProductosPorTipo(int cod)
        {
            return contexto.Set<producto>().Where(c => c.tipo == cod).ToList();
        }

        public List<string> getUbicaciones()
        {
            return new List<string> { "Nevera 1", "Nevera 2", "Nevera 3", "Nevera 4" };
        }

        public List<int> getIVAs()
        {
            return new List<int> { 0, 4, 10, 21 };
        }
    }
}
