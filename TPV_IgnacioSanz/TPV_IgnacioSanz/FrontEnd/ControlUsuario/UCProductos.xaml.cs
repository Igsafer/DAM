using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TPV_IgnacioSanz.BackEnd.Modelo;
using TPV_IgnacioSanz.BackEnd.Servicios;
using TPV_IgnacioSanz.FrontEnd.ClasesAuxiliares;

namespace TPV_IgnacioSanz.FrontEnd.ControlUsuario
{
    /// <summary>
    /// Lógica de interacción para UCProductos.xaml
    /// </summary>

    public partial class UCProductos : UserControl
    {
        private tpvEntities tpvEnt;
        private usuario usuLogin;

        private ServicioProducto servProd;
        private ServicioTipoProducto servTipo;

        private List<tipoproducto> listaTipos;
        private List<producto> listaProductos;
        private List<venta_producto> listaCompra;

        private producto prodSeleccionado;
        public UCProductos(tpvEntities ent, usuario usu)
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
            servTipo = new ServicioTipoProducto(tpvEnt);
            servProd = new ServicioProducto(tpvEnt);

            listaTipos = servTipo.getAll().ToList();
            listaProductos = servProd.getAll().ToList();
            listaCompra = new List<venta_producto>();

            prodSeleccionado = new producto();

            WrapPanel wrapTipos = panelTipoProductos();
            panelTipoProducto.Children.Add(wrapTipos);

            WrapPanel wrapProductos = panelProductos();
            panelProducto.Children.Add(wrapProductos);
        }

