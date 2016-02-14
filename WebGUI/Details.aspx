<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Details.aspx.cs" Inherits="Details" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   
    <style type="text/css">
        .style2
        {
            width: 270px;
        }
        .style3
        {
            width: 371px;
        }
        .style4
        {
            width: 371px;
            height: 144px;
        }
        .style5
        {
            height: 144px;
        }
        #Buttons
        {
            width: 651px;
        }
        .style6
        {
            width: 370px;
        }
    </style>
</head>

<body onunload="window.opener.location.reload();">
    <form id="form1" runat="server">
    <div id="Hidden">
        <asp:HiddenField ID="ItemId" runat="server" ViewStateMode="Enabled" />
        <asp:HiddenField ID="LastModified" runat="server" ViewStateMode="Enabled" />
        <asp:HiddenField ID="PageMode" runat="server" ViewStateMode="Enabled" />
    </div>

    <div id="Fields">
     <table>
     <tr>
        <td class="style3">
        <asp:Label ID="lblID" runat="server" Text="ID"></asp:Label>
        
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtID" runat="server" Width="150px"></asp:TextBox>
        
        </td>
        <td class="style2">
        <asp:Label ID="lblCategory" runat="server" Text="Category"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtCategory" runat="server" Width="150px"></asp:TextBox>
        </td>
     </tr>

     <tr>
        <td class="style3">
        <asp:Label ID="lblBrand" runat="server" Text="Brand"></asp:Label>
        
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
            <asp:TextBox ID="txtBrand" runat="server" Width="150px"></asp:TextBox>
        
        </td>
        <td class="style2">
        <asp:Label ID="lblModel" runat="server" Text="Model"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtModel" runat="server" Width="150px"></asp:TextBox>
        </td>
     </tr>
   
      <tr>
        <td class="style3">
        <asp:Label ID="lblReceivedDate" runat="server" Text="Received Date"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtReceivedDate" runat="server" Width="150px"></asp:TextBox>
        
        </td>
        <td class="style2">
        <asp:Label ID="lblWeight" runat="server" Text="Weight"></asp:Label>
        &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtWeight" runat="server" Width="150px"></asp:TextBox>
        </td>
     </tr>

          <tr>
        <td class="style3">
        <asp:Label ID="lblCondition" runat="server" Text="Condition"></asp:Label>
        
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtCondition" runat="server" Width="150px"></asp:TextBox>
        
        </td>
        <td class="style2">
        <asp:Label ID="lblLocation" runat="server" Text="Location"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtLocation" runat="server" Width="150px"></asp:TextBox>
        </td>
     </tr>

          <tr>
        <td class="style3">
        <asp:Label ID="lblPurshasePrice" runat="server" Text="Purshase Price"></asp:Label>
        
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtPurshasePrice" runat="server" Width="150px"></asp:TextBox>
        
        </td>
        <td class="style2">
        <asp:Label ID="lblSellingPrice" runat="server" Text="Selling Price"></asp:Label>
        &nbsp;&nbsp;
            <asp:TextBox ID="txtSellingPrice" runat="server" Width="150px"></asp:TextBox>
        </td>
     </tr>

     <tr>
     <td class="style4" >
      <asp:Label ID="lblNotes" runat="server" Text="Notes" ></asp:Label>
        
         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        
         <asp:TextBox ID="txtNotes" runat="server" style="resize:none" Width="231px" Height="127px" 
             TextMode="MultiLine" BorderStyle="NotSet"></asp:TextBox>
     </td>
     <td class="style5" valign="bottom" align="right">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" 
                onclick="btnSubmit_ServerClick"  />
     </td>
     </tr>
     </table>    
    </div>

    </form>
</body>
</html>
