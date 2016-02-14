using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using WarehouseDB;
using System.Runtime.CompilerServices;

namespace DataTier
{
    [ServiceBehavior(ConcurrencyMode=ConcurrencyMode.Multiple,UseSynchronizationContext=false,InstanceContextMode = InstanceContextMode.Single)]
    internal class DataControllerImpl : IDataController
    {

        //Object that get DB access
        private WarehouseDB.WarehouseDB dataBase;
        

        /// <summary>
        /// Constructor that initialise the Database. 
        /// The message is just to check whether object instatiation is working properly
        /// </summary>
        public DataControllerImpl()
        {
            System.Console.WriteLine("Initialising Database...");
            this.dataBase  =  new WarehouseDB.WarehouseDB();
            System.Console.WriteLine("Database Ready");
        }

         /// <summary>
        /// Destructor. The message is just to check whether object deinstatiation is working properly
        /// </summary>
        ~DataControllerImpl()
        {
            System.Console.WriteLine("Closing Application");
        }

        //Returns  the number of rows in database
        public int getNumRows()
        {
            return this.dataBase.NumRows;
        }

        //Returns the names of columns from database
        public string[] GetColumnNames()
        {
            return this.dataBase.GetColumnNames();
        }

        //Returns  a list of all IDs from database
        public int[] GetItemIDList()
        {
            return this.dataBase.GetItemIDList();
        }

        //Returns a list of IDs in a specific range from database
        public int[] GetItemIDListByRange(int startRow, int endRow)
        {
            return this.dataBase.GetItemIDList(startRow, endRow);
        }


        //Returns one row from Database
        public dbRecord GetItemRecord(int id)
        {

            int weight;
            string category, brand, model, condition, location, notes;
            double purchasePric,sellingPrice;
            DateTime receivedDate, lastUpdated;
            dbRecord record = new dbRecord();
            this.dataBase.GetItemRecord(id, out category, out brand, out model, out receivedDate, out weight
                                    , out condition, out location, out purchasePric, out sellingPrice, out notes
                                    , out lastUpdated);

            record.Brand = brand;
            record.Category = category;
            record.Condition = condition;
            record.LastUpdated = lastUpdated;
            record.Location = location;
            record.Model = model;
            record.Notes = notes;
            record.PurchasePrice = purchasePric;
            record.ReceivedDate = receivedDate;
            record.SellingPrice = sellingPrice;
            record.Weight = weight;
            record.ItemID = id;

            return record;
        
        }

        //Adds a new row in database and returns its ID
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int AddItemRecord(dbRecord itemRecord)
        {
             return this.dataBase.AddItemRecord(itemRecord.Category, itemRecord.Brand, itemRecord.Model, itemRecord.ReceivedDate, itemRecord.Weight,
                                    itemRecord.Condition, itemRecord.Location, itemRecord.PurchasePrice, itemRecord.SellingPrice, itemRecord.Notes);

        }

        //Updates a row in database. Returns true in success or false in fail.
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool UpdateItemRecord(dbRecord itemRecord)
        {
            dbRecord previousRecord;
            previousRecord = GetItemRecord(itemRecord.ItemID);
            if (previousRecord.LastUpdated == itemRecord.LastUpdated)
            {
                this.dataBase.UpdateItemRecord(itemRecord.ItemID, itemRecord.Category, itemRecord.Brand, itemRecord.Model, itemRecord.ReceivedDate, itemRecord.Weight,
                                    itemRecord.Condition, itemRecord.Location, itemRecord.PurchasePrice, itemRecord.SellingPrice, itemRecord.Notes);
                return true;
            }
            return false;

        }

        //Returns a list of records in a specific range from database
        public pageRecord GetPageRecord(int startRow, int endRow)
        {
            int[] listID;
            pageRecord page = new pageRecord(); 
            listID = this.dataBase.GetItemIDList(startRow, endRow);

            foreach (int id in listID)
            { 
                
                page.Records.Add(new dbRecord(this.GetItemRecord(id)));

                //Note field may be too large and bandwidth can be lost since this 
                //list populates the grid, and notes field is not necessary
                page.Records[page.Records.Count - 1].Notes = "";
            }

            return page;
        }

    }
}
