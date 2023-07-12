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
    /// Lógica de interacción para UCListaUsuarios.xaml
    /// </summary>
    public partial class UCListaUsuarios : UserControl
    {
        private tpvEntities tpvEnt;
        private usuario usuLogin;

        private const int GERENTE = 1;

        private ServicioUsuario servUsu;
        private ServicioRol servRol;

        private ListCollectionView listaUsuarios;

        private List<Predicate<usuario>> criterios;
        private Predicate<usuario> criterioUsername;
        private Predicate<usuario> criterioNombreApellidos;
        private Predicate<usuario> criterioRol;
        private Predicate<object> predicadoFiltros;

        private static Logger log = LogManager.GetCurrentClassLogger();
        public UCListaUsuarios(tpvEntities ent, usuario usu)
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
            servUsu = new ServicioUsuario(tpvEnt);
            servRol = new ServicioRol(tpvEnt);

            listaUsuarios = new ListCollectionView(servUsu.getAll().ToList());
            dgListaUsuarios.ItemsSource = listaUsuarios;
            dgListaUsuarios.SelectedItem = null;
            comboRol.ItemsSource = servRol.getAll().ToList();

            criterios = new List<Predicate<usuario>>();
            predicadoFiltros = new Predicate<object>(FiltroCombinadoCriterios);
            inicializaCriterios();
        }

        private void txtBuscarPorUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            incluyeCriterios();
            listaUsuarios.Filter = predicadoFiltros;
        }

        private void txtBuscarPorNombreApellidos_TextChanged(object sender, TextChangedEventArgs e)
        {
            incluyeCriterios();
            listaUsuarios.Filter = predicadoFiltros;
        }

        private void comboRol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            incluyeCriterios();
            listaUsuarios.Filter = predicadoFiltros;
        }

        public bool FiltroCombinadoCriterios(object item)
        {
            bool correcto = true;
            usuario usu = (usuario)item;
            if (criterios.Count() != 0)
            {
                correcto = criterios.TrueForAll(x => x(usu));
            }

            return correcto;
        }

        /// <summary>
        /// Establece los criterios para los filtros
        /// </summary>
        private void inicializaCriterios()
        {
            criterioUsername = new Predicate<usuario>(u => u.login != null
                && u.login.ToUpper().Contains(txtBuscarPorUsername.Text.ToUpper()));
            criterioNombreApellidos = new Predicate<usuario>(u => (u.nombre != null || u.apellidos != null)
                && (u.nombre.ToUpper().Contains(txtBuscarPorNombreApellidos.Text.ToUpper())
                ||  u.apellidos.ToUpper().Contains(txtBuscarPorNombreApellidos.Text.ToUpper())));
            criterioRol = new Predicate<usuario>(u => u.rol1!= null
                && u.rol1.Equals(comboRol.SelectedItem));
        }

        /// <summary>
        /// Aplica los filtros que ha seleccionado el usuario
        /// </summary>
        private void incluyeCriterios()
        {
            criterios.Clear();
            if (txtBuscarPorUsername.Text != null)
            {
                criterios.Add(criterioUsername);
            }
            if(txtBuscarPorNombreApellidos.Text != null)
            {
                criterios.Add(criterioNombreApellidos);
            }
            if (comboRol.SelectedItem != null)
            {
                criterios.Add(criterioRol);
            }
        }

        /// <summary>
        /// Elimina todos los filtros volviendo a mostrar el listado completo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuitaFiltro_Click(object sender, RoutedEventArgs e)
        {
            listaUsuarios.Filter = null;
            txtBuscarPorUsername.Text = null;
            txtBuscarPorNombreApellidos.Text = null;
            comboRol.SelectedItem = null;
        }

        /// <summary>
        /// Editar contraseña del usuario seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            usuario usuarioSeleccionado = (usuario)dgListaUsuarios.SelectedItem;

            if(dgListaUsuarios.SelectedItem != null){
                if(usuLogin.rol == GERENTE || usuLogin.codigo == usuarioSeleccionado.codigo)
                {
                    EditPassword diag = new EditPassword(tpvEnt, usuarioSeleccionado);
                    diag.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    diag.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Sólo puedes modificar tu contraseña", "GESTIÓN USUARIOS",
                                                MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
            {
                MessageBox.Show("Ningún usuario seleccionado", "GESTIÓN USUARIOS",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Eliminar el usuario seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            usuario usuSeleccionado = (usuario)dgListaUsuarios.SelectedItem;

            if (usuSeleccionado != null)
            {
                if(usuLogin.rol == GERENTE)
                {
                    if (usuSeleccionado.codigo == usuLogin.codigo)
                    {
                        MessageBox.Show("No puedes borrarte a ti mismo", "GESTIÓN USUARIOS",
                               MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBoxResult resultado = MessageBox.Show(
                            "¿Desea borrar este usuario?", "GESTIÓN USUARIOS", MessageBoxButton.YesNo);
                        if (resultado == MessageBoxResult.Yes)
                        {
                            try
                            {
                                servUsu.delete(usuSeleccionado);
                                servUsu.save();
                                MessageBox.Show("Borrado con éxito", "GESTIÓN USUARIOS",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                                dgListaUsuarios.Items.Refresh();
                            }
                            catch (DbUpdateException dbex)
                            {
                                MessageBox.Show("No se pudo borrar al usuario", "GESTIÓN USUARIOS",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                                log.Error(dbex.Message);
                                log.Error(dbex.InnerException);
                                log.Error(dbex.StackTrace);
                            }
                            catch (NullReferenceException nrex)
                            {
                                MessageBox.Show("El usuario ya no existe", "GESTIÓN USUARIOS",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                                log.Error(nrex.Message);
                                log.Error(nrex.InnerException);
                                log.Error(nrex.StackTrace);
                            }
                            catch (InvalidOperationException ioex)
                            {
                                MessageBox.Show("El usuario ya no existe", "GESTIÓN USUARIOS",
                                                                MessageBoxButton.OK, MessageBoxImage.Information);
                                log.Error(ioex.Message);
                                log.Error(ioex.InnerException);
                                log.Error(ioex.StackTrace);
                            }

                            dgListaUsuarios.Items.Refresh();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No tienes permiso para borrar", "GESTIÓN USUARIOS",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Ningún usuario seleccionado", "GESTIÓN USUARIOS",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
