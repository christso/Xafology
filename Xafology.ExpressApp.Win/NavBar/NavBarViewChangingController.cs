using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraNavBar;
using DevExpress.ExpressApp.Win.Templates.ActionContainers;
using DevExpress.ExpressApp;

namespace Xafology.ExpressApp.Win.NavBar
{
    // This only supports ExplorerBar, and 2 levels in the hiearchy
    public class NavBarViewChangingController : WindowController
    {
        private NavigationActionContainer navActionContainer;
        protected override void OnActivated()
        {
            base.OnActivated();

            Window.ProcessActionContainer += Window_ProcessActionContainer;
        }
        protected override void OnDeactivated()
        {
            Window.ProcessActionContainer -= Window_ProcessActionContainer;
            UnsubscribeFromContainerEvents();
            navActionContainer = null;
            base.OnDeactivated();
        }
        void Window_ProcessActionContainer(object sender, ProcessActionContainerEventArgs e)
        {
            UnsubscribeFromContainerEvents();
            if (e.ActionContainer is NavigationActionContainer)
            {
                navActionContainer = e.ActionContainer as NavigationActionContainer;
                SubscribeToContainerEvents();
                SetupNavBar();
            }
        }
        private void SubscribeToContainerEvents()
        {
            if (navActionContainer != null)
            {
                navActionContainer.NavigationControlCreated += navigationControlCreated;
            }
        }
        private void UnsubscribeFromContainerEvents()
        {
            if (navActionContainer != null)
            {
                navActionContainer.NavigationControlCreated -= navigationControlCreated;
            }
        }
        private void navigationControlCreated(object sender, EventArgs e)
        {
            SetupNavBar();
        }
        private void SetupNavBar()
        {
            if (navActionContainer != null &&
                navActionContainer.NavigationControl is NavBarNavigationControl)
            {
                NavBarNavigationControl navBarNavigationControl =
                    navActionContainer.NavigationControl as NavBarNavigationControl;

                // set navbar style so that it supports the search panel
                navBarNavigationControl.SkinExplorerBarViewScrollStyle =
                    SkinExplorerBarViewScrollStyle.ScrollBar;
                navBarNavigationControl.PaintStyleKind = NavBarViewKind.ExplorerBar;

                // create the search panel
                SearchHelper.CreateSearchPanel(navBarNavigationControl, SearchCriteria.Contains);

            }
        }
    }
}
