//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TPV_IgnacioSanz.BackEnd.Modelo
{
    using System;
    using System.Collections.Generic;
    
    public partial class venta_producto
    {
        public int venta { get; set; }
        public int producto { get; set; }
        public Nullable<int> cantidad { get; set; }
        public Nullable<double> preciototal { get; set; }
    
        public virtual producto producto1 { get; set; }
        public virtual venta venta1 { get; set; }
    }
}