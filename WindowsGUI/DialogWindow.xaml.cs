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
using System.Windows.Shapes;
using DataTier;

namespace WindowsGUI
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        public delegate void UpdateResponse();
        UpdateResponse m_updateFunction; //Delegate that Calls the update function from main form
        int m_option; //Controls in which mode the current dialog is.
        dbRecord m_record; // Record loaded in the dialog
        IDataController m_data; //Reference to the server

        /*Initialise the components and loads the data in textboxes, when it is the case*/
        public DialogWindow(int option, dbRecord record, UpdateResponse updateFunction, IDataController data)
        {
            InitializeComponent();
            m_updateFunction = updateFunction;
            m_option = option;
            m_record = record;
            m_data = data;
            LoadInformation();
        }


        /*Load infomormation in the field and enables/disables textboxes and buttons
         according to which mode the window is opened*/
        private void LoadInformation()
        {
            //Add mode
            if (m_option == 0) 
            {
                
                this.Title = "Add New Item";
                DisableControls(false);
                txtId.Text = "< new item >";
                btnSave.Content = "Add";
                btnSave.Visibility = Visibility.Visible;
               

            }
            //View Mode
            else if (m_option == 1)
            {
                this.Title = "Item Details";
                LoadInfo();
                DisableControls(true); ;  
                btnSave.Visibility = Visibility.Hidden;
               
            }
            //Edit
            else if (m_option == 2)
            {
                this.Title = "Edit Details";
                LoadInfo();
                DisableControls(false);
                btnSave.Content = "Save";
                btnSave.Visibility = Visibility.Visible;
                
            }
        }

        //Cloes the window
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }

        //Updates the main form
        private void frmDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            m_updateFunction.Invoke();
        }


        /*Loads the data from textboxes to the record and add or edit the record in
         * database. Also shows the ID or whether the user might retry editing the record*/ 
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            m_record.Brand = txtBrand.Text;
            m_record.Category = txtCategory.Text;
            m_record.Condition = txtCondition.Text;
            m_record.Location = txtLocation.Text;
            m_record.Model = txtModel.Text;
            m_record.Notes = new TextRange(rtbNotes.Document.ContentStart, rtbNotes.Document.ContentEnd).Text; 
            m_record.PurchasePrice = Convert.ToDouble(txtPurshasePrice.Text);
            m_record.ReceivedDate = Convert.ToDateTime(txtReceivedDate.Text);
            m_record.SellingPrice = Convert.ToDouble(txtSellingPrice.Text);
            m_record.Weight = Convert.ToInt32(txtWeight.Text);        
            
            if(btnSave.Content == "Save")
            {
                bool response = m_data.UpdateItemRecord(m_record);
                if(response)
                    MessageBox.Show("Item Edited.");
                else
                    MessageBox.Show("It could not edit this item. Please retry.");
            }
               
            else if (btnSave.Content == "Add")
            {
                int id = m_data.AddItemRecord(m_record);
                MessageBox.Show("Item Added. ID: " + id.ToString());
            }
            this.Close();
        }

        /*Loads data from the record to textboxes*/
        private void LoadInfo()
        {
            m_record = m_data.GetItemRecord(m_record.ItemID);
            txtBrand.Text = m_record.Brand;
            txtCategory.Text = m_record.Category;
            txtCondition.Text = m_record.Condition;
            txtId.Text = m_record.ItemID.ToString();
            txtLocation.Text = m_record.Location;
            txtModel.Text = m_record.Model;
            rtbNotes.AppendText(m_record.Notes);
            txtPurshasePrice.Text = m_record.PurchasePrice.ToString();
            txtReceivedDate.Text = m_record.ReceivedDate.ToString();
            txtSellingPrice.Text = m_record.SellingPrice.ToString();
            txtWeight.Text = m_record.Weight.ToString();
        }

        /*Enables/disables controls*/
        private void DisableControls(bool option)
        {
            txtId.IsReadOnly = true;

            txtBrand.IsReadOnly = option;
            txtCategory.IsReadOnly = option;
            txtCondition.IsReadOnly = option;
            txtLocation.IsReadOnly = option;
            txtModel.IsReadOnly = option;
            rtbNotes.IsReadOnly = option;
            txtPurshasePrice.IsReadOnly = option;
            txtReceivedDate.IsReadOnly = option;
            txtSellingPrice.IsReadOnly = option;
            txtWeight.IsReadOnly = option;

            btnSave.IsEnabled = !option;
        }

    }

}
