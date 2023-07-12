using MahApps.Metro.Controls;
using NLog;
using System;
using System.Windows;
using TPV_IgnacioSanz.BackEnd.Modelo;
using TPV_IgnacioSanz.BackEnd.Servicios;

namespace TPV_IgnacioSanz.FrontEnd.Dialogos
{
    /// <summary>
    /// Lógica de interacción para ReponerProducto.xaml
    /// </summary>
    public partial class ReponerProducto : MetroWindow
    {
        private tpvEntities tpvEnt;
        private ServicioProducto servProd;
        private producto productoSeleccionado;
        private static Logger log = LogManager.GetCurrentClassLogger();
        public ReponerProducto(tpvEntities ent, producto prodSel)
        {
            InitializeComponent();
            tpvEnt = ent;
            productoSeleccionado = prodSel;
            inicializa();
        }

        /// <summary>
        /// Inicializa los objetos creados tanto en XAML como en CS
        /// </summary>
        private void inicializa()
        {
            servProd = new ServicioProducto(tpvEnt);

            txtCantidad.Text = productoSeleccionado.cantidad.ToString();
        }

        /// <summary>
        /// Cancelamos la operación y no reponemos producto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        /// <summary>
        /// Añadimos la cantidad escrita al producto seleccinado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {                
                int cantidadAReponer = Convert.ToInt32(txtReponer.Text);

                if(cantidadAReponer > 0)
                {
                    productoSeleccionado.cantidad += cantidadAReponer;
                    servProd.edit(productoSeleccionado);
                    servProd.save();

                    MessageBox.Show("Repuesto con éxito", "GESTIÓN PRODUCTOS", MessageBoxButton.OK);

                    DialogResult = true;
                    }
                else
                {
                    MessageBox.Show("Escribe un número positivo", "GESTIÓN PRODUCTOS", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (FormatException fex)
            {
                MessageBox.Show("Debes escribir un número entero", "GESTIÓN PRODUCTOS", MessageBoxButton.OK, MessageBoxImage.Warning);
                log.Error(fex.Message);
                log.Error(fex.InnerException);
                log.Error(fex.StackTrace);
            }
        }
    }
}
