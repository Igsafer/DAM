using MahApps.Metro.Controls;
using NLog;
using System;
using System.Windows;
using TPV_IgnacioSanz.BackEnd.Modelo;
using TPV_IgnacioSanz.BackEnd.Servicios;

namespace TPV_IgnacioSanz.FrontEnd.Dialogos
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : MetroWindow
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private tpvEntities tpvEnt;
        private ServicioUsuario servUsu;

        public Login()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (!conectar())
            {
                MessageBox.Show("¡ERROR! NO SE PUEDE CONECTAR CON LA BASE DE DATOS",
                "CONEXION BASE DE DATOS", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
            servUsu = new ServicioUsuario(tpvEnt);
        }

        /// <summary>
        /// Método para comprobar las credenciales del usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsuario.Text) || string.IsNullOrEmpty(txtPassword.Password))
            {
                MessageBox.Show("ES NECESARIO ESTABLECER EL USUARIO Y LA CONTRASEÑA", "LOGIN", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (servUsu.login(txtUsuario.Text, txtPassword.Password))
            {
                MainWindow ventanaPrincipal = new MainWindow(tpvEnt, servUsu.usuLogin);
                ventanaPrincipal.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                ventanaPrincipal.Show();
                Close();
            }
            else
            {
                MessageBox.Show("USUARIO Y CONTRASEÑA INCORRECTOS", "LOGIN", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Método donde se crea el contexto de la BD y se comprueba que conecta
        /// </summary>
        /// <returns></returns>
        private bool conectar()
        {
            bool conectar = true;
            tpvEnt = new tpvEntities();
            try
            {
                tpvEnt.Database.Connection.Open();
            }
            catch (Exception ex)
            {
                conectar = false;
                log.Info("No se puedo conectar con la BD");
                log.Error(ex.InnerException);
                log.Error(ex.StackTrace);
            }
            return conectar;
        }
    }
}
