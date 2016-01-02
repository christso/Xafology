using DevExpress.Xpo;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.Xpo.Import.Parameters;

namespace Xafology.ExpressApp.Xpo.Import.Logic
{
    public class FieldMapListCreator
    {
        private readonly Stream stream;

        public FieldMapListCreator(Stream stream)
        {
            this.stream = stream;
        }
        
        /// <param name="fieldMaps">list that will be updated</param>
        public void AppendFieldMaps(Session session, IList<HeaderToFieldMap> fieldMaps)
        {
            using (var csvReader = new CsvReader(new StreamReader(stream), true))
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
        }

        /// <param name="fieldMaps">list that will be updated</param>
        public void AppendFieldMaps(Session session, IList<OrdinalToFieldMap> fieldMaps)
        {
            using (var csvReader = new CsvReader(new StreamReader(stream), false))
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
}
