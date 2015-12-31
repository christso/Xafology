using Xafology.ExpressApp.Concurrency;
using Xafology.Utils.Data;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Xpo;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Xafology.ExpressApp.Xpo.Import.Parameters;
using Xafology.Utils;
using System.Collections;

namespace Xafology.ExpressApp.Xpo.Import.Logic.New
{
    public class HeadCsvToXpoRecordMapper
    {
        private readonly IXpoFieldMapper xpoFieldMapper;
        private readonly IEnumerable<HeaderToFieldMap> fieldMaps;
        private readonly CsvReader csvReader;

        public HeadCsvToXpoRecordMapper(IXpoFieldMapper xpoFieldMapper, IEnumerable<HeaderToFieldMap> fieldMaps,
            CsvReader csvReader)
        {
            this.xpoFieldMapper = xpoFieldMapper;
            this.fieldMaps = fieldMaps;
            this.csvReader = csvReader;
        }

        public void SetMemberValues(IXPObject targetObject, ITypeInfo objTypeInfo)
        {
            SetMemberValues(targetObject, GetTargetMembers(objTypeInfo));
        }

        public void SetMemberValues(IXPObject targetObject, IEnumerable<IMemberInfo> targetMembers)
        {
            foreach (var targetMember in targetMembers)
            {
                // find corresponding map for target member
                var map = fieldMaps
                    .FirstOrDefault(x => x.TargetName == targetMember.Name);

                xpoFieldMapper.SetMemberValue(targetObject, targetMember,
                    csvReader[map.SourceName], map.CreateMember);

            }
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
