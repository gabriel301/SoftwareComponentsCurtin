using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DataTier
{
    [DataContract]
    public class dbRecord
    {

        private static int times = 0;
        //.NET Property of member fields
        [DataMember]
        public int ItemID { get; set; }

        [DataMember]
        public string Category { get; set; }

        [DataMember]
        public string Brand { get; set; }

        [DataMember]
        public string Model { get; set; }

        [DataMember]
        public DateTime ReceivedDate { get; set; }

        [DataMember]
        public int Weight { get; set; }

        [DataMember]
        public string Condition { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public double PurchasePrice { get; set; }

        [DataMember]
        public double SellingPrice { get; set; }

        [DataMember]
        public string Notes { get; set; }

        [DataMember]
        public DateTime LastUpdated { get; set; }

        // Instance constructors.
        
        public dbRecord() { }
        
        public dbRecord(int id, string category, string brand, string model,
                        DateTime receivedDate, int weight, string condition, string location,
                        double purchasePrice, double sellingPrice, string notes, DateTime lastUpdated)
        {
            this.Category = category;
            this.Brand = brand;
            this.Model = model;
            this.ReceivedDate = receivedDate;
            this.Weight = weight;
            this.Condition = condition;
            this.Location = location;
            this.PurchasePrice = purchasePrice;
            this.SellingPrice = sellingPrice;
            this.Notes = notes;
            this.LastUpdated = lastUpdated;
            this.ItemID = id;

        }


        //Copy Constructor
       
        public dbRecord(dbRecord record)
        {
            this.Category = record.Category;
            this.Brand = record.Brand;
            this.Model = record.Model;
            this.ReceivedDate = record.ReceivedDate;
            this.Weight = record.Weight;
            this.Condition = record.Condition;
            this.Location = record.Location;
            this.PurchasePrice = record.PurchasePrice;
            this.SellingPrice = record.SellingPrice;
            this.Notes = record.Notes;
            this.LastUpdated = record.LastUpdated;
            this.ItemID = record.ItemID;
        }
       

    }

    [DataContract]
    public class pageRecord
    {
        [DataMember]
        public List<dbRecord> Records { get; set; }

        
        public pageRecord() 
        {
            Records = new List<dbRecord>();
        }
    }

}
