using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataTier;
using System.Data;
using WebService;
public partial class Home : System.Web.UI.Page
{
    ServiceClient m_service;

    /// <summary>
    /// This method performs a connection to the server,get the first page
    /// of records and populate the grid with it.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {

        m_service = new ServiceClient();
        if (!Page.IsPostBack)
        {
            UpdatePageInfo(1);
        }
                            
    }



    /*Open a popup edit window or a popup view window when Edit or View button is clicked on the Grid.
     The code retrive the ID of the record and it stores it in a Session, as well store the
     Page mode (View or Edit).*/
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "btnView")
        {
            // Retrieve the row index stored in the 
            // CommandArgument property.
            int index = Convert.ToInt32(e.CommandArgument);
            Session.Clear();
            Session["ID"] = GridView1.Rows[index].Cells[2].Text;
            Session["Mode"] = "View";
        }
            
        else if (e.CommandName == "btnEdit")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            Session.Clear();
            Session["ID"] = GridView1.Rows[index].Cells[2].Text;
            Session["Mode"] = "Edit";
        }
        Page.ClientScript.RegisterStartupScript(this.GetType(), "openwindow", "window.open('Details.aspx','','height=500,width=650')", true);
    }

    /*Hides the columns that columns that might not be shown (notes, etc) when new row
     is created in the grid*/
    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[6].Visible = false;
        e.Row.Cells[7].Visible = false;
        e.Row.Cells[9].Visible = false;
        e.Row.Cells[10].Visible = false;
        e.Row.Cells[12].Visible = false;
        e.Row.Cells[13].Visible = false;
    }

    /*Go one record page foward*/
    protected void btnNext_ServerClick(object sender, EventArgs e)
    {
         int page = Convert.ToInt32(txtPage.Text) + 1;
            UpdatePageInfo(page);
    }

    /*Back one record page*/
    protected void btnPrevious_ServerClick(object sender, EventArgs e)
    {      
       
            int page = Convert.ToInt32(txtPage.Text) - 1;
            UpdatePageInfo(page);
            
    }

    /*Jumps to the number page in the textbox*/
    protected void btnGoToPage_ServerClick(object sender, EventArgs e)
    {

        int page = Convert.ToInt32(txtPage.Text);
        UpdatePageInfo(page);

    }

    /*Opens a popup Add window. It stores the page mode and the tag <new item> in a session*/
    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {
        Session.Clear();
        Session["Mode"] = "Add";
        Session["ID"] = "< new item >";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "openwindow", "window.open('Details.aspx','','height=500,width=650')", true);
       
    }


    /// <summary>
    /// This method performs the validation of number pages, populates the grid with data
    /// and it hides the information that might not be shown(notes, etc)
    /// NOTE: The page has 100 records.
    /// </summary>
    void UpdatePageInfo(int page)
    {
        
        int starRow, endRow, numRows;
        starRow = (page - 1) * 100;
        endRow = (page * 100) - 1;
        numRows = m_service.getNumRows();
        if (endRow > (numRows - 1))
            endRow = (numRows - 1);

        if (starRow >= 0 && starRow < numRows)
        {
            GridView1.DataSource = m_service.GetPageRecord(starRow, endRow).Records;
            GridView1.DataBind();
            /*If the number of rows returned by the server is not divisible by 100,
             * it means that the last page has less records than 100 records. So, it is add 1 at number
             * pages.*/
            lblNumPages.Text = numRows % 100 != 0 ? Convert.ToString((numRows / 100) + 1) : Convert.ToString((numRows / 100));
            txtPage.Text = page.ToString();

        }
        else
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "window.alert('This is not a valid page number')", true);
        }

    }
}
