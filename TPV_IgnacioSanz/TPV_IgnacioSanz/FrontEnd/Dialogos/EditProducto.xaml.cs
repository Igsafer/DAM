using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NLog;
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
    /// Lógica de interacción para EditProducto.xaml
    /// </summary>
    public partial class EditProducto : MetroWindow
    {
        private tpvEntities tpvEnt;

        private ServicioProducto servProd;
        private ServicioTipoProducto servTipo;

        private producto productoSeleccionado;

        private string imagen;

        private static Logger log = LogManager.GetCurrentClassLogger();
        public EditProducto(tpvEntities ent, producto prodSel)
        {
            InitializeComponent();
            tpvEnt = ent;
            productoSeleccionado = prodSel;
            inicializa();
        }

        /// <summary>
        /// Inicializamos los objetos creados tanto en CS como XAML
        /// </summary>
        private void inicializa()
        {
            servProd = new ServicioProducto(tpvEnt);
            servTipo = new ServicioTipoProducto(tpvEnt);

            txtDescripcion.Text = productoSeleccionado.descripcion;
            txtPrecio.Value = productoSeleccionado.precio;
            comboUbicacion.SelectedItem = productoSeleccionado.ubicacion;
            comboIVA.SelectedItem = productoSeleccionado.iva;
            comboTipo.SelectedItem = productoSeleccionado.tipoproducto;

            comboTipo.ItemsSource = servTipo.getAll().ToList();
            comboUbicacion.ItemsSource = servProd.getUbicaciones();
            comboIVA.ItemsSource = servProd.getIVAs();
        }

        /// <summary>
        /// Botón para seleccionar una imagen de archivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImagen_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.ShowDialog();

            imagen = dlg.FileName;
            //Cogemos solo el nombre de la imagen en lugar de la ruta completa
            int posicionContrabarra = imagen.LastIndexOf("\\") + 1;
            int longitud = imagen.Length - posicionContrabarra;
            imagen = imagen.Substring(posicionContrabarra, longitud);
            txtImagen.Text = imagen;
        }

        /// <summary>
        /// Cancelamos la operación y no editamos producto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        /// <summary>
        /// Guardamos el producto si todo ha sido introducido correctamente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (valida())
            {
                txtErrorCampos.Text = "";

                recogeDatos();

                servProd.edit(productoSeleccionado);

                try
                {
                    servProd.save();

                    await this.ShowMessageAsync("GESTIÓN PRODUCTOS",
                        "EL PRODUCTO SE HA GUARDADO CON ÉXITO \nRECARGA LA PÁGINA PARA VER LOS CAMBIOS");

                    DialogResult = true;
                }
                catch (DbUpdateException dbex)
                {
                    await this.ShowMessageAsync("GESTIÓN PRODUCTOS",
                        "NO SE HA PODIDO GUARDAR EL PRODUCTO");

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
        /// Asignamos al objeto producto los nuevos datos aportados por el usuario
        /// </summary>
        private void recogeDatos()
        {
            productoSeleccionado.descripcion = txtDescripcion.Text;
            productoSeleccionado.precio = (double)txtPrecio.Value;
            if (comboUbicacion.SelectedItem != null) productoSeleccionado.ubicacion = comboUbicacion.SelectedItem.ToString();
            productoSeleccionado.iva = (int)comboIVA.SelectedItem;
            productoSeleccionado.tipoproducto = (tipoproducto)comboTipo.SelectedItem;
            productoSeleccionado.imagen = "/Recursos/Imagenes/" + imagen;
        }

        /// <summary>
        /// Comprobamos que los datos de los campos están en un formato correcto
        /// y que los campos obligatorios están rellenados
        /// </summary>
        /// <returns></returns>
        private bool valida()
        {
            bool correcto = true;

            if (string.IsNullOrEmpty(txtDescripcion.Text))
            {
                muestraError(imgErrorDescripcion, txtDescripcion);
                correcto = false;
            }
            else
            {
                quitaError(imgErrorDescripcion, txtDescripcion);
            }

            if (comboTipo.SelectedItem == null)
            {
                muestraError(imgErrorTipo, comboTipo);
                correcto = false;
            }
            else
            {
                quitaError(imgErrorTipo, comboTipo);
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
