<%@ Control Language="C#" CodeBehind="DialogTemplateContent1.ascx.cs" ClassName="DialogTemplateContent1" Inherits="PivotGridLayoutDemo.Web.DialogTemplateContent1"%>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v15.2" Namespace="DevExpress.ExpressApp.Web.Templates.ActionContainers"
    TagPrefix="cc2" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v15.2" Namespace="DevExpress.ExpressApp.Web.Templates.Controls"
    TagPrefix="tc" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v15.2" Namespace="DevExpress.ExpressApp.Web.Controls"
    TagPrefix="cc4" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v15.2" Namespace="DevExpress.ExpressApp.Web.Templates"
    TagPrefix="cc3" %>
<div class="Dialog" id="DialogContent">
    <table Width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td valign="top" height="100%">
                <cc3:XafUpdatePanel ID="UPVH" runat="server">
                    <div style="display: none">
                        <cc4:ViewImageControl ID="VIC" runat="server" Control-UseLargeImage="false" />
                        <cc4:ViewCaptionControl ID="VCC" runat="server" DetailViewCaptionMode="ViewAndObjectCaption" />
                    </div>
                </cc3:XafUpdatePanel>
                <table class="DialogContent Content" border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td class="ContentCell">
                            <cc3:XafUpdatePanel ID="UPEI" runat="server">
                                <tc:ErrorInfoControl ID="ErrorInfo" Style="margin: 10px 0px 10px 0px" runat="server" />
                            </cc3:XafUpdatePanel>
                            <asp:Table ID="Table1" runat="server" Width="100%" BorderWidth="0px" CellPadding="0" CellSpacing="0">
                                <asp:TableRow ID="TableRow5" runat="server">
                                    <asp:TableCell runat="server" ID="TableCell1" HorizontalAlign="Center">
                                        <cc3:XafUpdatePanel ID="UPSAC" runat="server">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" >
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <cc2:ActionContainerHolder ID="OCC" runat="server" ContainerStyle="Buttons"
                                                            Orientation="Horizontal" PaintStyle="CaptionAndImage" Categories="ObjectsCreation" style="float: left" />
                                                    </td>
                                                    <td align="right" valign="top">
                                                        <cc2:ActionContainerHolder ID="SAC" runat="server" Categories="Search;FullTextSearch"
                                                            CssClass="HContainer" Orientation="Horizontal" ContainerStyle="Buttons" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </cc3:XafUpdatePanel>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow ID="TableRow2" runat="server">
                                    <asp:TableCell runat="server" ID="ViewSite">
                                        <cc3:XafUpdatePanel ID="UPVSC" runat="server">
                                            <cc4:ViewSiteControl ID="VSC" runat="server" />
                                        </cc3:XafUpdatePanel>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </td>
                    </tr>
                </table>
                <table class="DockBottom" border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <cc3:XafUpdatePanel ID="UPPAC" runat="server">
                                <cc2:ActionContainerHolder runat="server" ID="PAC" ContainerStyle="Buttons"
                                    Orientation="Horizontal" PaintStyle="CaptionAndImage" Categories="PopupActions;Diagnostic">
                                    <menu width="100%" itemautowidth="False" horizontalalign="Right" />
                                </cc2:ActionContainerHolder>
                            </cc3:XafUpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
