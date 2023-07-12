using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NLog;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TPV_IgnacioSanz.BackEnd.Modelo;
using TPV_IgnacioSanz.BackEnd.Servicios;

namespace TPV_IgnacioSanz.FrontEnd.Dialogos

{
    /// <summary>
    /// Lógica de interacción para AddProducto.xaml
    /// </summary>

    public partial class AddProducto : MetroWindow
    {
        private tpvEntities tpvEnt;

        private ServicioProducto servProd;
        private ServicioTipoProducto servTipo;

        private producto productoNuevo;
        private string imagen;

        private List<producto> listaProductos;

        private static Logger log = LogManager.GetCurrentClassLogger();

        public AddProducto(tpvEntities ent)
        {
            InitializeComponent();
            tpvEnt = ent;
            inicializa();
        }

        /// <summary>
        /// Inicializamos los objetos creados tanto en CS como XAML
        /// </summary>
        private void inicializa()
        {
            servProd = new ServicioProducto(tpvEnt);
            servTipo = new ServicioTipoProducto(tpvEnt);

            listaProductos = servProd.getAll().ToList();
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
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.ShowDialog();

            imagen = ofd.FileName;
            //Cogemos solo el nombre de la imagen en lugar de la ruta completa
            int posicionContrabarra = imagen.LastIndexOf("\\") + 1;
            int longitud = imagen.Length - posicionContrabarra;
            imagen = imagen.Substring(posicionContrabarra, longitud);
            txtImagen.Text = imagen;
        }

        /// <summary>
        /// Cancelamos la operación y no añadimos producto
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

                servProd.add(productoNuevo);

                try
                {
                    servProd.save();

                    await this.ShowMessageAsync("GESTIÓN PRODUCTOS",
                        "EL PRODUCTO SE HA GUARDADO CON ÉXITO");

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
        /// Asignamos al nuevo objeto producto los datos aportados por el usuario
        /// </summary>
        private void recogeDatos()
        {
            productoNuevo = new producto();

            if (listaProductos.Count() == 0) { productoNuevo.codigo = 1; }
            else { productoNuevo.codigo = servProd.getLastId() + 1; }

            productoNuevo.descripcion = txtDescripcion.Text;
            productoNuevo.precio = (double)txtPrecio.Value;
            if (comboUbicacion.SelectedItem != null) productoNuevo.ubicacion = comboUbicacion.SelectedItem.ToString();
            productoNuevo.cantidad = (int)txtCantidad.Value;
            productoNuevo.iva = (int)comboIVA.SelectedItem;
            productoNuevo.tipoproducto = (tipoproducto)comboTipo.SelectedItem;
            productoNuevo.imagen = "/Recursos/Imagenes/" + imagen;
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
