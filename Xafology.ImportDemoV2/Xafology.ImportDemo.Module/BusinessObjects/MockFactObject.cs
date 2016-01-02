using DevExpress.Persistent.BaseImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;

namespace Xafology.ImportDemo.Module.BusinessObjects
{
   [DefaultClassOptions]
    public class MockFactObject : BaseObject
    {
        private string description;
        private decimal amount;
 
        public MockFactObject(DevExpress.Xpo.Session session)
            : base(session)
        {

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
    }
}
