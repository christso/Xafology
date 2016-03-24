using DevExpress.ExpressApp;
using DevExpress.Xpo;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.Xpo.Import.Parameters;
using Xafology.TestUtils;
using Xafology.ExpressApp.Xpo.Import.Logic;

using Xafology.ExpressApp.Xpo.Import;
using Xafology.ExpressApp;

namespace Xafology.UnitTests.Import
{
    public abstract class ImportTestsBase : TestBase
    {
        public override void OnAddExportedTypes(DevExpress.ExpressApp.ModuleBase module)
        {
            module.AdditionalExportedTypes.Add(typeof(ImportHeadersParam));
            module.AdditionalExportedTypes.Add(typeof(ImportOrdinalsParam));
            module.AdditionalExportedTypes.Add(typeof(ImportForexParam));
            module.AdditionalExportedTypes.Add(typeof(MockFactObject));
            module.AdditionalExportedTypes.Add(typeof(MockLookupObject1));
            module.AdditionalExportedTypes.Add(typeof(MockLookupObject2));
            module.AdditionalExportedTypes.Add(typeof(ImportRequest));
            module.AdditionalExportedTypes.Add(typeof(HeaderToFieldMap));
            
        }

        protected void SetupViewController(ViewController controller, IXPObject currentObject)
        {
            controller.Application = Application;
            var view = Application.CreateDetailView(ObjectSpace, currentObject);
            controller.SetView(view);
        }

        protected Stream ConvertToCsvStream(string csvText)
        {
            byte[] csvBytes = Encoding.UTF8.GetBytes(csvText);
            return new MemoryStream(csvBytes);
        }

        protected ImportHeadersParam GetHeadMockParamObject()
        {
            var map1 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map1.SourceName = "Description";
            map1.TargetName = map1.SourceName;

            var map2 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map2.SourceName = "Amount";
            map2.TargetName = map2.SourceName;

            var param = ObjectSpace.CreateObject<ImportHeadersParam>();

            param.HeaderToFieldMaps.Add(map1);
            param.HeaderToFieldMaps.Add(map2);

            param.ObjectTypeName = "MockFactObject";

            return param;
        }

        protected ImportHeadersParam GetHeadMockParamObjectForLookup()
        {
            var map1 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map1.SourceName = "Description";
            map1.TargetName = map1.SourceName;

            var map2 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map2.SourceName = "Amount";
            map2.TargetName = map2.SourceName;

            var map3 = ObjectSpace.CreateObject<HeaderToFieldMap>();
            map3.SourceName = "MockLookupObject1";
            map3.TargetName = map3.SourceName;

            var param = ObjectSpace.CreateObject<ImportHeadersParam>();

            param.HeaderToFieldMaps.Add(map1);
            param.HeaderToFieldMaps.Add(map2);
            param.HeaderToFieldMaps.Add(map3);

            param.ObjectTypeName = "MockFactObject";

            return param;
        }
    }
}
