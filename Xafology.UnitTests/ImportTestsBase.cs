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
using Xafology.ExpressApp.Concurrency;
using Xafology.ExpressApp.Xpo.Import;
using Xafology.ExpressApp;

namespace Xafology.UnitTests
{
    public abstract class ImportTestsBase : InMemoryDbTestBase
    {
         protected override void SetupObjects()
        {
            base.SetupObjects();

        }

        protected override void AddExportedTypes(DevExpress.ExpressApp.ModuleBase module)
        {
            module.AdditionalExportedTypes.Add(typeof(ImportHeadersParam));
            module.AdditionalExportedTypes.Add(typeof(MockImportObject));
            module.AdditionalExportedTypes.Add(typeof(MockLookupObject));
            module.AdditionalExportedTypes.Add(typeof(MockLookupObject2));
        }


        protected void SetupViewController(ViewController controller, IXPObject currentObject)
        {
            controller.Application = Application;
            var view = Application.CreateDetailView(ObjectSpace, currentObject);
            controller.SetView(view);
        }

        protected Stream GetMockCsvStream(string csvText)
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

            param.ObjectTypeName = "MockImportObject";

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
            map3.SourceName = "MockLookupObject";
            map3.TargetName = map3.SourceName;

            var param = ObjectSpace.CreateObject<ImportHeadersParam>();

            param.HeaderToFieldMaps.Add(map1);
            param.HeaderToFieldMaps.Add(map2);
            param.HeaderToFieldMaps.Add(map3);

            param.ObjectTypeName = "MockImportObject";

            return param;
        }
    }
}
