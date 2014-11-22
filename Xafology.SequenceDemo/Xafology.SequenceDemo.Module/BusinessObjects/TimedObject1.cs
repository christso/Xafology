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

namespace Xafology.SequenceDemo.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class TimedObject1 : BaseObject
    {
        public TimedObject1(Session session)
            : base(session)
        {
            
        }
        private DateTime timeKey;
        public DateTime TimeKey
        {
            get
            {
                return timeKey;
            }
            set
            {
            	SetPropertyValue("TimeKey", ref timeKey, value);
            }
        }

        protected override void OnSaving()
        {
            SetPropertyValue("TimeKey", ref timeKey, DateTime.Now);
            base.OnSaving();
        }

    }
}
