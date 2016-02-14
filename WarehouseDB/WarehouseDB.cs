using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace WarehouseDB
{

    /// <summary>
    /// Class that controls access to an underlying database, providing
    /// only enough access to maintain Accounts and Transactions but
    /// disallowing the ability to delete entries for reasons of ensuring
    /// an audit trail.
    /// The database is in fact in-memory, not on a server.
    /// 
    /// NOTE: This class has NO synchronisation. This is important for
    /// avoiding lost updates, hence your Data tier class will need to
    /// perform thread synchronisation wherever it updates the database.
    /// 
    /// </summary>
    public class WarehouseDB
    {

        //
        //////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////
        // MEMBER VARIABLES
        ////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////
        //
        // The in-memory database
        //
        private DataSet m_dsWarehouse;


        //
        //////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////
        // CONSTRUCTORS
        ////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////
        //

        /// <summary>
        /// Initialise the database, populating it with a million
        /// randomly-generated records
        /// </summary>
        public WarehouseDB() {
            InitialiseDatabase(1000000);
        }



        //
        //////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////
        // PUBLIC METHODS
        ////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////
        //

        /// <summary>
        /// Gets the number of rows in the Warehouse table (read-only).
        /// </summary>
        /// <value>The number of rows.</value>
        public int NumRows
        {
            get { return m_dsWarehouse.Tables["Warehouse"].Rows.Count; }
        }


        public string[] GetColumnNames()
        {
            string[] sColNames;
            int ii;

            sColNames = new string[ m_dsWarehouse.Tables["Warehouse"].Columns.Count ];
            ii = 0;
            foreach (DataColumn c in m_dsWarehouse.Tables["Warehouse"].Columns) {
                sColNames[ii] = c.ColumnName;
                ii++;
            }

            return sColNames;
        }



        /// <summary>
        /// Gets all Item IDs for the Warehouse table.
        /// </summary>
        /// <returns>An int array of the IDs</returns>
        public int[] GetItemIDList()
        {
            return GetItemIDList(0, m_dsWarehouse.Tables["Warehouse"].Rows.Count-1);
        }

        /// <summary>
        /// Gets all Item IDs for the Warehouse table for a given subset of rows
        /// </summary>
        /// <param name="startRow">The start row.</param>
        /// <param name="endRow">The end row.</param>
        /// <returns>An int array of the IDs for the given subset of rows</returns>
        public int[] GetItemIDList(int startRow, int endRow)
        {
            int iNumRows;
            int[] itemIDs;
            DataRowCollection rows;

            rows = m_dsWarehouse.Tables["Warehouse"].Rows;
            iNumRows = rows.Count;

            if (startRow < 0)
                throw new ArgumentException("startRow cannot be negative");
            if (endRow >= iNumRows)
                throw new ArgumentException("endRow exceeds the number of rows, which is : " + iNumRows);
            if (endRow < startRow)
                throw new ArgumentException("endRow must be after startRow");

            itemIDs = new int[endRow - startRow + 1];

            // Get each ID for each row
            // Don't just assume IDs are consecutive numbers - what if we allowed deletions?
            for (int ii = startRow; ii <= endRow; ii++) {
                itemIDs[ii-startRow] = (int)rows[ii]["ItemID"];
            }
            return itemIDs;
        }


        /// <summary>
        /// Gets an Item record from the database
        /// </summary>
        public void GetItemRecord(int itemID, out string category, out string brand, out string model, 
                                    out DateTime receivedDate, out int weight, out string condition, out string location,
                                    out double purchasePrice, out double sellingPrice, out string notes, out DateTime lastUpdated) 
        {
            DataRow[] rows;

            rows = m_dsWarehouse.Tables["Warehouse"].Select("ItemID = " + itemID);
            if ((rows == null) || (rows.Length == 0))
                throw new ArgumentException("ItemID " + itemID + " not found");
            else {
                // Should only be one row returned
                // Use ordinal column indexing for speed reasons, since the caller is
                // likely to be in the middle of requesting a batch of ItemRecords at once
                category = (string)rows[0][1]; // ["Category"];
                brand = (string)rows[0][2]; //["Brand"];
                model = (string)rows[0][3]; // ["Model"];
                receivedDate = (DateTime)rows[0][4];   //["ReceivedDate"];
                weight = (int)rows[0][5];   //["Weight"];
                condition = (string)rows[0][6];   //["Condition"];
                location = (string)rows[0][7];   //["Location"];
                purchasePrice = (double)rows[0][8];   //["PurchasePrice"];
                sellingPrice = (double)rows[0][9];   //["SellingPrice"];
                notes = (string)rows[0][10];   //["Notes"];
                lastUpdated = (DateTime)rows[0][11];   //["LastUpdated"]
            }
        }


        
               
          
        /// <summary>
        /// Add a new Item record to the database
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="brand">The brand.</param>
        /// <param name="model">The model.</param>
        /// <param name="receivedDate">The received date.</param>
        /// <param name="weight">The weight.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="location">The location.</param>
        /// <param name="purchasePrice">The purchase price.</param>
        /// <param name="sellingPrice">The selling price.</param>
        /// <param name="notes">The notes.</param>
        /// <returns>The ID of the newly-added item</returns>
        public int AddItemRecord(string category, string brand, string model,
                              DateTime receivedDate, int weight, string condition, string location,
                              double purchasePrice, double sellingPrice, string notes)
        {
            DataRow row;
            int iItemID;

            // Set new Account's ID. We can do it using the row count since 
            // Account rows cannot be deleted so gaps will never appear
            iItemID = m_dsWarehouse.Tables["Warehouse"].Rows.Count + 1;

            row = m_dsWarehouse.Tables["Warehouse"].NewRow();
            row["ItemID"] = iItemID;
            row["Category"] = category;
            row["Brand"] = brand;
            row["Model"] = model;
            row["ReceivedDate"] = receivedDate;
            row["Weight"] = weight;
            row["Condition"] = condition;
            row["Location"] = location;
            row["PurchasePrice"] = purchasePrice;
            row["SellingPrice"] = sellingPrice;
            row["Notes"] = notes;
            row["LastUpdated"] = DateTime.Now;
            m_dsWarehouse.Tables["Warehouse"].Rows.Add(row);
            m_dsWarehouse.Tables["Warehouse"].AcceptChanges();

            return iItemID;
        }

        
        /// <summary>
        /// Update an Item record in the database
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="category">The category.</param>
        /// <param name="brand">The brand.</param>
        /// <param name="model">The model.</param>
        /// <param name="receivedDate">The received date.</param>
        /// <param name="weight">The weight.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="location">The location.</param>
        /// <param name="purchasePrice">The purchase price.</param>
        /// <param name="sellingPrice">The selling price.</param>
        /// <param name="notes">The notes.</param>
        public void UpdateItemRecord(int itemID, string category, string brand, string model, 
                                    DateTime receivedDate, int weight, string condition, string location,
                                    double purchasePrice, double sellingPrice, string notes)
        {
            DataRow[] rows;
            rows = m_dsWarehouse.Tables["Warehouse"].Select("ItemID = " + itemID);
            if ((rows == null) || (rows.Length == 0))
                throw new ArgumentException("ItemID " + itemID + " not found");
            else {
                rows[0]["Category"] = category;
                rows[0]["Brand"] = brand;
                rows[0]["Model"] = model;
                rows[0]["ReceivedDate"] = receivedDate;
                rows[0]["Weight"] = weight;
                rows[0]["Condition"] = condition;
                rows[0]["Location"] = location;
                rows[0]["PurchasePrice"] = purchasePrice;
                rows[0]["SellingPrice"] = sellingPrice;
                rows[0]["Notes"] = notes;
                rows[0]["LastUpdated"] = DateTime.Now;

                m_dsWarehouse.Tables["Warehouse"].AcceptChanges();
            }
        }





       


        //
        //////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////
        // PRIVATE METHODS
        ////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////
        //

        /// <summary>
        /// Initialise the database, populating basic tables
        /// and auto-generating records for Accounts and Transactions
        /// (but only if iNumInitialAccounts &gt; 0)
        /// </summary>
        /// <param name="iNumInitialAccounts">The number of items to add to the system.</param>
        private void InitialiseDatabase(int iNumInitialItems) 
        {
            CreateDatabaseTables();
            PopulateTable(iNumInitialItems);
            m_dsWarehouse.AcceptChanges();
        }



        /// <summary>
        /// Create the database schema
        /// </summary>
        private void CreateDatabaseTables() 
        {
            DataColumn col;
            DataColumn[] pkCols = new DataColumn[1];
            DataTable tbl;


            m_dsWarehouse = new DataSet();

            // Build tables
            tbl = new DataTable("Warehouse");
            col = tbl.Columns.Add("ItemID", typeof(int));             // Primary Key
            col.AllowDBNull = false; col.Unique = true;
            pkCols[0] = col;
            tbl.PrimaryKey = pkCols;
            tbl.Columns.Add("Category", typeof(string));
            tbl.Columns.Add("Brand", typeof(string));
            tbl.Columns.Add("Model", typeof(string));
            tbl.Columns.Add("ReceivedDate", typeof(DateTime));
            tbl.Columns.Add("Weight", typeof(int));
            tbl.Columns.Add("Condition", typeof(string));
            tbl.Columns.Add("Location", typeof(string));
            tbl.Columns.Add("PurchasePrice", typeof(double));
            tbl.Columns.Add("SellingPrice", typeof(double));
            tbl.Columns.Add("Notes", typeof(string));
            tbl.Columns.Add("LastUpdated", typeof(DateTime));

            m_dsWarehouse.Tables.Add(tbl);          
        }



        /// <summary>
        /// Populate the Account and Transaction tables with random records
        /// </summary>
        /// <param name="numAccounts"></param>
        private void PopulateTable(int numRecords) {
            DataRow row;
            Random randGen = new Random(100);
            DateTime recvDate;
            double fPurchPrice, fSellPrice;
            int iWeight;
            int idxCat, idxBrand, idxModel, idxCondtn, idxLocn;
            int iDay, iMonth, iYear;


            
            string[] sCategories = { "Bed", "Table", "Chair", "Couch", "Bookshelf", "Barbeque", "Heater", "Air Conditioner", "Refrigerator", "Washing Machine", "Dishwasher", "Dryer", "Vacuum Cleaner", "Console", "Computer", "Printer", "Scanner", "TV", "DVD Player", "Stereo", "Car", "Brick", "Nuclear Bomb" };
            double[] fCategoryPrices = { 500,      300,        150,    2000,       100,        250,        200,        400,                1500,           1000,           1000,          1000,    350,            300,        1800,       200,        150,    2500,   250,        200,      50000,    50,     10000000000 };      
            double[] fCategoryWeights = { 100,      80,         30,    100,        20,         75,         10,         40,                 200,            150,            150,            150,    5,              2,          10,         5,          5,       7,     5,           8,        1600,    5,      20000 };

            string[] sBrandFurniture = {"Stanley", "Johnson", "IKEA", "AICO", "Aspen", "Barcalounger", "Bassett", "BDI", "Bernhardt", "Boca Rattan", "Calligaris", "Caluco", "Coja", "Concordia", "Dainolite", "Dayva", "Dorel", "Dutailier", "Ebel", "Ekornes", "Fairmont", "Fredericia", "Fremarc", "Guildcraft", "Habersham", "Hammary", "Harden", "Hekman", "Homelegance", "Immodern", "Jadora", "Kalco", "Karastan", "Klausner", "Lexington", "Lloyd-Frank", "Maitland", "Meadowcraft", "Modus", "Najarian", "Natuzzi", "Norwalk", "Oakwood", "Omnia", "Palliser", "Quoizel", "Ratana", "Restonic", "Revco", "Ridgeway", "Saloom", "Sauder", "Schnadig", "Seabrook", "Sealy", "Simmons", "Strobel", "Tennsco", "Thayer-Coggin", "Vanguard", "Vaughan", "Wynwood", "Zocalo"};
            string[] sBrandWhitegoods = {"Fisher-Paykel", "IXL", "Parkinson", "Bosch", "AEG", "Electrolux", "Hoover", "LG", "Miele", "Panasonic", "Samsung", "Sanyo", "Siemans", "SEBO", "Vax", "Zanussi", "Dyson", "Fratelli", "GE", "Norcool", "Tecnik", "Titan", "Wrighton"};
            string[] sBrandElectronic = {"LG", "Sony", "Panasonic", "Samsung", "Sanyo", "Philips", "Acer", "Alpine", "Fujitsu", "GE", "Hitachi", "JVC", "Luxor", "NEC", "Pioneer", "Sharp", "Tandy", "Toshiba", "Westinghouse"};
            string[] sBrandComputer = {"LG", "Sony", "Panasonic", "Samsung", "Dell", "Philips", "Acer", "Apple", "Fujitsu", "IBM", "Toshiba", "Amstrad", "Hewlett-Packard", "Lenovo", "NEC", "Olivetti", "Xitrix", "Westinghouse"};
            string[] sBrandConsole = {"Microsoft", "Sony", "Nintendo" };
            string[] sBrandCar = { "Audi", "BMW", "Daewoo", "Ford", "Holden", "Honda", "Hyundai", "Mazda", "Mercedes", "Mitsubishi", "Nissan", "Renault", "Subaru", "Suzuki", "Toyota", "Volkswagen", "Porsche", "Ferrari" };
            string[] sBrandBrick = { "Metro", "Collier", "Northcot", "Austral", "Bendigo", "Midland", "Nubrik", "PGH", "Boral", "Selkirk" };
            string[] sBrandNuke = {"USA", "Russia", "France", "Britain", "China", "India", "Pakistan"};

            string[] sTV = { "21-inch LCD", "24-inch LCD", "26-inch LCD", "32-inch LCD", "40-inch LCD", "48-inch LCD", "32-inch Plasma", "40-inch Plasma", "48-inch Plasma", "56-inch Plasma" };

            string[] sConsoleMS = { "XBox", "XBox 360", "XBox 360 w/Kinect", "XBox One" };
            string[] sConsoleSony = { "Playstation", "Playstation 2", "Playstation 3", "Playstation Portable", "Playstation 4" };
            string[] sConsoleNintendo = { "Wii", "Wii U", "GameBoy", "DS", "GameCube" };

            string[] sComputer = {   "Fusion A4-5300 3.4GHz", "Fusion A6-5400 3.6Ghz", "Fusion A10-7700 3.8GHz",
                                     "Kaveri A10-7700 3.8GHz", "Kaveri A10-7850 4Ghz", "Piledriver FX-6300 3.5GHz", "Piledriver FX-8350 4Ghz",
                                     "Core i3 4130 3.4GHz", "Core i5 4570 3.2GHz", "Core i5 4670 3.4GHz", 
                                     "Core i7 4770 3.4GHz", "Core i7 4820 3.7GHz", "Core i7 4930 3.4GHz", "Core i7 4960 3.6GHz", "Xeon E3-1240 3.4GHz" };
            string[] sPrinter = { "Dot-matrix", "Bubble Jet", "Laser", "Hi-speed Laser", "Colour Laser", "Hi-speed Colour Laser" };
            string[] sGeneric = { "Standard", "Basic", "Econo", "Value", "Regular", "Premium", "Grand", "Deluxe", "Majestic", "Super-deluxe" };
            
            string[] sBed = { "Single", "Double", "Queen", "King", "Single-Spring", "Double-Spring", "Queen-Spring", "King-Spring", "Waterbed", "Futon" };

            string[] sNuke = { "Little Boy", "Fat Man", "Tsar Bomba", "H-Bomb", "Dirty Bomb", "W-87 Warhead" }; 

            string[] sCarAudi = {"A3", "A4", "A5", "A6", "Q5", "TT"};
            string[] sCarBMW = {"120i", "318i", "320i", "325i", "335l", "525i", "530i", "750iL", "M3", "Z4"};
            string[] sCarDaewoo = {"Cielo", "Lanos", "Nubira"};
            string[] sCarFord = {"Escape", "F250", "Falcon", "Festiva", "Focus", "Mondeo", "Mustang", "Ranger"};
            string[] sCarHolden = {"Astra", "Barina", "Berlina", "Commodore", "Cruze", "Statesman", "Ute", "Vectra"};
            string[] sCarHonda = {"Accord", "Civic", "CR-V", "Integra", "Jazz", "Legend", "Prelude"};
            string[] sCarHyundai = {"Accent", "Elantra", "Excel", "Getz", "Sonata"};
            string[] sCarMazda = {"2", "323", "3", "6", "626", "Bravo", "MX-5", "RX-8", "CX-7"};
            string[] sCarMercedes = {"280SE", "A160", "C180", "C200", "CLC200", "CLK320", "E250 CGI", "SLK350"};
            string[] sCarMitsubishi = {"Lancer", "Magna", "Nimbus", "Triton", "Verada"};
            string[] sCarNissan = { "200SX", "300ZX", "Pulsar", "Maxima", "Micra", "Navara", "Silvia", "Skyline", "Pathfinder", "Patrol", "Tilda", "X-Trail" };
            string[] sCarRenault = { "Laguna", "Scenic", "Megane" };
            string[] sCarSubaru = {"Forester", "Impreza", "Liberty", "Outback"};
            string[] sCarSuzuki = {"Alto", "Vitara", "Swift", "Ignis", "Baleno"};
            string[] sCarToyota = {"Aurion", "Avalon", "Camry", "Celica", "Supra", "Corolla", "Hiace", "Hilux", "Landcruiser", "MR2", "Prius", "Rav4", "Tarago", "Yaris"};
            string[] sCarVW = {"Beetle", "Golf", "Ghia", "Passat"};
            string[] sCarPorsche = {"911", "944", "928", "Boxster", "Cayenne", "Cayman", "959"};
            string[] sCarFerrari = { "328GTS", "550 Maranello", "360 Modena", "430", "Testarossa", "612 Scaglietti", "458 Italia", "288 GTO", "F40", "Enzo", "California"};

            string[] sConditions = { "Junk", "Wreck", "Poor", "Average", "Above-Average", "Good", "Very Good", "Excellent", "Near-Mint", "Mint", "New" };
            string[] sLocations = { "Osborne Park", "Cannington", "Balcatta", "Midland", "Joondalup", "Victoria Park" };

            string[][] sBrands;
            string[][][] sModels;


            sBrands = new string[sCategories.Length][];
            sModels = new string[sCategories.Length][][];

            sBrands[0] = sBrandFurniture;   // Bed
            sModels[0] = new string[sBrandFurniture.Length][];
            for (int ii = 0; ii < sBrandFurniture.Length; ii++)
                sModels[0][ii] = sBed;

            for (int bb = 1; bb <= 5; bb++) {   // Table/desk, Chair, Couch, Bookshelf, BBQ
                sBrands[bb] = sBrandFurniture;   // Table/Desk
                sModels[bb] = new string[sBrandFurniture.Length][];
                for (int ii = 0; ii < sBrandFurniture.Length; ii++)
                    sModels[bb][ii] = sGeneric;
            }

            for (int bb = 6; bb <= 12; bb++) { // Heater, Aircon, Fridge, Washing, Dishwasher, Dryer, Vacuum Cleaner
                sBrands[bb] = sBrandWhitegoods;   // Heater
                sModels[bb] = new string[sBrandWhitegoods.Length][];
                for (int ii = 0; ii < sBrandWhitegoods.Length; ii++)
                    sModels[bb][ii] = sGeneric;
            }

            sBrands[13] = sBrandConsole;   // Console
            sModels[13] = new string[sBrandConsole.Length][];
            sModels[13][0] = sConsoleMS;
            sModels[13][1] = sConsoleSony;
            sModels[13][2] = sConsoleNintendo;

            sBrands[14] = sBrandComputer;   // Computer
            sModels[14] = new string[sBrandComputer.Length][];
            for (int ii = 0; ii < sBrandComputer.Length; ii++)
                sModels[14][ii] = sComputer;

            sBrands[15] = sBrandComputer;   // Printer
            sModels[15] = new string[sBrandComputer.Length][];
            for (int ii = 0; ii < sBrandComputer.Length; ii++)
                sModels[15][ii] = sPrinter;

            sBrands[16] = sBrandComputer;   // Scanner
            sModels[16] = new string[sBrandComputer.Length][];
            for (int ii = 0; ii < sBrandComputer.Length; ii++)
                sModels[16][ii] = sGeneric;

            sBrands[17] = sBrandElectronic;   // TV
            sModels[17] = new string[sBrandElectronic.Length][];
            for (int ii = 0; ii < sBrandElectronic.Length; ii++)
                sModels[17][ii] = sTV;

            for (int bb = 18; bb <= 19; bb++) { // DVD player, Stereo           
                sBrands[bb] = sBrandElectronic;
                sModels[bb] = new string[sBrandElectronic.Length][];
                for (int ii = 0; ii < sBrandElectronic.Length; ii++)
                    sModels[bb][ii] = sGeneric;
            }

            sBrands[20] = sBrandCar;    // Cars
            sModels[20] = new string[sBrandCar.Length][];
            sModels[20][0] = sCarAudi;      sModels[20][1] = sCarBMW;       sModels[20][2] = sCarDaewoo; 
            sModels[20][3] = sCarFord;      sModels[20][4] = sCarHolden;    sModels[20][5] = sCarHonda; 
            sModels[20][6] = sCarHyundai;   sModels[20][7] = sCarMazda;     sModels[20][8] = sCarMercedes;
            sModels[20][9] = sCarMitsubishi; sModels[20][10] = sCarNissan;  sModels[20][11] = sCarRenault; 
            sModels[20][12] = sCarSubaru;   sModels[20][13] = sCarSuzuki;   sModels[20][14] = sCarToyota; 
            sModels[20][15] = sCarVW;       sModels[20][16] = sCarPorsche;  sModels[20][17] = sCarFerrari;

            sBrands[21] = sBrandBrick;   // Bricks
            sModels[21] = new string[sBrandBrick.Length][];
            for (int ii = 0; ii < sBrandBrick.Length; ii++)
                sModels[21][ii] = sGeneric;

            sBrands[22] = sBrandNuke;   // Nukes
            sModels[22] = new string[sBrandNuke.Length][];
            for (int ii = 0; ii < sBrandNuke.Length; ii++)
                sModels[22][ii] = sNuke;


            //
            // Generate items, randomly choosing values
            //
            for (int ii = 0; ii < numRecords; ii++) {

                //
                // Fill in the row by randomly choosing values
                //
                idxCat = randGen.Next(sCategories.Length - 0);
                while (idxCat >= 21) {  // Is this one of the weird ones?
                    if (randGen.NextDouble() < 0.01)   // Only do weird ones < 10%*5%=0.5% of the time
                        break;  // Take it
                    else
                        idxCat = randGen.Next(sCategories.Length - 0);  // Try again
                }


                idxBrand = randGen.Next(sBrands[idxCat].Length - 0);
                idxModel = randGen.Next(sModels[idxCat][idxBrand].Length - 0);

                iYear = randGen.Next(2) + 2008; // 2008..2010
                iMonth = randGen.Next(11)+1;    // 1..12
                if (iMonth == 2)
                    iDay = randGen.Next(27)+1;  // Feb has 28 days (never mind leap years)
                else
                    iDay = randGen.Next(29)+1;  // Forget about handling months with 31 days

                recvDate = new DateTime(iYear, iMonth, iDay);


                fPurchPrice = Math.Round( (randGen.NextDouble() * 0.4 - 0.2 + 1.0) * fCategoryPrices[idxCat], 2);    // Vary price by +/-20%
                //fPurchPrice = Math.Round(randGen.NextDouble() * 19999 + 1.0, 2); // $1..$20,000
                fSellPrice = Math.Round(fPurchPrice * (randGen.NextDouble() * 0.5 + 1.0), 0); // fPurchPrice x {1..1.5}

                iWeight = (int)Math.Round((randGen.NextDouble()*0.4 - 0.2 + 1.0) * fCategoryWeights[idxCat], 0);    // Vary wt by +/-20%

                idxCondtn = randGen.Next(sConditions.Length - 0);
                idxLocn = randGen.Next(sLocations.Length - 0);


                // Set the row contents
                row = m_dsWarehouse.Tables["Warehouse"].NewRow();
                
                // Use ordinal indexing because field name hashing is pretty slow
                row[0] = m_dsWarehouse.Tables["Warehouse"].Rows.Count + 1;   // Row isn't added yet, so need +1 to make 1-based
                row[1] = sCategories[idxCat];
                row[2] = sBrands[idxCat][idxBrand];
                row[3] = sModels[idxCat][idxBrand][idxModel];
                row[4] = recvDate.ToString("yyyy-MM-dd");
                row[5] = iWeight;
                row[6] = sConditions[idxCondtn];
                row[7] = sLocations[idxLocn];
                row[8] = fPurchPrice;
                row[9] = fSellPrice;
                row[10] = "";
                row[11] = DateTime.Now;

                m_dsWarehouse.Tables["Warehouse"].Rows.Add(row);
            }
        }

    }
}
