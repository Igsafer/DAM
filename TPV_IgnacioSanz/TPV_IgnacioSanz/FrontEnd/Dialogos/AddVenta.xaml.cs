using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NLog;
using System;
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
    /// Lógica de interacción para AddVenta.xaml
    /// </summary>    

    public partial class AddVenta : MetroWindow
    {
        private tpvEntities tpvEnt;
        private usuario usuLogin;
        private double total;

        private List<venta_producto> listaCompra;
        private List<venta> listaVentas;

        private ServicioVenta servVenta;
        private ServicioVentaProducto servVentaProd;
        private ServicioCliente servCli;

        private venta ventaNueva;
        private venta_producto ventaProdNueva;

        private static Logger log = LogManager.GetCurrentClassLogger();
        public AddVenta(tpvEntities ent, usuario usu, double totalVenta, List<venta_producto> lista)
        {
            InitializeComponent();
            tpvEnt = ent;
            usuLogin = usu;
            total = totalVenta;
            listaCompra = lista;
            inicializa();
        }

        /// <summary>
        /// Inicializa los objetos creados tanto en XAML como en CS
        /// </summary>
        private void inicializa()
        {

            txtFecha.SelectedDateTime = DateTime.Now;

            servVenta = new ServicioVenta(tpvEnt);
            servVentaProd = new ServicioVentaProducto(tpvEnt);
            servCli = new ServicioCliente(tpvEnt);

            listaVentas = servVenta.getAll().ToList();
            comboCliente.ItemsSource = servCli.getAll().ToList();
            comboCobro.ItemsSource = servVenta.getTiposCobro();

            txtTotal.Text = total.ToString();
        }

        /// <summary>
        /// Cancelamos la operación y no añadimos venta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        /// <summary>
        /// Guardamos la venta si todo ha sido introducido correctamente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (valida())
            {
                txtErrorCampos.Text = "";

                recogeDatos();
                guardarVentaProductos();

                servVenta.add(ventaNueva);

                try
                {
                    servVenta.save();

                    await this.ShowMessageAsync("GESTIÓN VENTAS",
                       "LA VENTA SE HA GUARDADO CON ÉXITO");

                    DialogResult = true;

                    MessageBoxResult resultado = MessageBox.Show("¿Desea una factura?", "FACTURA", MessageBoxButton.YesNo);

                    if (resultado == MessageBoxResult.Yes)
                    {
                        Factura diag = new Factura(tpvEnt, usuLogin, listaCompra, ventaNueva, ventaNueva.cliente1);
                        diag.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        diag.Show();
                    }
                    
                }
                catch (DbUpdateException dbex)
                {
                    await this.ShowMessageAsync("GESTIÓN VENTAS",
                        "NO SE HA PODIDO GUARDAR LA VENTA");

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
        /// Asignamos al nuevo objeto venta los datos aportados por el usuario
        /// </summary>
        private void recogeDatos()
        {
            ventaNueva = new venta();

            if(listaVentas.Count() == 0){ ventaNueva.codigo = 1; }
            else{ ventaNueva.codigo = servVenta.getLastId() + 1; }

            if(comboCliente.SelectedItem != null) ventaNueva.cliente1 = (cliente)comboCliente.SelectedItem;
            ventaNueva.fecha = (DateTime)txtFecha.SelectedDateTime;
            if(comboCobro.SelectedItem != null) ventaNueva.tipocobro = comboCobro.SelectedItem.ToString();
            ventaNueva.mensaje = txtMensaje.Text;
            ventaNueva.usuario1 = usuLogin;
            ventaNueva.total = total;
        }

        /// <summary>
        /// Se guargará un objeto venta_producto por cada producto distinto en la venta
        /// </summary>
        private void guardarVentaProductos()
        {
            for (int i = 0; i < listaCompra.Count(); i++)
            {
                ventaProdNueva = new venta_producto();

                ventaProdNueva.venta1 = ventaNueva;
                ventaProdNueva.producto1 = listaCompra[i].producto1;
                ventaProdNueva.cantidad = listaCompra[i].cantidad;
                ventaProdNueva.preciototal = listaCompra[i].preciototal;


                servVentaProd.add(ventaProdNueva);
            }
        }

        /// <summary>
        /// Comprobamos que los datos de los campos están en un formato correcto
        /// y que los campos obligatorios están rellenados
        /// </summary>
        /// <returns></returns>
        private bool valida()
        {
            bool correcto = true;

            if(txtFecha.SelectedDateTime == null)
            {
                muestraError(imgErrorFecha, txtFecha);
                correcto = false;
            }
            else
            {
                quitaError(imgErrorFecha, txtFecha);
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
