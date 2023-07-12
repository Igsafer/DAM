using System;
using System.Data.Entity;
using System.Linq;
using TPV_IgnacioSanz.BackEnd.Modelo;

namespace TPV_IgnacioSanz.BackEnd.Servicios
{
    public class ServicioUsuario : ServicioGenerico<usuario>
    {
        private DbContext contexto;

        public ServicioUsuario(DbContext context) : base(context)
        {
            contexto = context;
        }
        public usuario usuLogin { get; set; }

        public int getLastId()
        {
            usuario usu = contexto.Set<usuario>().OrderByDescending(u => u.codigo).FirstOrDefault();
            return usu.codigo;
        }

        public bool NombreUnico(string nombre)
        {
            return contexto.Set<usuario>().Where(e => e.login == nombre).Count() == 0;
        }

        /// <summary>
        /// Método para comprobar las credenciales del usuario
        /// </summary>
        /// <param name="user">Nombre de usuario</param>
        /// <param name="pass">Contraseña</param>
        /// <returns></returns>
        public bool login(string user, string pass)
        {
            bool correcto = true;

            try
            {
                usuLogin = contexto.Set<usuario>().Where(u => u.login == user).FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            if (usuLogin == null)
            {
                correcto = false;
            }
            else if (!usuLogin.login.Equals(user) || !usuLogin.contraseña.Equals(pass))
            {
                correcto = false;
            }

            return correcto;
        }
    }
}
