using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NLog;
using System.Collections.Generic;
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
    /// Lógica de interacción para AddCliente.xaml
    /// </summary>

    public partial class AddCliente : MetroWindow
    {
        private tpvEntities tpvEnt;
        private ServicioCliente servCli;
        private cliente clienteNuevo;
        private List<cliente> listaClientes;
        private static Logger log = LogManager.GetCurrentClassLogger();

        public AddCliente(tpvEntities ent)
        {
            InitializeComponent();
            tpvEnt = ent;
            inicializa();
        }

        /// <summary>
        /// Inicializamos los objetos creados
        /// </summary>
        private void inicializa()
        {
            servCli = new ServicioCliente(tpvEnt);

            listaClientes = servCli.getAll().ToList();
        }

        /// <summary>
        /// Cancelamos la operación y no añadimos cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        /// <summary>
        /// Guardamos el cliente si todo ha sido introducido correctamente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (valida())
            {
                txtErrorCampos.Text = "";

                recogeDatos();

                servCli.add(clienteNuevo);

                try
                {
                    servCli.save();

                    await this.ShowMessageAsync("GESTIÓN CLIENTES",
                        "CLIENTE GUARDADO CON ÉXITO");

                    DialogResult = true;
                }
                catch (DbUpdateException dbex)
                {
                    await this.ShowMessageAsync("GESTIÓN CLIENTES",
                        "NO SE PUDO GUARDAR EL CLIENTE");

                    log.Error(dbex.Message);
                    log.Error(dbex.InnerException);
                    log.Error(dbex.StackTrace);
                }
            }
            else
            {
                txtErrorCampos.Text = "Hay campos obligatorios sin rellenar";
            }
        }

        /// <summary>
        /// Asignamos al nuevo objeto cliente los datos aportados por el usuario
        /// </summary>
        private void recogeDatos()
        {
            clienteNuevo = new cliente();

            if (listaClientes.Count() == 0) clienteNuevo.codigo = 1;
            else { clienteNuevo.codigo = servCli.getLastId() + 1; }

            clienteNuevo.nombre = txtNombre.Text;
            clienteNuevo.apellidos = txtApellido.Text;
            clienteNuevo.email = txtEmail.Text;
            clienteNuevo.direccion = txtDireccion.Text;
            clienteNuevo.nombrecompleto = txtNombre.Text + " " + txtApellido.Text;
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
            else { quitaError(imgErrorNombre, txtNombre); }

            if (string.IsNullOrEmpty(txtApellido.Text))
            {
                muestraError(imgErrorApellido, txtApellido);
                correcto = false;
            }
            else { quitaError(imgErrorApellido, txtApellido); }

            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                muestraError(imgErrorEmail, txtEmail);
                correcto = false;
            }
            else { quitaError(imgErrorEmail, txtEmail); }

            if (string.IsNullOrEmpty(txtDireccion.Text))
            {
                muestraError(imgErrorDireccion, txtDireccion);
                correcto = false;
            }
            else { quitaError(imgErrorDireccion, txtDireccion); }

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
