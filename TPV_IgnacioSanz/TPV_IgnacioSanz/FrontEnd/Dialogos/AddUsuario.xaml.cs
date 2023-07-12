using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NLog;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TPV_IgnacioSanz.BackEnd.Modelo;
using TPV_IgnacioSanz.BackEnd.Servicios;

namespace TPV_IgnacioSanz.FrontEnd.Dialogos
{
    /// <summary>
    /// Lógica de interacción para AddUsuario.xaml
    /// </summary>
    public partial class AddUsuario : MetroWindow
    {
        private tpvEntities tpvEnt;

        private ServicioUsuario servUsu;
        private ServicioRol servRol;
        private usuario usuNuevo;
        private static Logger log = LogManager.GetCurrentClassLogger();

        public AddUsuario(tpvEntities ent)
        {
            InitializeComponent();
            tpvEnt = ent;
            inicializa();
        }

        /// <summary>
        /// Inicializamos los objetos creados tanto en CS como XAML
        /// </summary>
        private void inicializa()
        {
            servUsu = new ServicioUsuario(tpvEnt);
            servRol = new ServicioRol(tpvEnt);

            comboRol.ItemsSource = servRol.getAll().ToList();
        }

        /// <summary>
        /// Cancelamos la operación y no añadimos usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        /// <summary>
        /// Guardamos el usuario si todo ha sido introducido correctamente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (valida())
            {
                txtErrorCampos.Text = "";

                recogeDatos();

                servUsu.add(usuNuevo);

                try
                {
                    servUsu.save();

                    await this.ShowMessageAsync("GESTIÓN USUARIOS",
                        "EL USUARIO SE HA GUARDADO CON ÉXITO");

                    DialogResult = true;
                }
                catch (DbUpdateException dbex)
                {
                    await this.ShowMessageAsync("GESTIÓN USUARIOS",
                        "NO SE HA PODIDO GUARDAR EL USUARIO");

                    log.Error(dbex.Message);
                    log.Error(dbex.InnerException);
                    log.Error(dbex.StackTrace);
                }
            }
            else
            {
                txtErrorCampos.Text = "Todos los campos son obligatorios";
            }
        }

        /// <summary>
        /// Asignamos al nuevo objeto usuario los datos aportados por el usuario
        /// </summary>
        public void recogeDatos()
        {
            usuNuevo = new usuario();

            usuNuevo.codigo = servUsu.getLastId() + 1;
            usuNuevo.nombre = txtNombre.Text;
            usuNuevo.apellidos = txtApellidos.Text;
            usuNuevo.login = txtUsername.Text;
            usuNuevo.contraseña = txtPassword.Text;
            usuNuevo.rol1 = (rol)comboRol.SelectedItem;
        }

        /// <summary>
        /// Comprobamos que los datos de los campos están en un formato correcto
        /// y que los campos obligatorios están rellenados
        /// </summary>
        /// <returns></returns>
        private bool valida()
        {
            bool correcto = true;

            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                muestraError(imgErrorNombre, txtNombre);
                correcto = false;
            }
            else
            {
                quitaError(imgErrorNombre, txtNombre);
            }


            if (string.IsNullOrEmpty(txtApellidos.Text))
            {
                muestraError(imgErrorApellidos, txtApellidos);
                correcto = false;
            }
            else
            {
                quitaError(imgErrorApellidos, txtApellidos);
            }


            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                muestraError(imgErrorUsername, txtUsername);
                correcto = false;
            }
            else
            {
                if (!servUsu.NombreUnico(txtUsername.Text))
                {
                    muestraError(imgErrorUsername, txtUsername);
                    txtErrorUnico.Text = "El username debe ser único";
                    correcto = false;
                    txtUsername.Focus();
                }
                else
                {
                    quitaError(imgErrorUsername, txtUsername);
                    txtErrorUnico.Text = "";
                }
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                muestraError(imgErrorPassword, txtPassword);
                correcto = false;
            }
            else
            {
                quitaError(imgErrorPassword, txtPassword);
            }


            if (comboRol.SelectedItem == null)
            {
                muestraError(imgErrorRol, comboRol);
                correcto = false;
            }
            else
            {
                quitaError(imgErrorRol, comboRol);
            }

            return correcto;

        }

        /// <summary>
        /// Resalta los campos donde haya algún error
        /// </summary>
        /// <param name="img"></param>
        /// <param name="control"></param>
        private void muestraError(Image img, Control control)
        {
            img.Visibility = Visibility.Visible;
            control.BorderBrush = Brushes.Red;
        }

        /// <summary>
        /// Quita el resaltado de los campos donde no hay errores
        /// </summary>
        /// <param name="img"></param>
        /// <param name="control"></param>
        private void quitaError(Image img, Control control)
        {
            img.Visibility = Visibility.Hidden;
            control.BorderBrush = Brushes.Gray;
        }
    }
}
