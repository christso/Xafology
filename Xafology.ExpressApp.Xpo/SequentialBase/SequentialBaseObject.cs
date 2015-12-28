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

namespace Xafology.ExpressApp.Xpo.SequentialBase
{
    public interface ISupportSequentialNumber
    {
        int SequentialNumber { get; set; }
    }

    [NonPersistent]
    public abstract class SequentialBaseObject : BaseObject, ISupportSequentialNumber
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SequentialBaseObject(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        private int _sequentialNumber;

        [Indexed(Unique = false)]
        [Browsable(false)]
        public int SequentialNumber
        {
            get
            {
                return _sequentialNumber;
            }
            set
            {
                SetPropertyValue("SequentialNumber", ref _sequentialNumber, value);
            }
        }

        protected override void OnSaving()
        {
            Xafology.ExpressApp.Xpo.SequentialBase.DistributedIdGenerator.SetSequentialNumber(Session, this);
            base.OnSaving();
        }
    }
}
