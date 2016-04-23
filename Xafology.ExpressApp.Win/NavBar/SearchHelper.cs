using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Win.NavBar
{
    public enum SearchCriteria
    {
        Contains,
        StartsWith,
        Equals
    }

    public static class SearchHelper
    {
        static NavBarControl navBarControl1;
        static TextEdit textEdit;
        static SearchCriteria criteria1;

        public static void CreateSearchPanel(NavBarControl navBarControl, SearchCriteria criteria)
        {
            if (navBarControl != null)
            {
                navBarControl1 = navBarControl;
                criteria1 = criteria;
                textEdit = new TextEdit();
                NavBarGroup navBarGroup = new NavBarGroup();
                navBarControl.Groups.Insert(0, navBarGroup);
                navBarGroup.GroupStyle = NavBarGroupStyle.ControlContainer;
                navBarGroup.Caption = "Search";

                navBarGroup.ControlContainer.Controls.Add(textEdit);
                textEdit.Dock = System.Windows.Forms.DockStyle.Fill;
                textEdit.Visible = true;
                navBarGroup.GroupClientHeight = 26;
                navBarGroup.Expanded = true;
                navBarControl.Groups.Add(navBarGroup);

                navBarControl.CustomDrawGroupCaption += navBarControl_CustomDrawGroupCaption;
                textEdit.EditValueChanged += textEdit_EditValueChanged;
            }
        }

        static void navBarControl_CustomDrawGroupCaption(object sender, DevExpress.XtraNavBar.ViewInfo.CustomDrawNavBarElementEventArgs e)
        {
            if (e.Caption == "Search")
            {
                Rectangle rect = e.RealBounds;
                rect.Inflate(-10, -5);

                e.Graphics.DrawString(e.Caption, e.Appearance.Font, Brushes.Black, rect);
                e.Handled = true;
            }
        }

        static void textEdit_EditValueChanged(object sender, EventArgs e)
        {
            foreach (NavBarGroup group in navBarControl1.Groups)
            {
                foreach (NavBarItemLink link in group.ItemLinks)
                {
                    switch (criteria1)
                    {
                        case SearchCriteria.Contains:
                            link.Visible = link.Caption.ToLower().Contains(textEdit.Text.ToLower());
                            break;
                        case SearchCriteria.StartsWith:
                            link.Visible = link.Caption.ToLower().StartsWith(textEdit.Text.ToLower());
                            break;
                        case SearchCriteria.Equals:
                            link.Visible = link.Caption.ToLower().Equals(textEdit.Text.ToLower());
                            break;
                    }
                    if (group.VisibleItemLinks.Count == 0)
                        group.Visible = false;
                    else
                        group.Visible = true;
                }
            }
        }
    }
}
