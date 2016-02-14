using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace DataTier
{
    class Program
    {
        /*Opens the connection*/
        static void Main(string[] args)
        {

            ServiceHost host;
            NetTcpBinding tcpBinding = new NetTcpBinding();
            DataControllerImpl dataObj = new DataControllerImpl();
            tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;

            host = new ServiceHost(dataObj);
            host.AddServiceEndpoint(typeof(IDataController), tcpBinding, "net.tcp://localhost:50001/Data");
            host.Open();
            System.Console.WriteLine("***DATA TIER***");
            System.Console.WriteLine("Server is Running. Press ENTER to exit");
            System.Console.ReadLine();
            host.Close();

        }
    }
}
