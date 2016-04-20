using DevExpress.Persistent.BaseImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using Xafology.ExpressApp.BatchDelete;

namespace Xafology.BatchDemo.Module.BusinessObjects
{
    [DefaultClassOptions]
    [BatchDelete(isVisible: true, isOptimized: true)]
    public class MockFactObject : BaseObject, IBatchDeletable
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

        [Association("MockLookupObject1-MockFactObjects")]
        public MockLookupObject1 LookupObject1
        {
            get
            {
                return mockLookupObject1;
            }
            set
            {
                SetPropertyValue("LookupObject1", ref mockLookupObject1, value);
            }
        }

        private MockLookupObject2 mockLookupObject2;

        public MockLookupObject2 LookupObject2
        {
            get
            {
                return mockLookupObject2;
            }
            set
            {
                SetPropertyValue("LookupObject2", ref mockLookupObject2, value);
            }
        }		
    }
}
