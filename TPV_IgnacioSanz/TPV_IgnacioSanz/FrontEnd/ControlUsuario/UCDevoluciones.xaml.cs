using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TPV_IgnacioSanz.BackEnd.Modelo;
using TPV_IgnacioSanz.BackEnd.Servicios;

namespace TPV_IgnacioSanz.FrontEnd.ControlUsuario
{
    /// <summary>
    /// Lógica de interacción para UCDevoluciones.xaml
    /// </summary>
    public partial class UCDevoluciones : UserControl
    {
        private tpvEntities tpvEnt;

        private ServicioVenta servVenta;
        private ServicioVentaProducto servVentaProd;
        private ServicioCliente servCli;

        private ListCollectionView listaVentas;
        private ListCollectionView listaVentaProds;

        private List<Predicate<venta>> criterios;
        private Predicate<venta> criterioFechaInicial;
        private Predicate<venta> criterioFechaFinal;
        private Predicate<venta> criterioCliente;
        private Predicate<object> predicadoFiltros;

        private static Logger log = LogManager.GetCurrentClassLogger();

        public UCDevoluciones(tpvEntities ent)
        {
            InitializeComponent();
            tpvEnt = ent;
            inicializa();
        }

        /// <summary>
        /// Inicializa los objetos creados tanto en el CS como en el XAML
        /// </summary>
        private void inicializa()
        {
            servVenta = new ServicioVenta(tpvEnt);
            servVentaProd = new ServicioVentaProducto(tpvEnt);
            servCli = new ServicioCliente(tpvEnt);

            listaVentas = new ListCollectionView(servVenta.getAll().ToList());
            listaVentaProds = new ListCollectionView(servVentaProd.getAll().ToList());
            dgListaVentas.ItemsSource = listaVentas;
            dgListaVentas.SelectedItem = null;
            comboCliente.ItemsSource = servCli.getAll().ToList();

            criterios = new List<Predicate<venta>>();
            predicadoFiltros = new Predicate<object>(FiltroCombinadoCriterios);
            inicializaCriterios();
        }

        private void txtFechaInicial_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            incluyeCriterios();
            listaVentas.Filter = predicadoFiltros;
        }

        private void txtFechaFinal_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            incluyeCriterios();
            listaVentas.Filter = predicadoFiltros;
        }

        private void comboCliente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            incluyeCriterios();
            listaVentas.Filter = predicadoFiltros;
        }

        public bool FiltroCombinadoCriterios(object item)
        {
            bool correcto = true;
            venta venta = (venta)item;
            if (criterios.Count() != 0)
            {
                correcto = criterios.TrueForAll(x => x(venta));
            }

            return correcto;
        }

        /// <summary>
        /// Establece los criterios para los filtros
        /// </summary>
        private void inicializaCriterios()
        {            
            criterioFechaInicial = new Predicate<venta>(v => v.fecha != null
                && v.fecha >= (DateTime)txtFechaInicial.SelectedDate);
            criterioFechaFinal = new Predicate<venta>(v => v.fecha != null
                && v.fecha <= (DateTime)txtFechaFinal.SelectedDate);
            criterioCliente = new Predicate<venta>(c => c.cliente1 != null
                && c.cliente1.Equals(comboCliente.SelectedItem));
        }

        /// <summary>
        /// Aplica los filtros que ha seleccionado el usuario
        /// </summary>
        private void incluyeCriterios()
        {
            criterios.Clear();            
            if(txtFechaInicial.SelectedDate != null)
            {
                criterios.Add(criterioFechaInicial);
            }
            if(txtFechaFinal.SelectedDate != null)
            {
                criterios.Add(criterioFechaFinal);
            }
            if(comboCliente.SelectedItem != null)
            {
                criterios.Add(criterioCliente);
            }
        }

        /// <summary>
        /// Elimina todos los filtros volviendo a mostrar el listado completo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuitaFiltro_Click(object sender, RoutedEventArgs e)
        {
            listaVentas.Filter = null;
            txtFechaInicial.SelectedDate = null;
            txtFechaFinal.SelectedDate = null;
            comboCliente.SelectedItem = null;
        }

        /// <summary>
        /// Elimina la venta seleccionada, devolviendo los productos al stock
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            venta ventaSeleccionada = (venta)dgListaVentas.SelectedItem;
            if(ventaSeleccionada != null)
            {
                MessageBoxResult resultado = MessageBox.Show(
                    "¿Desea devolver esta venta?", "GESTIÓN VENTAS", MessageBoxButton.YesNo);
                if(resultado == MessageBoxResult.Yes)
                {
                    try
                    {
                        foreach(venta_producto ventaProd in listaVentaProds)
                        {

                            if(ventaProd.venta == ventaSeleccionada.codigo)
                            {
                                producto prod = ventaProd.producto1;    
                                prod.cantidad += (int)ventaProd.cantidad;

                                servVentaProd.delete(ventaProd);
                            }
                        }

                        servVenta.delete(ventaSeleccionada);
                        servVenta.save();
                    
                    }
                    catch (DbUpdateException dbex)
                    {
                        MessageBox.Show("No se pudo borrar la venta", "GESTIÓN VENTAS",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        log.Error(dbex.Message);
                        log.Error(dbex.InnerException);
                        log.Error(dbex.StackTrace);
                    }
                    catch (NullReferenceException nrex)
                    {
                        MessageBox.Show("La venta ya no existe", "GESTIÓN VENTAS",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        log.Error(nrex.Message);
                        log.Error(nrex.InnerException);
                        log.Error(nrex.StackTrace);
                    }

                    dgListaVentas.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Ninguna venta seleccionada", "GESTIÓN VENTAS",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Esconde los detalles de la venta seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuitaDetalles_Click(object sender, RoutedEventArgs e)
        {
            dgListaVentas.SelectedItem = null;
        }                
    }
}
