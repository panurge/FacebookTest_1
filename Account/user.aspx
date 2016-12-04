<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user.aspx.cs" Inherits="FacebookLoginASPnetWebForms.account.user" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ListView ID="ListView1" runat="server">
                <ItemTemplate>
                    <table>
                        <tr>
                            <td>Id:</td>
                            <td><%# Eval("id") %><br />
                            </td>
                        </tr>
                        <tr>
                            <td>First Name:</td>
                            <td><%# Eval("first_name") %><br />
                            </td>
                        </tr>
                        <tr>
                            <td>Last Name:</td>
                            <td><%# Eval("last_name") %><br />
                            </td>
                        </tr>
                        <tr>
                            <td>Link:</td>
                            <td><%# Eval("link") %><br />
                            </td>
                        </tr>
                        <tr>
                            <td>Gender:</td>
                            <td><%# Eval("gender") %><br />
                            </td>
                        </tr>
                        <tr>
                            <td>Locale:</td>
                            <td><%# Eval("locale") %><br />
                            </td>
                        </tr>
                        <tr>
                            <td>Birthday:</td>
                            <td><%# Eval("birthday") %><br />
                            </td>
                        </tr>
                        <tr>
                            <td>posts:</td>
                            <td><br />
                            </td>
                        </tr>

                    </table>
                </ItemTemplate>
            </asp:ListView>
               <asp:ListBox ID="ListBox1" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ListBox1_SelectedIndexChanged" Width="489px" Height="265px"></asp:ListBox>
               <asp:TextBox rows="30" columns="200" TextMode="MultiLine" id="ploo" runat="server"></asp:TextBox>

        </div>
    </form>
</body>
</html>
