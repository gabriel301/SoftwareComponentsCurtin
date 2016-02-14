using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DataTier;


// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService" in both code and config file together.
[ServiceContract]
public interface IService
{
    [OperationContract]
    int getNumRows();

    [OperationContract]
    string[] GetColumnNames();

    [OperationContract]
    int[] GetItemIDList();

    [OperationContract]
    int[] GetItemIDListByRange(int startRow, int endRow);

    [OperationContract]
    dbRecord GetItemRecord(int id);

    [OperationContract]
    int AddItemRecord(dbRecord itemRecord);

    [OperationContract]
    bool UpdateItemRecord(dbRecord itemRecord);

    [OperationContract]
    pageRecord GetPageRecord(int startrow, int endrow);
	
}
