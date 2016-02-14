using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DataTier;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
public class Service : IService
{
    IDataController m_data;
    /// <summary>
    /// Constructor that performs a connection to the server
    /// </summary>
    public Service()
    {
        ChannelFactory<IDataController> dataFactory;
        NetTcpBinding tcpBinding = new NetTcpBinding();
        tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;
        tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;
        string sURL = "net.tcp://localhost:50001/Data";
        dataFactory = new ChannelFactory<IDataController>(tcpBinding, sURL);
        m_data = dataFactory.CreateChannel();
    }

    //Returns  the number of rows in database
    public int getNumRows()
    {
        return m_data.getNumRows();
    }

    //Returns the names of columns from the server
    public string[] GetColumnNames()
    {
        return m_data.GetColumnNames();
    }

    //Returns  a list of all IDs from the server
    public int[] GetItemIDList()
    {
        return m_data.GetItemIDList();
    }

    //Returns a list of IDs in a specific range from the server
    public int[] GetItemIDListByRange(int startRow, int endRow)
    {
        return m_data.GetItemIDListByRange(startRow,endRow);
    }

    //Returns one row from Database
    public dbRecord GetItemRecord(int id)
    {

        return m_data.GetItemRecord(id);

    }

    //Adds a new row in database and returns its ID
    public int AddItemRecord(dbRecord itemRecord)
    {
        return m_data.AddItemRecord(itemRecord);

    }

    //Updates a row in database. Returns true in success or false in fail.
    public bool UpdateItemRecord(dbRecord itemRecord)
    {      
           return  m_data.UpdateItemRecord(itemRecord);       
    }

    //Returns a list of records in a specific range from the server
    public pageRecord GetPageRecord(int startRow, int endRow)
    {
        return m_data.GetPageRecord(startRow, endRow);
    }
}
