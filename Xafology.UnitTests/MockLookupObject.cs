using DevExpress.Persistent.BaseImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpo;
namespace Xafology.UnitTests
{
    public class MockLookupObject : BaseObject
    {
        private string name;
        public MockLookupObject(DevExpress.Xpo.Session session)
            : base(session)
        {
            
        }
        public MockLookupObject()
        {
            
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                SetPropertyValue("Name", ref name, value);
            }
        }		

    }
}
