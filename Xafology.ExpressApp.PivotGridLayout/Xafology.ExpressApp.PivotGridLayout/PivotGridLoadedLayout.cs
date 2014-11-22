using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Security.Strategy;
using Xafology.ExpressApp.PivotGridLayout.Helpers;

namespace Xafology.ExpressApp.PivotGridLayout
{
    public class PivotGridLoadedLayout : BaseObject
    {

        private PivotGridSavedLayout _SavedLayout;
        private SecuritySystemUser _User;

        public PivotGridLoadedLayout(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            var currentUser = StaticHelpers.GetCurrentUser(Session);
            User = currentUser;
        }

        public PivotGridSavedLayout SavedLayout
        {
            get
            {
                return _SavedLayout;
            }
            set
            {
                SetPropertyValue("SavedLayout", ref _SavedLayout, value);
            }
        }

        public UIPlatform UIPlatform
        {
            get
            {
                if (_SavedLayout == null) return UIPlatform.All;
                return _SavedLayout.UIPlatform;
            }
        }

        [ModelDefault("AllowEdit", "False")]
        public SecuritySystemUser User
        {
            get
            {
                return _User;
            }
            set
            {
                SetPropertyValue("User", ref _User, value);
            }
        }

        public string TypeName
        {
            get
            {
                return SavedLayout.TypeName;
            }
        }
    }
}
