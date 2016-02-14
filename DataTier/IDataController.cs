using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace DataTier
{
    [ServiceContract]
    public interface IDataController
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
}
