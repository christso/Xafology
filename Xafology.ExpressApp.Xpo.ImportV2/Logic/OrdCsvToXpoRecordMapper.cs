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
using Xafology.ExpressApp.Xpo.Import.Logic;

namespace Xafology.ExpressApp.Xpo.Import.Logic
{
    public class OrdCsvToXpoRecordMapper
    {
        private readonly IXpoFieldMapper xpoFieldMapper;
        private readonly IEnumerable<OrdinalToFieldMap> fieldMaps;
        private readonly CsvReader csvReader;

        public OrdCsvToXpoRecordMapper(IXpoFieldMapper xpoFieldMapper, IEnumerable<OrdinalToFieldMap> fieldMaps,
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
                    csvReader[map.SourceOrdinal], map.CreateMember);

            }
        }
    }
}
