using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Layout;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.Persistent.Validation;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxPanel;
using DevExpress.Web.ASPxRoundPanel;
using DevExpress.Web.ASPxTabControl;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Xafology.ExpressApp.Layout.Module.Web
{
    public class HeaderTemplateEx : ITemplate
    {
        private string _caption = "N/A";
        private ASPxPanel _panel;

        public HeaderTemplateEx(string caption, ASPxPanel panel)
        {
            _caption = caption;
            _panel = panel;
        }

        void ITemplate.InstantiateIn(Control container)
        {
            Table table = new Table();
            table.CellPadding = 0;
            table.CellSpacing = 0;
            table.Rows.Add(new TableRow());
            table.Rows[0].Cells.Add(new TableCell());
            table.Rows[0].Cells.Add(new TableCell());

            ASPxButton btnExpandCollapse = new ASPxButton();
            btnExpandCollapse.Text = "-";
            btnExpandCollapse.AllowFocus = false;
            btnExpandCollapse.AutoPostBack = false;
            btnExpandCollapse.Width = Unit.Pixel(20);
            btnExpandCollapse.FocusRectPaddings.Padding = Unit.Parse("0");

            btnExpandCollapse.ClientSideEvents.Click = "function (s,e) { " +
                "var isVisible = " + _panel.ClientInstanceName + ".GetVisible();\n" +
                "s.SetText(isVisible ? '+' : '-');\n" +
                _panel.ClientInstanceName + ".SetVisible(!isVisible);\n" +
                "}";

            table.Rows[0].Cells[0].Controls.Add(new LiteralControl(_caption));
            table.Rows[0].Cells[0].Style["white-space"] = "nowrap";

            table.Rows[0].Cells[1].Controls.Add(btnExpandCollapse);
            table.Rows[0].Cells[1].Style["width"] = "1%";
            table.Rows[0].Cells[1].Style["padding-left"] = "5px";

            container.Controls.Add(table);
        }
    }

    public class LayoutItemTemplateEx : LayoutItemTemplate
    {
        protected override Control CreateCaptionControl(LayoutItemTemplateContainer templateContainer)
        {
            Control baseControl = base.CreateCaptionControl(templateContainer);

            string icon = GetIcon(templateContainer);
            if (!string.IsNullOrEmpty(icon))
            {
                // TODO: specify character for mandatory marker
                Literal literal = (Literal)baseControl;
                literal.Text += icon;
                return literal;
                //return CreateItemIconTable(baseControl, icon);
            }
            else
            {
                return baseControl;
            }
        }

        private static string GetIcon(LayoutItemTemplateContainer templateContainer)
        {
            // LayoutItem ItemIcon

            var modelLayoutItemIcon = templateContainer.Model as IModelLayoutItemIcon;

            if (modelLayoutItemIcon != null && !string.IsNullOrEmpty(modelLayoutItemIcon.ItemIcon))
            {
                return modelLayoutItemIcon.ItemIcon;
            }

            // Member ItemIcon

            IModelMember modelMember = (templateContainer.Model.ViewItem as IModelPropertyEditor).ModelMember;
            IModelIcon modelIcon = modelMember as IModelIcon;
            if (modelIcon != null && !string.IsNullOrEmpty(modelIcon.ItemIcon))
            {
                return ((IModelIcon)modelMember).ItemIcon;
            }

            // RuleRequiredField

            var attr = modelMember.MemberInfo.FindAttribute<RuleRequiredFieldAttribute>();
            if (attr != null)
            {
                return "<span style=\"color:#FF0000\">*</span>";
            }

            return "";
        }
    }

    public class LayoutGroupTemplateEx : LayoutGroupTemplate
    {
        protected override void LayoutContentControls(LayoutGroupTemplateContainer templateContainer, IList<Control> controlsToLayout)
        {
            LayoutGroupTemplateContainer layoutGroupTemplateContainer = (LayoutGroupTemplateContainer)templateContainer;

            if (layoutGroupTemplateContainer.ShowCaption)
            {
                // Outer Panel for setting the default style
                ASPxPanel outerPanel = new ASPxPanel();
                outerPanel.Style.Add(HtmlTextWriterStyle.Padding, "10px 5px 10px 5px");

                // Round Panel containing the Content Panel
                ASPxRoundPanel roundPanel = new ASPxRoundPanel();
                roundPanel.Width = Unit.Percentage(100);
                roundPanel.ShowHeader = layoutGroupTemplateContainer.ShowCaption;
                if (layoutGroupTemplateContainer.HasHeaderImage)
                {
                    ASPxImageHelper.SetImageProperties(roundPanel.HeaderImage, layoutGroupTemplateContainer.HeaderImageInfo);
                }

                // Content Panel
                ASPxPanel contentPanel = new ASPxPanel();
                contentPanel.ClientInstanceName = templateContainer.Model.Id + "_ContentPanel";

                // Set the RoundPanel Header Template
                roundPanel.HeaderTemplate = new HeaderTemplateEx(layoutGroupTemplateContainer.Caption, contentPanel);

                // Populate the controls
                roundPanel.Controls.Add(contentPanel);
                outerPanel.Controls.Add(roundPanel);
                templateContainer.Controls.Add(outerPanel);
                foreach (Control control in controlsToLayout)
                {
                    contentPanel.Controls.Add(control);
                }

            }
            else
            {
                foreach (Control control in controlsToLayout)
                {
                    templateContainer.Controls.Add(control);
                }
            }
        }
    }

    public class TabbedGroupTemplateEx : TabbedGroupTemplate
    {
        protected override ASPxPageControl CreatePageControl(TabbedGroupTemplateContainer tabbedGroupTemplateContainer)
        {
            ASPxPageControl pageControl = new ASPxPageControl();
            pageControl.ID = WebIdHelper.GetCorrectedLayoutItemId(tabbedGroupTemplateContainer.Model, "", "_pg");
            pageControl.TabPosition = TabPosition.Left;
            pageControl.Width = Unit.Percentage(100);
            pageControl.ContentStyle.Paddings.Padding = Unit.Pixel(10);
            pageControl.ContentStyle.CssClass = "TabControlContent";
            return pageControl;
        }
    }
}
