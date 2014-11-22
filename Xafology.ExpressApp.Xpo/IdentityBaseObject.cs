using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Xpo
{
    [NonPersistent]
    public abstract class IdentityBaseObject : BaseObject
    {

        public IdentityBaseObject(Session session)
            : base(session)
        {
            
        }

        [Custom("ExcludeFromUpdate", "true")]
        [Persistent("SequentialNumber")]
        private long _SequentialNumber;

        [PersistentAlias("_SequentialNumber")]
        [DbIdentity]
        [Browsable(false)]
        [Indexed(Unique = false)]
        public long SequentialNumber
        {
            get
            {
                return (long)EvaluateAlias("SequentialNumber");
            }
        }

        protected override void OnSaved()
        {
            base.OnSaved();
        }
    }
}
