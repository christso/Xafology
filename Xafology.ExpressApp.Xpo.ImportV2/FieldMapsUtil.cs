using DevExpress.ExpressApp.DC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.Xpo.Import.Parameters;

namespace Xafology.ExpressApp.Xpo.Import
{
    public class FieldMapsUtil
    {
        public static List<IMemberInfo> GetTargetMembers(IEnumerable fieldMaps, ITypeInfo objTypeInfo)
        {
            var targetMembers = new List<IMemberInfo>();
            foreach (var member in objTypeInfo.Members)
            {
                var targetCount = fieldMaps.Cast<FieldMap>().Count(x => x.TargetName == (member.Name));
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

        public static void ValidateParameters(ImportParam param)
        {
            if (param.ObjectTypeInfo == null)
                throw new ArgumentException(
                    string.Format("ObjectTypeInfo cannot be null. "
                    + "This may also be because ObjectTypeName '{0}' does not match any business objects",
                    param.ObjectTypeName),
                    "ObjectTypeInfo");
        }


    }
}
