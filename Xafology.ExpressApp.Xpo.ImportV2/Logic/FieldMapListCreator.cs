using DevExpress.Xpo;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.Xpo.Import.Parameters;

namespace Xafology.ExpressApp.Xpo.Import.Logic
{
    public class FieldMapListCreator
    {
        private readonly CsvReader csvReader;

        public FieldMapListCreator(CsvReader csvReader)
        {
            this.csvReader = csvReader;
        }

        public void AddFieldMaps(IList<HeaderToFieldMap> fieldMaps, Session session)
        {
            string[] headers = csvReader.GetFieldHeaders();
            foreach (var header in headers)
            {
                fieldMaps.Add(new HeaderToFieldMap(session)
                {
                    SourceName = header,
                    TargetName = header
                });
            }
            session.CommitTransaction();
        }

        public void AddFieldMaps(IList<OrdinalToFieldMap> fieldMaps, Session session)
        {
            for (int i = 0; i < csvReader.FieldCount; i++)
            {
                fieldMaps.Add(new OrdinalToFieldMap(session)
                {
                    SourceOrdinal = i,
                    TargetName = string.Format("Field_{0}", i)
                });
            }
            session.CommitTransaction();
        }
    }
}
