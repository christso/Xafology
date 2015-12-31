using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.Xpo.Import.Parameters;
using System.Linq;

namespace Xafology.ExpressApp.Xpo.Import.Logic
{
    public class FieldMaps
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
                    RaiseDuplicateTargetMemberException(member);
                else if (targetCount == 0)
                    continue;
                else if (member.IsKey)
                    continue;
                // the below executes given condition: targetCount == 1 && !member.IsKey
                targetMembers.Add(member);
            }
            return targetMembers;
        }

        private static void RaiseDuplicateTargetMemberException(IMemberInfo member)
        {
            throw new UserFriendlyException("Duplicate maps were found for member '" + member.Name + "'");
        }

    }
}
