using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.Xpo.Import.Parameters;

namespace Xafology.ExpressApp.Xpo.Import.Logic
{
    public class XpoTargetMembers
    {
        private readonly IList fieldMaps;

        public XpoTargetMembers(IList fieldMaps)
        {
            this.fieldMaps = fieldMaps;
        }

        public List<IMemberInfo> GetList(ITypeInfo objTypeInfo)
        {
            var targetMembers = new List<IMemberInfo>();
            foreach (var member in objTypeInfo.Members)
            {
                
                var targetCount = fieldMaps.Cast<FieldMap>().Count(x => x.TargetName == (member.Name));
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
