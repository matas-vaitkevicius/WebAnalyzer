//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class SpatialAnalysis
    {
        public int Id { get; set; }
        public System.Data.Entity.Spatial.DbGeography Point { get; set; }
        public Nullable<int> RentsIn1kRadiusCount { get; set; }
        public Nullable<int> SalesIn1kRadiusCount { get; set; }
        public Nullable<decimal> RentsIn1kRadiusAvgSqM { get; set; }
        public Nullable<decimal> SalesIn1kRadiusAvgSqM { get; set; }
        public Nullable<int> RentsIn500RadiusCount { get; set; }
        public Nullable<int> SalesIn500RadiusCount { get; set; }
        public Nullable<decimal> RentsIn500RadiusAvgSqM { get; set; }
        public Nullable<decimal> SalesIn500RadiusAvgSqM { get; set; }
        public Nullable<int> RentsIn200RadiusCount { get; set; }
        public Nullable<int> SalesIn200RadiusCount { get; set; }
        public Nullable<decimal> RentsIn200RadiusAvgSqM { get; set; }
        public Nullable<decimal> SalesIn200RadiusAvgSqM { get; set; }
        public Nullable<int> RentsIn100RadiusCount { get; set; }
        public Nullable<int> SalesIn100RadiusCount { get; set; }
        public Nullable<decimal> RentsIn100RadiusAvgSqM { get; set; }
        public Nullable<decimal> SalesIn100RadiusAvgSqM { get; set; }
    
        public virtual Rent Rent { get; set; }
        public virtual Sale Sale { get; set; }
    }
}