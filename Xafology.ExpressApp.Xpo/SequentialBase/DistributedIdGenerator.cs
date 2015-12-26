using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Xpo.SequentialBase
{
    public class DistributedIdGenerator
    {
        public const int IDNOTASSIGNED = -1;

        public static int GenerateDistributedId(Session session, object obj)
        {
            if (!(session is NestedUnitOfWork) && session.IsNewObject(obj))
            {
                int nextSequence = DistributedIdGeneratorHelper.Generate(session.DataLayer, obj.GetType().FullName, string.Empty);
                return nextSequence;
            }
            return -1;
        }

        public static void SetSequentialNumber(Session session, Xafology.ExpressApp.Xpo.SequentialBase.ISupportSequentialNumber obj)
        {
            var tmpSequentialNumber = Xafology.ExpressApp.Xpo.SequentialBase.DistributedIdGenerator.GenerateDistributedId(session, obj);
            if (tmpSequentialNumber != Xafology.ExpressApp.Xpo.SequentialBase.DistributedIdGenerator.IDNOTASSIGNED)
            {
                obj.SequentialNumber = tmpSequentialNumber;
            }
        }
    }
}
