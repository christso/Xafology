using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Xafology.ExpressApp.Win.NavBar
{
    public class SearchPanel : IDisposable
    {
        private NavBarControl navBarControl;
        private TextEdit textEdit;
        private SearchCriteria criteria1;

        public void Dispose()
        {
            if (navBarControl != null)
            {
                navBarControl.Dispose();
                navBarControl = null;
            }
        }

        public void CreateSearchPanel(NavBarControl navBarControl, SearchCriteria criteria)
        {
            if (navBarControl != null)
            {
                this.navBarControl = navBarControl;
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

        private void navBarControl_CustomDrawGroupCaption(object sender, DevExpress.XtraNavBar.ViewInfo.CustomDrawNavBarElementEventArgs e)
        {
            if (e.Caption == "Search")
            {
                Rectangle rect = e.RealBounds;
                rect.Inflate(-10, -5);

                e.Graphics.DrawString(e.Caption, e.Appearance.Font, Brushes.Black, rect);
                e.Handled = true;
            }
        }

        private void textEdit_EditValueChanged(object sender, EventArgs e)
        {
            foreach (NavBarGroup group in navBarControl.Groups)
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
