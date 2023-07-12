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
    /// Lógica de interacción para UCListaClientes.xaml
    /// </summary>


    public partial class UCListaClientes : UserControl
    {
        private tpvEntities tpvEnt;
        private usuario usuLogin;

        private const int EMPLEADO = 3;

        private ServicioCliente servCli;
        private ListCollectionView listaClientes;

        private List<Predicate<cliente>> criterios;
        private Predicate<cliente> criterioNombre;
        private Predicate<cliente> criterioApellido;
        private Predicate<object> predicadoFiltros;

        private static Logger log = LogManager.GetCurrentClassLogger();
        public UCListaClientes(tpvEntities ent, usuario usu)
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
            servCli = new ServicioCliente(tpvEnt);

            listaClientes = new ListCollectionView(servCli.getAll().ToList());
            dgListaClientes.ItemsSource = listaClientes;
            dgListaClientes.SelectedItem = null;

            criterios = new List<Predicate<cliente>>();
            predicadoFiltros = new Predicate<object>(FiltroCombinadoCriterios);
            inicializaCriterios();
        }

        private void txtBuscarPorNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            incluyeCriterios();
            listaClientes.Filter = predicadoFiltros;
        }

        private void txtBuscarPorApellido_TextChanged(object sender, TextChangedEventArgs e)
        {
            incluyeCriterios();
            listaClientes.Filter = predicadoFiltros;
        }

        public bool FiltroCombinadoCriterios(object item)
        {
            bool correcto = true;
            cliente cli = (cliente)item;
            if (criterios.Count() != 0)
            {
                correcto = criterios.TrueForAll(x => x(cli));
            }

            return correcto;
        }

        /// <summary>
        /// Establece los criterios para los filtros
        /// </summary>
        private void inicializaCriterios()
        {
            criterioNombre = new Predicate<cliente>(c => c.nombre!= null
                && c.nombre.ToUpper().Contains(txtBuscarPorNombre.Text.ToUpper()));
            criterioApellido = new Predicate<cliente>(c => c.apellidos!= null
                && c.apellidos.ToUpper().Contains(txtBuscarPorApellido.Text.ToUpper()));
        }

        /// <summary>
        /// Aplica los filtros que ha seleccionado el usuario
        /// </summary>
        private void incluyeCriterios()
        {
            criterios.Clear();
            if (txtBuscarPorNombre.Text != null)
            {
                criterios.Add(criterioNombre);
            }
            if (txtBuscarPorApellido.Text != null)
            {
                criterios.Add(criterioApellido);
            }
        }

        /// <summary>
        /// Elimina todos los filtros volviendo a mostrar el listado completo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuitaFiltro_Click(object sender, RoutedEventArgs e)
        {
            listaClientes.Filter = null;
            txtBuscarPorNombre.Text = null;
            txtBuscarPorApellido.Text = null;
        }

        /// <summary>
        /// Editar datos del cliente seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            cliente clienteSeleccionado = (cliente)dgListaClientes.SelectedItem;

            if(dgListaClientes.SelectedItem != null)
            {
                if(usuLogin.rol == EMPLEADO)
                {
                    MessageBox.Show("No tienes permiso para editar", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    EditCliente diag = new EditCliente(tpvEnt, clienteSeleccionado);
                    diag.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    diag.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Ningún cliente seleccionado", "GESTIÓN CLIENTES",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        /// <summary>
        /// Elimina el cliente seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (dgListaClientes.SelectedItem != null)
            {
                if(usuLogin.rol == EMPLEADO)
                {
                    MessageBox.Show("No tienes permiso para borrar", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    MessageBoxResult resultado = MessageBox.Show(
                        "¿Desea borrar este cliente?", "GESTIÓN CLIENTES", MessageBoxButton.YesNo);
                    if (resultado == MessageBoxResult.Yes)
                    {
                        try
                        {
                            servCli.delete((cliente)dgListaClientes.SelectedItem);
                            servCli.save();
                        
                            MessageBox.Show("Borrado con éxito", "GESTIÓN CLIENTES",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (DbUpdateException dbex)
                        {
                            MessageBox.Show("No se pudo borrar al cliente", "GESTIÓN CLIENTES",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            log.Error(dbex.Message);
                            log.Error(dbex.InnerException);
                            log.Error(dbex.StackTrace);
                        }
                        catch (NullReferenceException nrex)
                        {
                            MessageBox.Show("El cliente ya no existe", "GESTIÓN CLIENTES",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            log.Error(nrex.Message);
                            log.Error(nrex.InnerException);
                            log.Error(nrex.StackTrace);
                        }
                        catch (InvalidOperationException ioex)
                        {
                            MessageBox.Show("El cliente ya no existe", "GESTIÓN CLIENTES",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            log.Error(ioex.Message);
                            log.Error(ioex.InnerException);
                            log.Error(ioex.StackTrace);
                        }

                        dgListaClientes.Items.Refresh();
                    }
                }
            }
            else
            {
                MessageBox.Show("Ningún cliente seleccionado", "GESTIÓN CLIENTES",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
