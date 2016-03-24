using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.Xpo.Import;
using Xafology.ExpressApp.Xpo.Import.Parameters;
using DevExpress.Xpo;

namespace Xafology.ImportDemo.UnitTests
{
    public class ImportForexParam : ImportParamBase, ImportParam
    {
        public ImportForexParam(Session session)
            : base(session)
        {
            
        }
        public override FieldMaps FieldMaps
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
