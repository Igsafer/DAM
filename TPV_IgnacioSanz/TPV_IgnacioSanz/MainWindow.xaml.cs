using MahApps.Metro.Controls;
using NLog;
using System.Windows;
using TPV_IgnacioSanz.BackEnd.Modelo;
using TPV_IgnacioSanz.FrontEnd.ControlUsuario;
using TPV_IgnacioSanz.FrontEnd.Dialogos;

namespace TPV_IgnacioSanz
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>

    public partial class MainWindow : MetroWindow
    {
        private tpvEntities tpvEnt;
        private usuario usuLogin;

        private const int GERENTE = 1;
        private const int EMPLEADO = 3;

        private static Logger log = LogManager.GetCurrentClassLogger();

        public MainWindow(tpvEntities ent, usuario usu)
        {
            InitializeComponent();
            tpvEnt = ent;
            usuLogin = usu;
            inicializa();
        }

        /// <summary>
        /// Queremos que el panel de selección de productos esté nada más iniciar la MainWindow
        /// </summary>
        private void inicializa()
        {
            UCProductos uc = new UCProductos(tpvEnt, usuLogin);
            gridCentral.Children.Add(uc);
        }

        /// <summary>
        /// Volver al panel de selección de productos (borrará la lista de la compra)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInicio_Click(object sender, RoutedEventArgs e)
        {
            UCProductos uc = new UCProductos(tpvEnt, usuLogin);
            gridCentral.Children.Clear();
            gridCentral.Children.Add(uc);
        }

        /// <summary>
        /// Cerrar sesión del usuario. Vuelve a la ventana de Login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            login.Show();
            Close();
        }

        /// <summary>
        /// Accede al panel de selección de productos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVentas_Click(object sender, RoutedEventArgs e)
        {        
            UCProductos uc = new UCProductos(tpvEnt, usuLogin);
            gridCentral.Children.Clear();
            gridCentral.Children.Add(uc);
        }

        /// <summary>
        /// Accede a la lista de ventas, así como operaciones sobre las mismas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDevoluciones_Click(object sender, RoutedEventArgs e)
        {   
            if(usuLogin.rol == EMPLEADO)
            {
                MessageBox.Show("No tienes permiso para acceder", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                UCDevoluciones uc = new UCDevoluciones(tpvEnt);
                gridCentral.Children.Clear();
                gridCentral.Children.Add(uc);
            }
        }

        /// <summary>
        /// Accede al menú para añadir un cliente nuevo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCliente_Click(object sender, RoutedEventArgs e)
        {
            AddCliente diag = new AddCliente(tpvEnt);
            diag.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            diag.ShowDialog();
        }

        /// <summary>
        /// Accede al listado de clientes, así como operacione sobre los mismos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnListaClientes_Click(object sender, RoutedEventArgs e)
        {
            UCListaClientes uc = new UCListaClientes(tpvEnt, usuLogin);
            gridCentral.Children.Clear();
            gridCentral.Children.Add(uc);
        }

        /// <summary>
        /// Accede al menú para añadir un producto nuevo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddProducto_Click(object sender, RoutedEventArgs e)
        {
            AddProducto diag = new AddProducto(tpvEnt);
            diag.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            diag.ShowDialog();
        }

        /// <summary>
        /// Accede al listado de productos, así como operaciones sobre los mismos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnListaProductos_Click(object sender, RoutedEventArgs e)
        {
            UCListaProductos uc = new UCListaProductos(tpvEnt, usuLogin);
            gridCentral.Children.Clear();
            gridCentral.Children.Add(uc);
        }

        /// <summary>
        /// Accede al menú para añadir a un usuario nuevo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddUsuario_Click(object sender, RoutedEventArgs e)
        {
            if(usuLogin.rol == GERENTE)
            {
                AddUsuario diag = new AddUsuario(tpvEnt);
                diag.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                diag.ShowDialog();
            }
            else
            {
                MessageBox.Show("No tienes permiso para acceder", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Accede al listado de usuarios, así como operaciones sobre los mismos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnListaUsuarios_Click(object sender, RoutedEventArgs e)
        {        
            UCListaUsuarios uc = new UCListaUsuarios(tpvEnt, usuLogin);
            gridCentral.Children.Clear();
            gridCentral.Children.Add(uc);
        }
    }
}
