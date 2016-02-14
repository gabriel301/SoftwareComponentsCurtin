<%@ Page Title="Home Page" Language="C#"  AutoEventWireup="true"
    CodeFile="Home.aspx.cs" Inherits="Home" EnableEventValidation="false" %>
 <html xmlns="http://www.w3.org/1999/xhtml" >
 <head runat="server">

    <style type="text/css">
        .style1
        {
            width: 222px;
        }
        .style2
        {
            width: 76px;
        }
        #txtPages
        {
            width: 61px;
        }
        #txtPage
        {
            width: 70px;
        }
        #Text1
        {
            width: 72px;
        }
        #Button1
        {
            margin-top: 0px;
        }
        .style3
        {
            width: 93px;
        }
        .style4
        {
            width: 125px;
        }
    </style>
</head>

<div id="Grid">
<form id="formGrid" runat="server">
<asp:GridView ID="GridView1" runat="server" onrowcommand="GridView1_RowCommand" 
    onrowcreated="GridView1_RowCreated" BackColor="White" 
    BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
    ForeColor="Black" GridLines="Vertical">
    <AlternatingRowStyle BackColor="White" />
    <Columns>
        <asp:TemplateField>
  <ItemTemplate>
    <asp:Button ID="btnView" runat="server" 
      CommandName="btnView" 
      CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
      Text="View Details" />
  </ItemTemplate> 
</asp:TemplateField>
       <asp:TemplateField>
  <ItemTemplate> 
    <asp:Button ID="btnEdit" runat="server" 
      CommandName="btnEdit" 
      CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
      Text="Edit Details" />
  </ItemTemplate> 
</asp:TemplateField>
    </Columns>
    <FooterStyle BackColor="#CCCC99" />
    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
    <RowStyle BackColor="#F7F7DE" />
    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
    <SortedAscendingCellStyle BackColor="#FBFBF2" />
    <SortedAscendingHeaderStyle BackColor="#848384" />
    <SortedDescendingCellStyle BackColor="#EAEAD3" />
    <SortedDescendingHeaderStyle BackColor="#575357" />
</asp:GridView>
</div>

<div id="foot">
   <asp:Panel Id="panel1" runat="server" DefaultButton="btnGoToPage">
   <table  style="width:78%;" > 
        <tr>
            <td class="style1"  >
                &nbsp;&nbsp;&nbsp; Page&nbsp;
                <asp:TextBox id="txtPage" name="txtPage" runat="server" type="text" text="1" /></asp:TextBox> 
            &nbsp;of&nbsp;
                <asp:Label ID="lblNumPages" runat="server" Text = "100"></asp:Label>
       
                <td class="style3">

                <asp:Button ID="btnGoToPage" runat="server" Text="Go to Page" 
                        onserverclick="btnGoToPage_ServerClick" onclick="btnGoToPage_ServerClick"> </asp:Button>

                </td>
                             
            <td class="style2">
                <input id="btnPrevious" runat="server" type="button" value="<<" onserverclick="btnPrevious_ServerClick" /> 
                <input id="btnNext" runat="server" type="button" value=">>" onserverclick="btnNext_ServerClick"  />
            </td>
             <td class="style4" >                            
             
             
                 <asp:Button ID="btnAdd" runat="server" Text="Add Item" 
                     onclick="btnAdd_ServerClick" />
             
             
            </td>   
        </tr>
       
    </table> 
     </asp:Panel> 
 </div>

</form>


