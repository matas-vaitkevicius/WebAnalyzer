using System;

namespace Funda
{
    public interface IRecord
    {
         int Id { get; set; }
         string Url { get; set; }
         string Title { get; set; }
         string Address { get; set; }
         string Subtitle { get; set; }
         Nullable<decimal> Price { get; set; }
         Nullable<int> LivingArea { get; set; }
         Nullable<int> TotalArea { get; set; }
         Nullable<int> RoomCount { get; set; }
         string PostCode { get; set; }
         Nullable<System.DateTime> DateAdded { get; set; }
         Nullable<System.DateTime> DateRemoved { get; set; }
         Nullable<System.DateTime> DateLastProcessed { get; set; }
         string HeatingType { get; set; }
        Nullable<bool> IsBendrabutis { get; set; }
    }
}