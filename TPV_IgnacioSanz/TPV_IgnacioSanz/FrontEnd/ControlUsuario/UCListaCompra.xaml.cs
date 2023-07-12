using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TPV_IgnacioSanz.BackEnd.Modelo;
using TPV_IgnacioSanz.FrontEnd.Dialogos;

namespace TPV_IgnacioSanz.FrontEnd.ControlUsuario
{
    /// <summary>
    /// Lógica de interacción para UCListaCompra.xaml
    /// </summary>

    public partial class UCListaCompra : UserControl
    {
        private tpvEntities tpvEnt;
        private usuario usuLogin;
        private List<venta_producto> listaCompra;
        public UCListaCompra(tpvEntities ent, List<venta_producto> lista, usuario usu)
        {
            InitializeComponent();
            tpvEnt = ent;
            listaCompra = lista;
            usuLogin = usu;
            inicializa();
        }

        /// <summary>
        /// Inicializa los objetos creados tanto en el CS como en el XAML
        /// </summary>
        private void inicializa()
        {
            dgListaCompra.ItemsSource = listaCompra;
            totalVenta();
        }


        /// <summary>
        /// Recalcula el total actual de la venta por efectuar
        /// </summary>
        private void totalVenta()
        {
            double total = 0;

            for (int i = 0; i < listaCompra.Count(); i++)
            {
                total += listaCompra[i].preciototal.GetValueOrDefault();
            }

            txtTotal.Text = total.ToString();
        }

        /// <summary>
        /// Método para acceder al diálogo de añadir una venta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnComprar_Click(object sender, RoutedEventArgs e)
        {
            double total = double.Parse(txtTotal.Text.ToString());

            if(listaCompra.Count != 0)
            {
                AddVenta diag = new AddVenta(tpvEnt, usuLogin, total, listaCompra);
                diag.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                diag.ShowDialog();
            }
            else
            {
                MessageBox.Show("No hay productos en la lista de la compra", "GESTIÓN DE VENTAS", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Método para borrar toda la lista de la compra
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBorrarTodo_Click(object sender, RoutedEventArgs e)
        {
            foreach(venta_producto ventaProd in listaCompra)
            {
                producto prod = ventaProd.producto1;

                prod.cantidad += (int)ventaProd.cantidad;
            }

            listaCompra.Clear();
            dgListaCompra.Items.Refresh();

            txtTotal.Text = "0";
        }
                
        /// <summary>
        /// Quitar un producto seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuitarProducto_Click(object sender, RoutedEventArgs e)
        {
            venta_producto ventaProd = (venta_producto)dgListaCompra.SelectedItem;

            if (dgListaCompra.SelectedItem != null)
            {
                producto prod = ventaProd.producto1;
                prod.cantidad += (int)ventaProd.cantidad;

                listaCompra.Remove(ventaProd);
                txtTotal.Text = "";
                totalVenta();
                dgListaCompra.Items.Refresh();
            }
        }
                
        /// <summary>
        /// Quitar una unidad del producto seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuitar1_Click(object sender, RoutedEventArgs e)
        {
            venta_producto ventaProd = (venta_producto)dgListaCompra.SelectedItem;

            if (dgListaCompra.SelectedItem != null)
            {
                producto prod = ventaProd.producto1;

                ventaProd.cantidad--;

                prod.cantidad++;

                //Si la cantidad del producto llega a 0 es retirado de la lista
                if(ventaProd.cantidad <= 0)
                {
                    listaCompra.Remove(ventaProd);
                }
                else
                {
                    ventaProd.preciototal -= prod.precio;
                }

                txtTotal.Text = "";
                totalVenta();
                dgListaCompra.Items.Refresh();
            }
        }

        /// <summary>
        /// Método para agregar 1 unidad al producto seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            venta_producto ventaProd = (venta_producto)dgListaCompra.SelectedItem;

            if (dgListaCompra.SelectedItem != null)
            {
                producto prod = ventaProd.producto1;

                ventaProd.cantidad++;
                ventaProd.preciototal += prod.precio;

                prod.cantidad--;

                txtTotal.Text = "";
                totalVenta();
                dgListaCompra.Items.Refresh();
            }
        }

        /// <summary>
        /// Método para agregar 2 unidades al producto seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            venta_producto ventaProd = (venta_producto)dgListaCompra.SelectedItem;             

            if (dgListaCompra.SelectedItem != null)
            {
                producto prod = ventaProd.producto1;

                if(prod.cantidad >= 2)
                {
                    ventaProd.cantidad = ventaProd.cantidad + 2;
                    ventaProd.preciototal = ventaProd.preciototal + (prod.precio * 2);

                    prod.cantidad -= 2;

                    txtTotal.Text = "";
                    totalVenta();
                    dgListaCompra.Items.Refresh();
                }
            }
        }

        /// <summary>
        /// Método para agregar 3 unidades al producto seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            venta_producto ventaProd = (venta_producto)dgListaCompra.SelectedItem;

            if (dgListaCompra.SelectedItem != null)
            {
                producto prod = ventaProd.producto1;

                if (prod.cantidad >= 3)
                {
                    ventaProd.cantidad = ventaProd.cantidad + 3;
                    ventaProd.preciototal = ventaProd.preciototal + (prod.precio * 3);

                    prod.cantidad -= 3;

                    txtTotal.Text = "";
                    totalVenta();
                    dgListaCompra.Items.Refresh();
                }
            }
        }

        /// <summary>
        /// Método para agregar 4 unidades al producto seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            venta_producto ventaProd = (venta_producto)dgListaCompra.SelectedItem;

