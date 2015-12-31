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

namespace Xafology.ExpressApp.Xpo.Import.Logic
{
    public class OrdCsvToXpoUpdater : ICsvToXpoLoader
    {
        public OrdCsvToXpoUpdater(XafApplication application, ImportOrdinalsParam param, Stream stream)
        {
        }

        //public IXPObject GetTargetObject()
        //{
        //    IXPObject targetObject = (IXPObject)Activator.CreateInstance(_objTypeInfo.Type, param.Session);
        //}

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public List<IMemberInfo> GetTargetMembers(ITypeInfo objTypeInfo)
        {
            throw new NotImplementedException();
        }
    }
}
