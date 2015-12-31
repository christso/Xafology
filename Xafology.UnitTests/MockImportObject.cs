using DevExpress.Persistent.BaseImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpo;
namespace Xafology.UnitTests
{
    public class MockImportObject : BaseObject
    {
        private string description;
        private decimal amount;
 
        public MockImportObject(DevExpress.Xpo.Session session)
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

        private MockLookupObject mockLookupObject;

        public MockLookupObject MockLookupObject
        {
            get
            {
                return mockLookupObject;
            }
            set
            {
                SetPropertyValue("MockLookupObject", ref mockLookupObject, value);
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
