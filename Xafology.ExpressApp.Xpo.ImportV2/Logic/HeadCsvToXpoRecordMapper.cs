
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

namespace Xafology.ExpressApp.Xpo.Import.Logic
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

        public void SetMemberValues(IXPObject targetObject)
        {
            var typesInfo = XafTypesInfo.Instance.FindTypeInfo(targetObject.GetType());
            SetMemberValues(targetObject, 
                FieldMapsUtil.GetTargetMembers(fieldMaps, typesInfo));
        }

        public void SetMemberValues(IXPObject targetObject, IEnumerable<IMemberInfo> targetMembers)
        {
            foreach (var targetMember in targetMembers)
            {
                // find corresponding map for target member
                var map = fieldMaps
                    .FirstOrDefault(x => x.TargetName == targetMember.Name);

                xpoFieldMapper.SetMemberValue(targetObject, targetMember,
                    csvReader[map.SourceName], map.CreateMember, map.CacheObject);

            }
        }
    }
}