            if (dgListaCompra.SelectedItem != null)
            {
                producto prod = ventaProd.producto1;

                if (prod.cantidad >= 4)
                {
                    ventaProd.cantidad = ventaProd.cantidad + 4;
                    ventaProd.preciototal = ventaProd.preciototal + (prod.precio * 4);

                    prod.cantidad -= 4;

                    txtTotal.Text = "";
                    totalVenta();
                    dgListaCompra.Items.Refresh();
                }
            }
        }

        /// <summary>
        /// Método para agregar 5 unidades al producto seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            venta_producto ventaProd = (venta_producto)dgListaCompra.SelectedItem;

            if (dgListaCompra.SelectedItem != null)
            {
                producto prod = ventaProd.producto1;

                if (prod.cantidad >= 5)
                {
                    ventaProd.cantidad = ventaProd.cantidad + 5;
                    ventaProd.preciototal = ventaProd.preciototal + (prod.precio * 5);

                    prod.cantidad -= 5;

                    txtTotal.Text = "";
                    totalVenta();
                    dgListaCompra.Items.Refresh();
                }
            }
        }

        /// <summary>
        /// Método para agregar 6 unidades al producto seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            venta_producto ventaProd = (venta_producto)dgListaCompra.SelectedItem;

            if (dgListaCompra.SelectedItem != null)
            {
                producto prod = ventaProd.producto1;

                if (prod.cantidad >= 6)
                {
                    ventaProd.cantidad = ventaProd.cantidad + 6;
                    ventaProd.preciototal = ventaProd.preciototal + (prod.precio * 6);

                    prod.cantidad -= 6;

                    txtTotal.Text = "";
                    totalVenta();
                    dgListaCompra.Items.Refresh();
                }
            }
        }

        /// <summary>
        /// Método para agregar 7 unidades al producto seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            venta_producto ventaProd = (venta_producto)dgListaCompra.SelectedItem;

            if (dgListaCompra.SelectedItem != null)
            {
                producto prod = ventaProd.producto1;

                if (prod.cantidad >= 7)
                {
                    ventaProd.cantidad = ventaProd.cantidad + 7;
                    ventaProd.preciototal = ventaProd.preciototal + (prod.precio * 7);

                    prod.cantidad -= 7;

                    txtTotal.Text = "";
                    totalVenta();
                    dgListaCompra.Items.Refresh();
                }
            }
        }

        /// <summary>
        /// Método para agregar 8 unidades al producto seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            venta_producto ventaProd = (venta_producto)dgListaCompra.SelectedItem;

            if (dgListaCompra.SelectedItem != null)
            {
                producto prod = ventaProd.producto1;

                if (prod.cantidad >= 8)
                {
                    ventaProd.cantidad = ventaProd.cantidad + 8;
                    ventaProd.preciototal = ventaProd.preciototal + (prod.precio * 8);

                    prod.cantidad -= 8;

                    txtTotal.Text = "";
                    totalVenta();
                    dgListaCompra.Items.Refresh();
                }
            }
        }

        /// <summary>
        /// Método para agregar 9 unidades al producto seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            venta_producto ventaProd = (venta_producto)dgListaCompra.SelectedItem;

            if (dgListaCompra.SelectedItem != null)
            {
                producto prod = ventaProd.producto1;

                if (prod.cantidad >= 9)
                {
                    ventaProd.cantidad = ventaProd.cantidad + 9;
                    ventaProd.preciototal = ventaProd.preciototal + (prod.precio * 9);

                    prod.cantidad -= 9;

                    txtTotal.Text = "";
                    totalVenta();
                    dgListaCompra.Items.Refresh();
                }
            }
        }

        /// <summary>
        /// Método para agregar 10 unidades al producto seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn10_Click(object sender, RoutedEventArgs e)
        {
            venta_producto ventaProd = (venta_producto)dgListaCompra.SelectedItem;

            if (dgListaCompra.SelectedItem != null)
            {
                producto prod = ventaProd.producto1;

                if (prod.cantidad >= 10)
                {
                    ventaProd.cantidad = ventaProd.cantidad + 10;
                    ventaProd.preciototal = ventaProd.preciototal + (prod.precio * 10);

                    prod.cantidad -= 10;

                    txtTotal.Text = "";
                    totalVenta();
                    dgListaCompra.Items.Refresh();
                }
            }
        }

        /// <summary>
        /// Método para agregar 100 unidades al producto seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn100_Click(object sender, RoutedEventArgs e)
        {
            venta_producto ventaProd = (venta_producto)dgListaCompra.SelectedItem;

            if (dgListaCompra.SelectedItem != null)
            {
                producto prod = ventaProd.producto1;

                if (prod.cantidad >= 100)
                {
                    ventaProd.cantidad = ventaProd.cantidad + 100;
                    ventaProd.preciototal = ventaProd.preciototal + (prod.precio * 100);

                    prod.cantidad -= 100;

                    txtTotal.Text = "";
                    totalVenta();
                    dgListaCompra.Items.Refresh();
                }
            }
        }

        /// <summary>
        /// Método para agregar 1000 unidades al producto seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn1000_Click(object sender, RoutedEventArgs e)
        {
            venta_producto ventaProd = (venta_producto)dgListaCompra.SelectedItem;

            if (dgListaCompra.SelectedItem != null)
            {
                producto prod = ventaProd.producto1;

                if (prod.cantidad >= 1000)
                {
                    ventaProd.cantidad = ventaProd.cantidad + 1000;
                    ventaProd.preciototal = ventaProd.preciototal + (prod.precio * 1000);

                    prod.cantidad -= 1000;

                    txtTotal.Text = "";
                    totalVenta();
                    dgListaCompra.Items.Refresh();
                }
            }
        }
    }
}
