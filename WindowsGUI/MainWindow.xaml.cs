using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataTier;
using System.ServiceModel;

namespace WindowsGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal IDataController m_data; //Reference to the server
        pageRecord m_recordPage; //List with page records

        /// <summary>
        /// This method performs a connection to the server,get the first page
        /// of records and populate the grid with it.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            ChannelFactory<IDataController> dataFactory;
            NetTcpBinding tcpBinding = new NetTcpBinding();

        
            tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;
            tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;
            string sURL = "net.tcp://localhost:50001/Data";

            dataFactory = new ChannelFactory<IDataController>(tcpBinding, sURL);

            m_data = dataFactory.CreateChannel();

            string [] columns = m_data.GetColumnNames();

            /*Binds the columns with the name of the fields of the table*/
            foreach (string name in columns)
            {
                DataGridTextColumn textcol = new DataGridTextColumn();
                Binding b = new Binding(name);
                textcol.Binding = b;
                textcol.Header = name; 
                dgrdData.Columns.Add(textcol);
            }

            m_recordPage = new pageRecord();
            txtPage.Text = "1";
            UpdateDataGrid(1);
           
        }


        /// <summary>
        /// This method performs the validation of number pages, populates the grid with data
        /// and it hides the information that might not be shown(notes, etc)
        /// NOTE: The page has 100 records.
        /// </summary>
        private void UpdateDataGrid(int page)
        {
            
            int starRow, endRow, numRows;
            starRow = (page -1) * 100;
            endRow = (page*100) -1;
            numRows = m_data.getNumRows();
            
            if (endRow > (numRows - 1))
                endRow = (numRows - 1);

            if (starRow >= 0 && starRow < numRows)
            {
                m_recordPage = m_data.GetPageRecord(starRow, endRow);
                dgrdData.ItemsSource = m_recordPage.Records;
                dgrdData.IsReadOnly = true;
                dgrdData.Columns[4].Visibility = Visibility.Hidden;
                dgrdData.Columns[5].Visibility = Visibility.Hidden;
                dgrdData.Columns[7].Visibility = Visibility.Hidden;
                dgrdData.Columns[8].Visibility = Visibility.Hidden;
                dgrdData.Columns[10].Visibility = Visibility.Hidden;
                dgrdData.Columns[11].Visibility = Visibility.Hidden;
                dgrdData.SelectedIndex = 0;

                /*If the number of rows returned by the server is not divisible by 100,
                 * it means that the last page has less records than 100 records. So, it is add 1 at number
                 * pages.*/
                lblTotalPages.Content = numRows % 100 != 0 ? " of " + Convert.ToString((numRows / 100) + 1) : " of " + Convert.ToString((numRows / 100));
                txtPage.Text = page.ToString();

            }
            else
            {
                MessageBox.Show("This is not a valid page number");
            }

        }

        /*Back one page*/
        private void btnPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid(Convert.ToInt32(txtPage.Text)-1);
        }

        /*Go Foward one page*/
        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid(Convert.ToInt32(txtPage.Text) + 1);
        }
        
        /*Jumps to the page number that is in textbox by pressing Enter key*/
        private void txtPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UpdateDataGrid(Convert.ToInt32(txtPage.Text));
            }

        }

        /*Method called at OnClose() event from Dialog box. It updates current page loaded
         * in main form. */
        public void OnItemUpdate()
        {
            UpdateDataGrid(Convert.ToInt32(txtPage.Text));
        }

        /*Opens the dialog form in view mode*/
        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            int gridIndex = dgrdData.SelectedIndex;
            DialogWindow diagWindow = new DialogWindow(1, m_recordPage.Records[gridIndex], OnItemUpdate, m_data);
            diagWindow.Show();
        }

        /*Opens the dialog form in edit mode*/
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            int gridIndex = dgrdData.SelectedIndex;
            DialogWindow diagWindow = new DialogWindow(2, m_recordPage.Records[gridIndex], OnItemUpdate, m_data);
            diagWindow.Show();
        }

        /*Opens the dialog form in add mode*/
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            int gridIndex = dgrdData.SelectedIndex;
            DialogWindow diagWindow = new DialogWindow(0, m_recordPage.Records[gridIndex], OnItemUpdate, m_data);
            diagWindow.Show();
        }

    }

    
}
