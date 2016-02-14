using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebService;
using DataTier;
public partial class Details : System.Web.UI.Page
{
    dbRecord m_record; // Record from DB
    ServiceClient m_service; //Reference to the Web Service

    /*Load information in the field and enables/disables textboxes and buttons
      according to which mode the page is opened
     NOTE: ItemId, PageMode and LastModified are hidden fiels in the page
     that stores the ID of current item, the mode which page is opened and the Last Update date
     of the record, once webpages are stateless*/
    protected void Page_Load(object sender, EventArgs e)
    {
        m_service = new ServiceClient();

        if(!IsPostBack)
        {
            ItemId.Value = Session["ID"].ToString();
            PageMode.Value = Session["Mode"].ToString();

            if (PageMode.Value == "Edit")
            {
                this.Page.Title = "Edit Item";
                LoadInfo();
                EnableControls(true);
                btnSubmit.Text = "Save";
            
            }
            else if (PageMode.Value == "View")
            {
                this.Page.Title = "View Details";
                LoadInfo();
                EnableControls(false);
            }

            if (PageMode.Value == "Add")
            {
                this.Page.Title = "Add New Item";
                txtID.Text = ItemId.Value;
                btnSubmit.Text = "Add";
                EnableControls(true);
            }
      }  
    }

    /*Loads the data from textboxes to the record and add or edit the record in
     database. Also shows the ID or whether the user might retry editing the record
     After page response, the main page is reloaded on the onunload event of the window by
     a javascrip on <body> tag of the page*/
    protected void btnSubmit_ServerClick(object sender, EventArgs e)
    {
        SetRecord();
        if (PageMode.Value == "Edit")
        {
            bool response = m_service.UpdateItemRecord(m_record);
            if (response)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "openwindow", "window.alert('Item Edited');", true);
            }
            else 
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "openwindow", "window.alert('It could not edit this item. Please retry');", true);
            }
        }
        else if (PageMode.Value == "Add")
        {
            int id = m_service.AddItemRecord(m_record);
            string msg = "Item Added. ID: " + id.ToString();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "openwindow", "window.alert('"+ msg+ "');", true);
            
        }
        Page.ClientScript.RegisterStartupScript(this.GetType(), "ClosePopup", "window.close();", true);
    }

    /*Enables/disables controls*/
    private void EnableControls(bool option)
    {
        txtID.Enabled = false;
        txtBrand.Enabled = option;
        txtCategory.Enabled = option;
        txtCondition.Enabled = option;
        txtLocation.Enabled = option;
        txtModel.Enabled = option;
        txtPurshasePrice.Enabled = option;
        txtReceivedDate.Enabled = option;
        txtSellingPrice.Enabled = option;
        txtWeight.Enabled = option;
        txtNotes.Enabled = option;
        btnSubmit.Enabled = option;
        btnSubmit.Visible = option;
    }

    /*Loads the data from textboxes to the record*/
    private void SetRecord()
    {
        m_record = new dbRecord();
        m_record.Brand = txtBrand.Text;
        m_record.Category = txtCategory.Text;
        m_record.Condition = txtCondition.Text;
        if (PageMode.Value == "Edit")
        {
            m_record.ItemID = Convert.ToInt32(ItemId.Value);
            m_record.LastUpdated = new DateTime(Convert.ToInt64(LastModified.Value));
        }

        m_record.Location = txtLocation.Text;
        m_record.Model = txtModel.Text;
        m_record.Notes = txtNotes.Text;
        m_record.PurchasePrice = Convert.ToDouble(txtPurshasePrice.Text);
        m_record.ReceivedDate = Convert.ToDateTime(txtReceivedDate.Text);
        m_record.SellingPrice = Convert.ToDouble(txtSellingPrice.Text);
        m_record.Weight = Convert.ToInt32(txtWeight.Text);
        

    }

    /*Loads data from the record to textboxes*/
    private void LoadInfo()
    {
        m_record = new dbRecord(m_service.GetItemRecord(Convert.ToInt32(ItemId.Value)));
        txtID.Text = m_record.ItemID.ToString();
        txtBrand.Text = m_record.Brand;
        txtCategory.Text = m_record.Category;
        txtCondition.Text = m_record.Condition;
        txtLocation.Text = m_record.Location;
        txtModel.Text = m_record.Model;
        txtPurshasePrice.Text = m_record.PurchasePrice.ToString();
        txtReceivedDate.Text = m_record.ReceivedDate.ToString();
        txtSellingPrice.Text = m_record.SellingPrice.ToString();
        txtWeight.Text = m_record.Weight.ToString();
        txtNotes.Text = m_record.Notes;

        /*Converts the date to ticks and then store it. When date is converted to string
         the ticks differ from the original date, although date, hour, minutes and seconds are equal*/
        LastModified.Value = m_record.LastUpdated.Ticks.ToString();
    }
    
}