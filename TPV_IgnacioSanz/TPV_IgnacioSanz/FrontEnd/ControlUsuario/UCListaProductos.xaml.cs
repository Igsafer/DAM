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
using TPV_IgnacioSanz.FrontEnd.Dialogos;

namespace TPV_IgnacioSanz.FrontEnd.ControlUsuario
{
    /// <summary>
    /// Lógica de interacción para UCListaProductos.xaml
    /// </summary>

    public partial class UCListaProductos : UserControl
    {
        private tpvEntities tpvEnt;
        private usuario usuLogin;

        private const int EMPLEADO = 3;

        private ServicioProducto servProd;
        private ServicioTipoProducto servTipo;
        private ListCollectionView listaProductos;

        private List<Predicate<producto>> criterios;
        private Predicate<producto> criterioNombre;
        private Predicate<producto> criterioTipo;
        private Predicate<producto> criterioCantidad;
        private Predicate<object> predicadoFiltros;

        private static Logger log = LogManager.GetCurrentClassLogger();
        public UCListaProductos(tpvEntities ent, usuario usu)
        {
            InitializeComponent();
            tpvEnt = ent;
            usuLogin = usu;
            inicializa();
        }

        /// <summary>
        /// Inicializa los objetos creados tanto en el CS como en el XAML
        /// </summary>
        private void inicializa()
        {
            servProd = new ServicioProducto(tpvEnt);
            servTipo = new ServicioTipoProducto(tpvEnt);

            listaProductos = new ListCollectionView(servProd.getAll().ToList());
            dgListaProductos.ItemsSource = listaProductos;
            dgListaProductos.SelectedItem = null;
            comboTipo.ItemsSource = servTipo.getAll().ToList();

            criterios = new List<Predicate<producto>>();
            predicadoFiltros = new Predicate<object>(FiltroCombinadoCriterios);
            inicializaCriterios();
        }

        private void txtNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            incluyeCriterios();
            listaProductos.Filter = predicadoFiltros;
        }

        private void comboTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            incluyeCriterios();
            listaProductos.Filter = predicadoFiltros;
        }

        private void chkCantidad_Checked(object sender, RoutedEventArgs e)
        {
            incluyeCriterios();
            listaProductos.Filter = predicadoFiltros;
        }

        private void chkCantidad_Unchecked(object sender, RoutedEventArgs e)
        {
            incluyeCriterios();
            listaProductos.Filter = predicadoFiltros;
        }

        public bool FiltroCombinadoCriterios(object item)
        {
            bool correcto = true;
            producto prod = (producto)item;
            if (criterios.Count() != 0)
            {
                correcto = criterios.TrueForAll(x => x(prod));
            }

            return correcto;
        }

        /// <summary>
        /// Establece los criterios para los filtros
        /// </summary>
        private void inicializaCriterios()
        {
            criterioNombre = new Predicate<producto>(p => p.descripcion != null
                && p.descripcion.ToUpper().Contains(txtNombre.Text.ToUpper()));
            criterioTipo = new Predicate<producto>(t => t.tipoproducto!= null
                && t.tipoproducto.Equals(comboTipo.SelectedItem));
            criterioCantidad = new Predicate<producto>(p => p.cantidad <= 10);
        }

        /// <summary>
        /// Aplica los filtros que ha seleccionado el usuario
        /// </summary>
        private void incluyeCriterios()
        {
            criterios.Clear();
            if (txtNombre.Text != null)
            {
                criterios.Add(criterioNombre);
            }
            if (comboTipo.SelectedItem != null)
            {
                criterios.Add(criterioTipo);
            }
            if ((bool)chkCantidad.IsChecked)
            {
                criterios.Add(criterioCantidad);
            }
        }

        /// <summary>
        /// Elimina todos los filtros volviendo a mostrar el listado completo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuitaFiltro_Click(object sender, RoutedEventArgs e)
        {
            listaProductos.Filter = null;
            comboTipo.SelectedItem = null;
            chkCantidad.IsChecked = false;
        }

        /// <summary>
        /// Editar datos del producto seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            producto productoSeleccionado = (producto)dgListaProductos.SelectedItem;

            if (dgListaProductos.SelectedItem != null)
            {
                if(usuLogin.rol == EMPLEADO)
                {
                    MessageBox.Show("No tienes permiso para editar", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    EditProducto diag = new EditProducto(tpvEnt, productoSeleccionado);
                    diag.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    diag.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Ningún producto seleccionado", "GESTIÓN PRODUCTOS",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Elimina el producto seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (dgListaProductos.SelectedItem != null)
            {
                if(usuLogin.rol == EMPLEADO)
                {
                    MessageBox.Show("No tienes permiso para borrar", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    MessageBoxResult resultado = MessageBox.Show(
                        "¿Desea borrar este producto?", "GESTIÓN PRODUCTOS", MessageBoxButton.YesNo);
                    if (resultado == MessageBoxResult.Yes)
                    {
                        try
                        {
                            servProd.delete((producto)dgListaProductos.SelectedItem);
                            servProd.save();
                            dgListaProductos.Items.Refresh();
                            MessageBox.Show("Borrado con éxito", "GESTIÓN PRODUCTOS",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (DbUpdateException dbex)
                        {
                            MessageBox.Show("No se pudo borrar el producto", "GESTIÓN PRODUCTOS",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            log.Error(dbex.Message);
                            log.Error(dbex.InnerException);
                            log.Error(dbex.StackTrace);
                        }
                        catch (NullReferenceException nrex)
                        {
                            MessageBox.Show("El producto ya no existe", "GESTIÓN PRODUCTOS",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            log.Error(nrex.Message);
                            log.Error(nrex.InnerException);
                            log.Error(nrex.StackTrace);
                        }
                        catch (InvalidOperationException ioex)
                        {
                            MessageBox.Show("El producto ya no existe", "GESTIÓN PRODUCTOS",
                                                            MessageBoxButton.OK, MessageBoxImage.Information);
                            log.Error(ioex.Message);
                            log.Error(ioex.InnerException);
                            log.Error(ioex.StackTrace);
                        }

                        dgListaProductos.Items.Refresh();
                    }
                }
            }
            else
            {
                MessageBox.Show("Ningún producto seleccionado", "GESTIÓN PRODUCTOS",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Repone una cantidad dada por el usuario del producto seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReponer_Click(object sender, RoutedEventArgs e)
        {
            producto productoSeleccionado = (producto)dgListaProductos.SelectedItem;

            if (dgListaProductos.SelectedItem != null)
            {
                if(usuLogin.rol == EMPLEADO)
                {
                    MessageBox.Show("No tienes permiso para editar", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    ReponerProducto diag = new ReponerProducto(tpvEnt, productoSeleccionado);
                    diag.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    diag.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Ningún producto seleccionado", "GESTIÓN PRODUCTOS",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
