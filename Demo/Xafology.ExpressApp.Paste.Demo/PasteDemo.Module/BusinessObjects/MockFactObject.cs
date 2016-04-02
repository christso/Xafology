using DevExpress.Persistent.BaseImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;

namespace PasteDemo.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultListViewOptions(allowEdit: true, newItemRowPosition: DevExpress.ExpressApp.NewItemRowPosition.Top)]
    public class MockFactObject : BaseObject
    {
        private string description;
        private decimal amount;
        private DateTime tranDate;
        private string memo;

        public MockFactObject(DevExpress.Xpo.Session session)
            : base(session)
        {

        }

        [NonPersistent]
        public string ShortUID
        {
            get
            {
                return Convert.ToString(Oid).Substring(0, 8);
            }
        }

        [ModelDefault("DisplayFormat", "dd-MMM-yy")]
        [RuleRequiredField("MockFactObject.TranDate_RuleRequiredField", DefaultContexts.Save)]
        public DateTime TranDate
        {
            get
            {
                return tranDate;
            }
            set
            {
                SetPropertyValue("TranDate", ref tranDate, value.Date);
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                SetPropertyValue("Description", ref description, value);
            }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Memo
        {
            get
            {
                return memo;
            }
            set
            {
                SetPropertyValue("Memo", ref memo, value);
            }
        }

        public decimal Amount
        {
            get
            {
                return amount;
            }
            set
            {
                SetPropertyValue("Amount", ref amount, value);
            }
        }

        private MockLookupObject1 mockLookupObject1;
        public MockLookupObject1 MockLookupObject1
        {
            get
            {
                return mockLookupObject1;
            }
            set
            {
                SetPropertyValue("MockLookupObject1", ref mockLookupObject1, value);
            }
        }


        private MockLookupObject2 mockLookupObject2;
        public MockLookupObject2 MockLookupObject2
        {
            get
            {
                return mockLookupObject2;
            }
            set
            {
                SetPropertyValue("MockLookupObject2", ref mockLookupObject2, value);
            }
        }

        private Status status;
        public Status Status
        {
            get
            {
                return status;
            }
            set
            {
                SetPropertyValue("Status", ref status, value);
            }
        }

    }

    public enum Status
    {
        Status0 = 0,
        Status1 = 1
    }
}
