﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class tpvEntities : DbContext
    {
        public tpvEntities()
            : base("name=tpvEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<cliente> cliente { get; set; }
        public virtual DbSet<oferta> oferta { get; set; }
        public virtual DbSet<permiso> permiso { get; set; }
        public virtual DbSet<producto> producto { get; set; }
        public virtual DbSet<rol> rol { get; set; }
        public virtual DbSet<tipoproducto> tipoproducto { get; set; }
        public virtual DbSet<usuario> usuario { get; set; }
        public virtual DbSet<venta> venta { get; set; }
        public virtual DbSet<venta_producto> venta_producto { get; set; }
    }
}