        /// <summary>
        /// Carga la lista de los botones de tipo de producto
        /// </summary>
        /// <returns>WrapPanel con los tipos de productos</returns>
        private WrapPanel panelTipoProductos()
        {
            WrapPanel wrapTipos = new WrapPanel();

            foreach (tipoproducto tipo in listaTipos)
            {
                StackPanel stackTipos = new StackPanel();
                stackTipos.Orientation = Orientation.Vertical;

                BotonTipo btnTipo = new BotonTipo
                {
                    Width = 100,
                    Height = 100,
                    Margin = new Thickness(10),
                    Background = Brushes.White,
                    Content = new Image
                    {
                        Source = new BitmapImage(new Uri(tipo.imagen, UriKind.RelativeOrAbsolute)),
                    },
                    tipoproductoTPV = tipo
                };

                btnTipo.Click += new RoutedEventHandler(BotonTipo_Click);

                TextBlock tbTipo = new TextBlock()
                {
                    Text = tipo.nombre,
                    FontSize = 13,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                stackTipos.Children.Add(btnTipo);
                stackTipos.Children.Add(tbTipo);

                wrapTipos.Children.Add(stackTipos);
            }

            //Botón para cargar todos los productos
            StackPanel stackBotonTodos = new StackPanel();
            stackBotonTodos.Orientation = Orientation.Vertical;

            BotonTipo botonTodos = new BotonTipo
            {
                Width = 100,
                Height = 100,
                Margin = new Thickness(10),
                Background = Brushes.White,
                Content = "TODOS",
                FontWeight = FontWeights.Bold
            };

            botonTodos.Click += new RoutedEventHandler(BotonTodos_Click);

            TextBlock tbTodos = new TextBlock()
            {
                Text = "TODOS LOS PRODUCTOS",
                FontSize = 10,
                Margin = new Thickness(0,3,0,0),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            stackBotonTodos.Children.Add(botonTodos);
            stackBotonTodos.Children.Add(tbTodos);

            wrapTipos.Children.Add(stackBotonTodos);
            
            return wrapTipos;
        }

        /// <summary>
        /// Carga la lista de los botones de producto
        /// </summary>
        /// <returns>WrapPanel con los productos</returns>
        private WrapPanel panelProductos()
        {
            WrapPanel wrapProductos = new WrapPanel();

            foreach (producto producto in listaProductos)
            {
                StackPanel stackProductos = new StackPanel();
                stackProductos.Orientation = Orientation.Vertical;

                BotonProducto btnProducto = new BotonProducto
                {
                    Width = 100,
                    Height = 100,
                    Margin = new Thickness(10),
                    Background = Brushes.White,
                    Content = new Image
                    {
                        Source = new BitmapImage(new Uri(producto.imagen, UriKind.RelativeOrAbsolute)),
                    },
                    productoTPV = producto
                };

                TextBlock tbProducto = new TextBlock
                {
                    Text = producto.descripcion,
                    FontSize = 13,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                stackProductos.Children.Add(btnProducto);
                stackProductos.Children.Add(tbProducto);

                btnProducto.Click += new RoutedEventHandler(BotonProducto_Click);

                wrapProductos.Children.Add(stackProductos);
            }
            return wrapProductos;
        }

        /// <summary>
        /// Filtra los productos por tipo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BotonTipo_Click(object sender, RoutedEventArgs e)
        {
            panelProducto.Children.Clear();

            listaProductos = servProd.listaProductosPorTipo(((BotonTipo)sender).tipoproductoTPV.codigo);

            foreach (producto producto in listaProductos)
            {
                StackPanel stackProductos = new StackPanel();
                stackProductos.Orientation = Orientation.Vertical;

                BotonProducto btnProducto = new BotonProducto
                {
                    Width = 100,
                    Height = 100,
                    Margin = new Thickness(10),
                    Background = Brushes.White,
                    Content = new Image
                    {
                        Source = new BitmapImage(new Uri(producto.imagen, UriKind.RelativeOrAbsolute)),
                    },
                    productoTPV = producto
                };

                TextBlock tbProducto = new TextBlock
                {
                    Text = producto.descripcion,
                    FontSize = 13,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                stackProductos.Children.Add(btnProducto);
                stackProductos.Children.Add(tbProducto);

                btnProducto.Click += new RoutedEventHandler(BotonProducto_Click);

                panelProducto.Children.Add(stackProductos);
            }
        }

        /// <summary>
        /// Vuelve a mostrar todos los productos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BotonTodos_Click(object sender, RoutedEventArgs e)
        {
            panelProducto.Children.Clear();

            listaProductos = servProd.getAll().ToList();

            foreach (producto producto in listaProductos)
            {
                StackPanel stackProductos = new StackPanel();
                stackProductos.Orientation = Orientation.Vertical;

                BotonProducto btnProducto = new BotonProducto
                {
                    Width = 100,
                    Height = 100,
                    Margin = new Thickness(10),
                    Background = Brushes.White,
                    Content = new Image
                    {
                        Source = new BitmapImage(new Uri(producto.imagen, UriKind.RelativeOrAbsolute)),
                    },
                    productoTPV = producto
                };

                TextBlock tbProducto = new TextBlock
                {
                    Text = producto.descripcion,
                    FontSize = 13,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                stackProductos.Children.Add(btnProducto);
                stackProductos.Children.Add(tbProducto);

                btnProducto.Click += new RoutedEventHandler(BotonProducto_Click);

                panelProducto.Children.Add(stackProductos);
            }
        }

        /// <summary>
        /// Añade el producto seleccionado a la lista de la compra
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BotonProducto_Click(object sender, RoutedEventArgs e)
        {
            prodSeleccionado = ((BotonProducto)sender).productoTPV;

            gridListaCompra.Children.Clear();

            //Solo podremos seleccionar un producto si queda stock del mismo
            if(prodSeleccionado.cantidad > 0)
            {
                //Si es el primer producto que clickamos creamos una venta_producto nueva
                if(listaCompra.Count() == 0)
                {
                    venta_producto ventaProd = new venta_producto();

                    ventaProd.producto = prodSeleccionado.codigo;
                    ventaProd.cantidad = 1;
                    ventaProd.preciototal = prodSeleccionado.precio;
                    ventaProd.producto1 = prodSeleccionado;                            

                    listaCompra.Add(ventaProd);

                    prodSeleccionado.cantidad--;

                    UCListaCompra uc = new UCListaCompra(tpvEnt, listaCompra, usuLogin);
                    gridListaCompra.Children.Add(uc);
                }
                else
                {
                    bool enc = false; //enc = encontrado

                    for(int i = 0; i < listaCompra.Count() && !enc; i++)
                    {
                        //Buscamos si el producto ya está en la lista de la compra
                        //Si ya lo está se añade a una venta_producto ya creada
                        if(listaCompra[i].producto1.codigo == prodSeleccionado.codigo)
                        {
                            enc = true;
                            listaCompra[i].cantidad++;
                            listaCompra[i].preciototal += listaCompra[i].producto1.precio;

                            prodSeleccionado.cantidad--;

                            UCListaCompra uc = new UCListaCompra(tpvEnt, listaCompra, usuLogin);
                            gridListaCompra.Children.Add(uc);
                        }
                    }

                    //Si no está en la lista, creamos una venta_producto nueva
                    if (!enc)
                    {
                        venta_producto ventaProd = new venta_producto();

                        ventaProd.producto = prodSeleccionado.codigo;
                        ventaProd.cantidad = 1;
                        ventaProd.preciototal = prodSeleccionado.precio;
                        ventaProd.producto1 = prodSeleccionado;

                        listaCompra.Add(ventaProd);

                        prodSeleccionado.cantidad--;

                        UCListaCompra uc = new UCListaCompra(tpvEnt,listaCompra, usuLogin);
                        gridListaCompra.Children.Add(uc);
                    }            
                }
            }
        }
    }
}
