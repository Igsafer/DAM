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
    
    public partial class producto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public producto()
        {
            this.venta_producto = new HashSet<venta_producto>();
        }
    
        public int codigo { get; set; }
        public string descripcion { get; set; }
        public double precio { get; set; }
        public string ubicacion { get; set; }
        public int cantidad { get; set; }
        public int tipo { get; set; }
        public Nullable<int> oferta { get; set; }
        public string imagen { get; set; }
        public int iva { get; set; }
    
        public virtual oferta oferta1 { get; set; }
        public virtual tipoproducto tipoproducto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<venta_producto> venta_producto { get; set; }
    }
}
