using DevExpress.ExpressApp.DC;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using DevExpress.Data.Filtering;
using DevExpress.Xpo.Metadata;
using DevExpress.Xpo.Helpers;
namespace Xafology.ExpressApp.Xpo.Import.Parameters
{
    public class FieldMaps : IEnumerable<FieldMap>
    {
        private readonly IEnumerable<FieldMap> fieldMaps;

        public FieldMaps(IEnumerable fieldMaps)
        {
            this.fieldMaps = fieldMaps.Cast<FieldMap>();
        }

        public List<IMemberInfo> GetTargetMembers(ITypeInfo objTypeInfo)
        {
            var targetMembers = new List<IMemberInfo>();
            foreach (var member in objTypeInfo.Members)
            {
                var targetCount = fieldMaps.Count(x => x.TargetName == (member.Name));
                if (targetCount > 1)
                    throw new InvalidOperationException("Duplicate maps were found for member '" + member.Name + "'");
                else if (targetCount == 0)
                    continue;
                else if (member.IsKey)
                    continue;
                // the below executes given condition: targetCount == 1 && !member.IsKey
                targetMembers.Add(member);
            }
            return targetMembers;
        }

    }
}
