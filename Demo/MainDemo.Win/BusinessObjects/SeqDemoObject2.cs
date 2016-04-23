using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainDemo.Win.BusinessObjects
{
    [DefaultClassOptions]
    public class SeqDemoObject2 : BaseObject
    {
        public SeqDemoObject2(Session session)
            : base(session)
        {

        }

        public string Instructions
        {
            get
            {
                return "Create a new object and the SequentialNumber will automatically increment by 1.";
            }
        }

        private int _sequentialNumber;

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
            var tmpSequentialNumber = GenerateDistributedId(Session, this);
            if (tmpSequentialNumber >= 0)
            {
                SequentialNumber = tmpSequentialNumber;
            }
            base.OnSaving();
        }

        private static int GenerateDistributedId(Session session, object obj)
        {
            if (!(session is NestedUnitOfWork) && session.IsNewObject(obj))
            {
                int nextSequence = DistributedIdGeneratorHelper.Generate(session.DataLayer, obj.GetType().FullName, string.Empty);
                return nextSequence;
            }
            return -1;
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
