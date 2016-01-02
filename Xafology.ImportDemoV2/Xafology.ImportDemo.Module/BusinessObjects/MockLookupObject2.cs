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
    public class MockLookupObject2 : BaseObject
    {
        private string name;
        public MockLookupObject2(DevExpress.Xpo.Session session)
            : base(session)
        {
            
        }
        public MockLookupObject2()
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
