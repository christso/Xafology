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
using Xafology.ExpressApp.Xpo;

namespace Xafology.IdentitySequenceDemo.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class DomainObject1 : IdentityBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (http://documentation.devexpress.com/#Xaf/CustomDocument3146).
        public DomainObject1(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (http://documentation.devexpress.com/#Xaf/CustomDocument2834).
        }

        // Fields...
        private string _Property1;

        public string Property1
        {
            get
            {
                return _Property1;
            }
            set
            {
                SetPropertyValue("Property1", ref _Property1, value);
            }
        }

        [PersistentAlias("concat('D', ToStr(SequentialNumber))")]
        public string DomainObjectId
        {
            get
            {
                return Convert.ToString(EvaluateAlias("DomainObjectId"));
            }
        }
    }
}
