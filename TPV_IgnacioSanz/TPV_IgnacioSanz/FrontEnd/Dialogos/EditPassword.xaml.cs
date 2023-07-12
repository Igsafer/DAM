using NLog;
using System.Data.Entity.Infrastructure;
using System.Windows;
using System.Windows.Media;
using TPV_IgnacioSanz.BackEnd.Modelo;
using TPV_IgnacioSanz.BackEnd.Servicios;

namespace TPV_IgnacioSanz.FrontEnd.Dialogos
{
    /// <summary>
    /// Lógica de interacción para EditPassword.xaml
    /// </summary>
    
    public partial class EditPassword : Window
    {
        private tpvEntities tpvEnt;
        private ServicioUsuario servUsu;
        private usuario usuSeleccionado;
        private static Logger log = LogManager.GetCurrentClassLogger();

        public EditPassword(tpvEntities ent, usuario usu)
        {
            InitializeComponent();
            tpvEnt = ent;
            usuSeleccionado = usu;
            inicializa();
        }

        /// <summary>
        /// Inicializa los objetos creados
        /// </summary>
        private void inicializa()
        {
            servUsu = new ServicioUsuario(tpvEnt);            
        }

        /// <summary>
        /// Cancelamos la operación y no editamos contraseña
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        /// <summary>
        /// Guardamos la contraseña si ha sido introducida correctamente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (valida())
            {
                txtError.Text = "";

                recogeDatos();

                servUsu.edit(usuSeleccionado);

                try
                {
                    servUsu.save();

                    MessageBox.Show("Contraseña modificada con éxito", "GESTIÓN USUARIOS", MessageBoxButton.OK);

                    DialogResult = true;
                }
                catch (DbUpdateException dbex)
                {
                    MessageBox.Show("No se pudo cambiar la contraseña", "GESTIÓN USUARIOS", MessageBoxButton.OK, MessageBoxImage.Error);

                    log.Error(dbex.Message);
                    log.Error(dbex.InnerException);
                    log.Error(dbex.StackTrace);
                }
            }
        }

        /// <summary>
        /// Asignamos al objeto usuario la nueva contraseña aportada por el usuario
        /// </summary>
        private void recogeDatos()
        {
            usuSeleccionado.contraseña = txtNewPass.Password;
        }

        /// <summary>
        /// Comprobamos que la contraseña nueva:
        /// 1) No está vacía
        /// 2) Está correctamente confirmada
        /// 3) No es igual a la antigua
        /// </summary>
        /// <returns></returns>
        private bool valida()
        {
            bool correcto = true;

            string oldPass = usuSeleccionado.contraseña;

            //La contraseña nueva no está vacía
            if (string.IsNullOrEmpty(txtNewPass.Password))
            {
                txtNewPass.BorderBrush = Brushes.Red;
                correcto = false;
                txtError.Text = "Hay campos sin rellenar";
            }
            else
            {
                txtNewPass.BorderBrush = Brushes.Gray;
            }

            //La confirmación tampoco está vacía
            if (string.IsNullOrEmpty(txtConfirmarPass.Password))
            {
                txtConfirmarPass.BorderBrush = Brushes.Red;
                correcto = false;
                txtError.Text = "Hay campos sin rellenar";
            }
            else
            {
                txtConfirmarPass.BorderBrush = Brushes.Gray;
            }

            //Ambos campos coinciden
            if(txtNewPass.Password != txtConfirmarPass.Password)
            {
                txtNewPass.BorderBrush = Brushes.Red;
                txtConfirmarPass.BorderBrush = Brushes.Red;
                correcto = false;
                txtError.Text = "Las contraseñas nuevas no coinciden";
            }
            else
            {
                txtNewPass.BorderBrush = Brushes.Gray;
                txtConfirmarPass.BorderBrush = Brushes.Gray;

                //La contraseña nueva es distinta a la antigua
                if (txtNewPass.Password == oldPass)
                {
                    txtNewPass.BorderBrush = Brushes.Red;
                    txtConfirmarPass.BorderBrush = Brushes.Red;
                    correcto = false;
                    txtError.Text = "La nueva contraseña no puede ser la antigua";
                }
                else
                {
                    txtNewPass.BorderBrush = Brushes.Gray;
                }
            }

            return correcto;
        }
    }
}
