using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using MainDemo.Win.BusinessObjects;
using MainDemo.Win.CustomFunctions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.TestUtils;
using Xafology.Utils.SequentialGuid;

namespace MainDemo.Win
{
    [TestFixture]
    public class MainDemoTests : TestBase
    {
        public MainDemoTests()
        {
            SetTesterDbType(TesterDbType.InMemory);
        }

        public override void OnSetup()
        {
            CriteriaOperator.RegisterCustomFunction(new DaysFunction());
            CriteriaOperator.RegisterCustomFunction(new EoMonthFunction());
        }

        public override void OnAddExportedTypes(ModuleBase module)
        {
            module.AdditionalExportedTypes.Add(typeof(SeqDemoObject));
        }

        [Test]
        public void CustomFunctionTest()
        {
            var obj = ObjectSpace.CreateObject<SeqDemoObject>();
            obj.TranDate = new DateTime(2016, 3, 15);
            var tranDate2 = new DateTime(2016, 3, 31);
            
            var parsedResult = obj.Evaluate(CriteriaOperator.Parse("Days(EOMONTH(TranDate) - TranDate)"));
            Assert.NotNull(parsedResult);
        }
    }
}
