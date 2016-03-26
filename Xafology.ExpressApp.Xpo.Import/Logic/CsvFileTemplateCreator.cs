using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xafology.Utils;

namespace Xafology.ExpressApp.Xpo.Import.Logic
{
    public class CsvFileTemplateCreator
    {
        public static Stream CreateStream(ITypeInfo objTypeInfo)
        {
            var templateMemberNames = new List<string>();

            foreach (var m in objTypeInfo.Members)
            {
                if (m.IsVisible && m.IsPersistent && !m.IsReadOnly)
                {
                    templateMemberNames.Add(m.Name);
                }
            }
            return string.Join(",", templateMemberNames).ToStream();
        }
    }
}
