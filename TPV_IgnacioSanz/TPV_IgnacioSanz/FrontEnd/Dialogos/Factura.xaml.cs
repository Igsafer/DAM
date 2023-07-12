using System.Collections.Generic;
using System.Windows;
using TPV_IgnacioSanz.BackEnd.Modelo;

namespace TPV_IgnacioSanz.FrontEnd.Dialogos
{
    /// <summary>
    /// Lógica de interacción para Factura.xaml
    /// </summary>

    public partial class Factura : Window
    {
        private tpvEntities tpvEnt;
        private usuario usuLogin;
        private List<venta_producto> listaCompra;
        private venta venta;
        private cliente cliente;

        public Factura(tpvEntities ent, usuario usu, List<venta_producto> lista, venta ven, cliente cli)
        {
            InitializeComponent();
            tpvEnt = ent;
            usuLogin = usu;
            listaCompra = lista;
            venta = ven;
            cliente = cli;
            inicializa();
        }

        /// <summary>
        /// Inicializa los objetos creados tanto en XAML como en CS
        /// </summary>
        private void inicializa()
        {
            dgFactura.ItemsSource = listaCompra;
            txtTotal.Text = venta.total.ToString();
            txtFecha.Text = venta.fecha.ToString();
            txtUsu.Text = usuLogin.nombre;
        }

        /// <summary>
        /// Cerramos la factura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {            
            Close();
            listaCompra.Clear();
        }

        /// <summary>
        /// Envía la factura por email al cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnviar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Factura enviada a " + cliente.email, "FACTURA", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
            listaCompra.Clear();
        }
    }
}
