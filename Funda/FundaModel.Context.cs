﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Funda
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class WebAnalyzerEntities : DbContext
    {
        public WebAnalyzerEntities()
            : base("name=WebAnalyzerEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Rent> Rent { get; set; }
        public virtual DbSet<Sale> Sale { get; set; }
        public virtual DbSet<SpatialAnalysis> SpatialAnalysis { get; set; }
    }
}
