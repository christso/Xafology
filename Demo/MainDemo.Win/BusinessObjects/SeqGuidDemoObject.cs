using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.Xpo.SequentialGuidBase;

namespace MainDemo.Win.BusinessObjects
{
    [DefaultClassOptions]
    public class SeqGuidDemoObject : SequentialGuidBaseObject
    {
        public SeqGuidDemoObject(Session session) : base(session)
        { 

        }

        private DateTime _TranDate;
        public DateTime TranDate
        {
            get
            {
                return _TranDate;
            }
            set
            {
                SetPropertyValue("TranDate", ref _TranDate, value.Date);
            }
        }

        private decimal _AccountCcyAmt;

        public decimal AccountCcyAmt
        {
            get
            {
                return _AccountCcyAmt;
            }
            set
            {
                SetPropertyValue("AccountCcyAmt", ref _AccountCcyAmt, Math.Round(value, 2));
            }
        }

    }
}
